<script setup lang="ts">
import type { RecipeCard, RecipeContainer } from '@/models/recipe';
import type { Category } from '@/models/category';
import { onMounted, ref, computed, watch } from 'vue'
import { useRoute } from 'vue-router';
import RecipeCardTemplate from '../RecipesPage/RecipesCard.vue'
import RecipesAddCard from '../RecipesPage/RecipesAddCard.vue';
import RecipesAddNewModal from '../RecipesPage/RecipesAddNewModal.vue';
import RecipesAddSuccessAlert from '../RecipesPage/RecipesAddSuccessAlert.vue';
import RecipesAddErrorAlert from '../RecipesPage/RecipesAddErrorAlert.vue';
import RecipesDeleteConfirmModal from '../RecipesPage/RecipesDeleteConfirmModal.vue';
import RecipesDeleteSuccessAlert from '../RecipesPage/RecipesDeleteSuccessAlert.vue';
import RecipeDetailsView from '../RecipesPage/RecipeDetailsView.vue';
import { API_ENDPOINTS } from '@/config/api';


import { Swiper, SwiperSlide } from 'swiper/vue';
import { Navigation, Keyboard, Mousewheel } from 'swiper/modules';
import 'swiper/css';
import 'swiper/css/navigation';

const route = useRoute();

const allRecipes = ref<RecipeContainer[]>([]);
const recipesByCategory = ref<Record<string, RecipeContainer[]>>({});
const categoryOrder = ref<string[]>([]);
const categoryColors = ref<Record<string, string>>({});
const showAddNewModal = ref(false);
const showSuccessAlert = ref(false);
const showErrorAlert = ref(false);
const errorMessage = ref('');
const selectedRecipe = ref<RecipeContainer | null>(null);
const selectedCarousel = ref<string | null>(null); // Track which carousel was selected

// Delete state
const showDeleteConfirmModal = ref(false);
const recipeToDelete = ref<RecipeContainer | null>(null);
const showDeleteSuccessAlert = ref(false);
const deleteSuccessMessage = ref('');

onMounted(async () => {
  await loadRecipes();
});

async function loadRecipes() {
  try {
    const response = await fetch(API_ENDPOINTS.RECIPE_CARDS_GROUPED);
    const grouped: Record<string, RecipeCard[]> = await response.json();

    // Convert to RecipeContainer structure
    recipesByCategory.value = {};
    categoryOrder.value = [];
    allRecipes.value = [];

    // Sort: user categories alphabetically, then "Uncategorized" last
    const categories = Object.keys(grouped).sort((a, b) => {
      if (a === 'Uncategorized') return 1;
      if (b === 'Uncategorized') return -1;
      return a.localeCompare(b);
    });

    categories.forEach(categoryName => {
      categoryOrder.value.push(categoryName);
      recipesByCategory.value[categoryName] = grouped[categoryName].map(card => ({
        recipeCard: card,
        isSelected: false
      }));

      // Add to all recipes
      allRecipes.value.push(...recipesByCategory.value[categoryName]);
    });

    // Fetch category metadata for colors
    const categoriesResponse = await fetch(API_ENDPOINTS.CATEGORY);
    const categoriesData: Category[] = await categoriesResponse.json();
    categoriesData.forEach(cat => {
      categoryColors.value[cat.name] = cat.color ?? '#6B7280';
    });
    // Default color for Uncategorized
    categoryColors.value['Uncategorized'] = '#6B7280';

  } catch (err) {
    console.error('Failed to load recipes:', err);
  }
}

/**
 * Filters a recipe based on search term across name, description, and ingredients
 * @param recipe RecipeContainer to filter
 * @param searchTerm Search term (case-insensitive)
 * @returns true if recipe matches search criteria
 */
function matchesSearch(recipe: RecipeContainer, searchTerm: string): boolean {
  if (!searchTerm) return true; // No search term means show all

  const lowerSearch = searchTerm.toLowerCase();
  const card = recipe.recipeCard;

  // Search in name
  if (card.name?.toLowerCase().includes(lowerSearch)) {
    return true;
  }

  // Search in description
  if (card.description?.toLowerCase().includes(lowerSearch)) {
    return true;
  }

  // Search in ingredients array
  if (card.recipeIngredient?.some(ingredient =>
    ingredient.toLowerCase().includes(lowerSearch)
  )) {
    return true;
  }

  return false;
}

// Computed property for current search term
const currentSearch = computed(() => {
  const search = route.query.search;
  return typeof search === 'string' ? search.trim() : '';
});

