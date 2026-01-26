<script setup lang="ts">
import type { RecipeCard, RecipeContainer } from '@/models/recipe';
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
import { Navigation, Keyboard, Mousewheel, Controller } from 'swiper/modules';
import 'swiper/css';
import 'swiper/css/navigation';
import type { Swiper as SwiperType } from 'swiper/types';

const recipes = ref<RecipeContainer[]>([]);
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

const controlledSwiper = ref<SwiperType | null>(null);
const setControlledSwiper = (swiper: any) => {
  controlledSwiper.value = swiper;
};

onMounted(() => {
  fetch(API_ENDPOINTS.RECIPE_CARD, {
    method: 'GET'
  })
    .then(response => {
      response.json().then(res => {
        const cards: RecipeCard[] = res;

        recipes.value = cards.map(x => {
          return {
            recipeCard: x,
            isSelected: false
          } as RecipeContainer
        });
      });
    })
    .catch(err => {
      console.error(err);
    });
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
    .then((res: RecipeCard) => {
      const newRecipe: RecipeContainer = {
        recipeCard: res,
        isSelected: false
      };
      recipes.value.unshift(newRecipe);

      controlledSwiper.value?.slideTo(0);
      selectCard(0);

      showSuccessAlert.value = true;
    })
    .catch(err => {
      console.error(err);
      errorMessage.value = err.message || 'Failed to import recipe. Please try again.';
      showErrorAlert.value = true;
    });
}

function selectCard(index: number) {
  recipes.value.forEach((recipe, i) => {
    if (recipe.isSelected) {
      recipe.isSelected = false;
    } else {
      recipe.isSelected = index === i;
    }
  });
  selectedRecipe.value = recipes.value[index];
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

    // Find index of deleted recipe
    const deletedIndex = recipes.value.findIndex(r => r.recipeCard.id === recipeId);

    // Clear selection if deleting selected recipe
    if (selectedRecipe.value?.recipeCard.id === recipeId) {
      selectedRecipe.value = null;
    }

    // Remove from array
    recipes.value.splice(deletedIndex, 1);

    // Auto-select adjacent recipe for smooth UX
    if (recipes.value.length > 0) {
      const newSelectedIndex = deletedIndex > 0 ? deletedIndex - 1 : 0;
      if (newSelectedIndex < recipes.value.length) {
        controlledSwiper.value?.slideTo(newSelectedIndex);
        selectCard(newSelectedIndex);
      }
    }

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
    <div class="float-left mr-12">
      <RecipesAddCard @add-recipe="showAddNewModal = true" />
    </div>
    <swiper @swiper="setControlledSwiper" :slidesPerView="'auto'" :spaceBetween="30" :loop="false" :navigation="true"
      :mousewheel="true" :keyboard="{
        enabled: true,
      }" :modules="[Navigation, Keyboard, Mousewheel, Controller]" class="mySwiper">
      <swiper-slide v-for="(recipe, index) in recipes" :key="recipe.recipeCard.id" class="w-52">
        <RecipeCardTemplate :recipe="recipe" @recipe-clicked="selectCard(index)" @delete-recipe="initiateDelete"></RecipeCardTemplate>
      </swiper-slide>
    </swiper>
  </div>
  <RecipeDetailsView :recipe="selectedRecipe" />
  <RecipesAddNewModal v-if="showAddNewModal" @cancel-recipe="showAddNewModal = false"
    @import-recipe="(url: string) => importRecipe(url)"></RecipesAddNewModal>
  <RecipesAddSuccessAlert v-if="showSuccessAlert" @dismiss-alert="showSuccessAlert = false" title="Recipe imported"
    body="New recipe added!"></RecipesAddSuccessAlert>
  <RecipesAddErrorAlert v-if="showErrorAlert" @dismiss-alert="showErrorAlert = false" title="Import failed"
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
</template>

<style scoped>
.swiper-slide {
  width: 13rem;
}
</style>
