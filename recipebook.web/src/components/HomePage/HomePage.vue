<script setup lang="ts">
import type { RecipeCard, RecipeContainer } from '@/models/recipe';
import type { Category } from '@/models/category';
import { onMounted, ref } from 'vue'
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

const recipesByCategory = ref<Record<string, RecipeContainer[]>>({});
const categoryOrder = ref<string[]>([]);
const categoryColors = ref<Record<string, string>>({});
const showAddNewModal = ref(false);
const showSuccessAlert = ref(false);
const showErrorAlert = ref(false);
const errorMessage = ref('');
const selectedRecipe = ref<RecipeContainer | null>(null);

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

function selectCard(categoryName: string, index: number) {
  // Deselect all recipes in all categories
  Object.values(recipesByCategory.value).forEach(recipes => {
    recipes.forEach(r => r.isSelected = false);
  });

  // Select the clicked recipe
  recipesByCategory.value[categoryName][index].isSelected = true;
  selectedRecipe.value = recipesByCategory.value[categoryName][index];
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
    <!-- Add Recipe Card -->
    <div class="mb-8">
      <RecipesAddCard @add-recipe="showAddNewModal = true" />
    </div>

    <!-- Iterate over categories -->
    <div v-for="categoryName in categoryOrder" :key="categoryName" class="mb-12">
      <h2 class="text-2xl font-bold mb-4 flex items-center gap-2">
        <span
          class="w-4 h-4 rounded-full"
          :style="{ backgroundColor: categoryColors[categoryName] }"></span>
        {{ categoryName }}
        <span class="text-gray-500 text-base font-normal">
          ({{ recipesByCategory[categoryName].length }})
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
          v-for="(recipe, index) in recipesByCategory[categoryName]"
          :key="recipe.recipeCard.id"
          class="w-52">
          <RecipeCardTemplate
            :recipe="recipe"
            @recipe-clicked="selectCard(categoryName, index)"
            @delete-recipe="initiateDelete">
          </RecipeCardTemplate>
        </swiper-slide>
      </swiper>
    </div>

    <RecipeDetailsView
      :recipe="selectedRecipe"
      @categories-updated="loadRecipes" />

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