// Filtered "All Recipes" carousel
const filteredAllRecipes = computed(() => {
  const searchTerm = currentSearch.value;
  if (!searchTerm) return allRecipes.value;

  return allRecipes.value.filter(recipe => matchesSearch(recipe, searchTerm));
});

// Filtered recipes by category
const filteredRecipesByCategory = computed(() => {
  const searchTerm = currentSearch.value;
  if (!searchTerm) return recipesByCategory.value;

  const filtered: Record<string, RecipeContainer[]> = {};

  for (const [categoryName, recipes] of Object.entries(recipesByCategory.value)) {
    const categoryFiltered = recipes.filter(recipe => matchesSearch(recipe, searchTerm));
    if (categoryFiltered.length > 0) {
      filtered[categoryName] = categoryFiltered;
    }
  }

  return filtered;
});

// Visible categories (only show categories with matches)
const visibleCategories = computed(() => {
  const searchTerm = currentSearch.value;
  if (!searchTerm) return categoryOrder.value;

  return categoryOrder.value.filter(categoryName =>
    filteredRecipesByCategory.value[categoryName] !== undefined
  );
});

// Computed for showing "no results" message
const hasSearchResults = computed(() => {
  return filteredAllRecipes.value.length > 0;
});

// Watch for when selected recipe gets filtered out
watch([filteredAllRecipes, currentSearch], () => {
  if (selectedRecipe.value && currentSearch.value) {
    const stillVisible = filteredAllRecipes.value.some(
      r => r.recipeCard.id === selectedRecipe.value?.recipeCard.id
    );

    if (!stillVisible) {
      // Clear selection if filtered out
      selectedRecipe.value = null;
      selectedCarousel.value = null;
    }
  }
});

function importRecipe(recipeUrl: string) {
  showAddNewModal.value = false;

  const encodedUrl = encodeURI(recipeUrl);

  fetch(`${API_ENDPOINTS.RECIPE_IMPORT}?url=${encodedUrl}`, {
    method: 'GET'
  })
    .then(response => {
      if (!response.ok) {
        throw new Error(`Failed to import recipe: ${response.statusText}`);
      }
      return response.json();
    })
    .then(async () => {
      // Reload recipes to show in correct carousels
      await loadRecipes();
      showSuccessAlert.value = true;
    })
    .catch(err => {
      console.error(err);
      errorMessage.value = err.message || 'Failed to import recipe. Please try again.';
      showErrorAlert.value = true;
    });
}

function selectCard(categoryName: string | null, recipe: RecipeContainer) {
  // Deselect all recipes in all categories
  allRecipes.value.forEach(r => r.isSelected = false);

  // Find and select the recipe in the original unfiltered array
  const recipeToSelect = allRecipes.value.find(r =>
    r.recipeCard.id === recipe.recipeCard.id
  );

  if (recipeToSelect) {
    recipeToSelect.isSelected = true;
    selectedRecipe.value = recipeToSelect;
    selectedCarousel.value = categoryName === null ? 'all' : categoryName;
  }
}

// Delete flow functions
function initiateDelete(recipe: RecipeContainer) {
  recipeToDelete.value = recipe;
  showDeleteConfirmModal.value = true;
}

function cancelDelete() {
  showDeleteConfirmModal.value = false;
  recipeToDelete.value = null;
}

