<script setup lang="ts">
import type { RecipeCard, RecipeContainer } from '@/models/recipe';
import { onMounted, ref } from 'vue'
import RecipeCardTemplate from '../RecipesPage/RecipesCard.vue'
import RecipesAddCard from '../RecipesPage/RecipesAddCard.vue';
import RecipesAddNewModal from '../RecipesPage/RecipesAddNewModal.vue';
import RecipesAddSuccessAlert from '../RecipesPage/RecipesAddSuccessAlert.vue';


import { Swiper, SwiperSlide } from 'swiper/vue';
import { Navigation, Keyboard, Mousewheel, Controller } from 'swiper/modules';
import 'swiper/css';
import 'swiper/css/navigation';
import type { Swiper as SwiperType } from 'swiper/types';

const recipes = ref<RecipeContainer[]>([]);
const showAddNewModal = ref(false);
const showSuccessAlert = ref(false);

const controlledSwiper = ref<SwiperType | null>(null);
const setControlledSwiper = (swiper: any) => {
  controlledSwiper.value = swiper;
};

onMounted(() => {
  fetch('https://localhost:52167/RecipeCard', {
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

  // if (!recipeUrl) {
  //   alert("Invalid recipe url");
  //   return;
  // }

  const encodedUrl = encodeURI(recipeUrl);


  // fetch(`https://localhost:52167/RecipeImport?url=${encodedUrl}`, {
  //   method: 'GET'
  // })
  //   .then(response => {
  //     response.json().then(res => {
  //       recipes.value.unshift(res);

  //       showSuccessAlert.value = true;
  //     });
  //   })
  //   .catch(err => {
  //     console.error(err);
  //   });

  controlledSwiper.value?.slideTo(0);
  selectCard(0);

  showSuccessAlert.value = true;
}

function selectCard(index: number) {
  recipes.value.forEach((recipe, i) => {
    if (recipe.isSelected) {
      recipe.isSelected = false;
    } else {
      recipe.isSelected = index === i;
    }
  });
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
        <RecipeCardTemplate :recipe="recipe" @recipe-clicked="selectCard(index)"></RecipeCardTemplate>
      </swiper-slide>
    </swiper>
  </div>
  <RecipesAddNewModal v-if="showAddNewModal" @cancel-recipe="showAddNewModal = false"
    @import-recipe="(url: string) => importRecipe(url)"></RecipesAddNewModal>
  <RecipesAddSuccessAlert v-if="showSuccessAlert" @dismiss-alert="showSuccessAlert = false" title="Recipe imported"
    body="New recipe added!"></RecipesAddSuccessAlert>
</template>

<style scoped>
.swiper-slide {
  width: 13rem;
}
</style>