async function confirmDelete() {
  if (!recipeToDelete.value) return;

  const recipeId = recipeToDelete.value.recipeCard.id;
  const recipeName = recipeToDelete.value.recipeCard.name;

  showDeleteConfirmModal.value = false;

  try {
    const response = await fetch(API_ENDPOINTS.DELETE_RECIPE_CARD(recipeId), {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Failed to delete recipe: ${response.statusText}`);
    }

    // Clear selection if deleting selected recipe
    if (selectedRecipe.value?.recipeCard.id === recipeId) {
      selectedRecipe.value = null;
    }

    // Reload grouped data
    await loadRecipes();

    // Show success alert
    deleteSuccessMessage.value = `"${recipeName}" deleted successfully`;
    showDeleteSuccessAlert.value = true;

  } catch (err: any) {
    console.error(err);
    errorMessage.value = err.message || 'Failed to delete recipe. Please try again.';
    showErrorAlert.value = true;
  } finally {
    recipeToDelete.value = null;
  }
}

</script>

<template>
  <div>
    <!-- All Recipes Carousel -->
    <div class="mb-12">
      <h2 class="text-2xl font-bold mb-4 flex items-center gap-2">
        All Recipes
        <span class="text-gray-500 text-base font-normal">
          ({{ filteredAllRecipes.length }})
        </span>
        <span v-if="currentSearch" class="text-sm text-blue-600 font-normal">
          - searching for "{{ currentSearch }}"
        </span>
      </h2>

      <!-- No results message -->
      <div v-if="currentSearch && !hasSearchResults"
           class="bg-yellow-50 border border-yellow-200 rounded-lg p-6 text-center">
        <p class="text-gray-700 text-lg">
          No recipes found matching "{{ currentSearch }}"
        </p>
        <p class="text-gray-600 text-sm mt-2">
          Try a different search term or
          <button @click="$router.push({ query: {} })"
                  class="text-blue-600 hover:text-blue-800 underline">
            clear search
          </button>
        </p>
      </div>

      <!-- Recipe carousel (show only if has results or no search) -->
      <template v-else>
        <!-- Add Recipe Button - full width on mobile, floated on larger screens -->
        <div class="mb-4 md:mb-0 md:float-left md:mr-12">
          <RecipesAddCard @add-recipe="showAddNewModal = true" />
        </div>

        <swiper
          :slidesPerView="'auto'"
          :spaceBetween="30"
          :loop="false"
          :navigation="true"
          :mousewheel="true"
          :keyboard="{ enabled: true }"
          :modules="[Navigation, Keyboard, Mousewheel]"
          class="mySwiper">

          <!-- All recipe cards -->
          <swiper-slide
            v-for="recipe in filteredAllRecipes"
            :key="recipe.recipeCard.id"
            class="w-52">
            <RecipeCardTemplate
              :recipe="recipe"
              @recipe-clicked="selectCard(null, recipe)"
              @delete-recipe="initiateDelete">
            </RecipeCardTemplate>
          </swiper-slide>
        </swiper>

        <!-- Show recipe details if selected from "All Recipes" carousel -->
        <RecipeDetailsView
          v-if="selectedCarousel === 'all' && selectedRecipe"
          :recipe="selectedRecipe"
          @categories-updated="loadRecipes" />
      </template>
    </div>

    <!-- Iterate over visible categories only -->
    <div v-for="categoryName in visibleCategories" :key="categoryName" class="mb-12">
      <h2 class="text-2xl font-bold mb-4 flex items-center gap-2">
        <span
          class="w-4 h-4 rounded-full"
          :style="{ backgroundColor: categoryColors[categoryName] }"></span>
        {{ categoryName }}
        <span class="text-gray-500 text-base font-normal">
          ({{ filteredRecipesByCategory[categoryName].length }})
        </span>
      </h2>

      <swiper
        :slidesPerView="'auto'"
        :spaceBetween="30"
        :loop="false"
        :navigation="true"
        :mousewheel="true"
        :keyboard="{ enabled: true }"
        :modules="[Navigation, Keyboard, Mousewheel]"
        class="mySwiper">

        <swiper-slide
          v-for="recipe in filteredRecipesByCategory[categoryName]"
          :key="recipe.recipeCard.id"
          class="w-52">
          <RecipeCardTemplate
            :recipe="recipe"
            @recipe-clicked="selectCard(categoryName, recipe)"
            @delete-recipe="initiateDelete">
          </RecipeCardTemplate>
        </swiper-slide>
      </swiper>

      <!-- Show recipe details if selected from this category carousel -->
      <RecipeDetailsView
        v-if="selectedCarousel === categoryName && selectedRecipe"
        :recipe="selectedRecipe"
        @categories-updated="loadRecipes" />
    </div>

    <!-- Modals and alerts -->
    <RecipesAddNewModal
      v-if="showAddNewModal"
      @cancel-recipe="showAddNewModal = false"
      @import-recipe="(url: string) => importRecipe(url)"></RecipesAddNewModal>

    <RecipesAddSuccessAlert
      v-if="showSuccessAlert"
      @dismiss-alert="showSuccessAlert = false"
      title="Recipe imported"
      body="New recipe added!"></RecipesAddSuccessAlert>

    <RecipesAddErrorAlert
      v-if="showErrorAlert"
      @dismiss-alert="showErrorAlert = false"
      title="Import failed"
      :body="errorMessage"></RecipesAddErrorAlert>

    <!-- Delete modals/alerts -->
    <RecipesDeleteConfirmModal
      v-if="showDeleteConfirmModal && recipeToDelete"
      :recipe="recipeToDelete"
      @confirm-delete="confirmDelete"
      @cancel-delete="cancelDelete" />

    <RecipesDeleteSuccessAlert
      v-if="showDeleteSuccessAlert"
      @dismiss-alert="showDeleteSuccessAlert = false"
      title="Recipe deleted"
      :body="deleteSuccessMessage" />
  </div>
</template>

<style scoped>
.swiper-slide {
  width: 13rem;
}
</style>
