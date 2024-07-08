<script setup lang="ts">
import type { RecipeCard } from '@/models/recipe';
import { onMounted, ref } from 'vue'
import RecipeCardTemplate from '../RecipesPage/RecipesCard.vue'
import RecipesAddCard from '../RecipesPage/RecipesAddCard.vue';
import RecipesAddNewModal from '../RecipesPage/RecipesAddNewModal.vue';
import RecipesAddSuccessAlert from '../RecipesPage/RecipesAddSuccessAlert.vue';


import { Swiper, SwiperSlide } from 'swiper/vue';
import { Navigation, Keyboard, Mousewheel } from 'swiper/modules';
import 'swiper/css';
import 'swiper/css/navigation';

const recipes = ref<RecipeCard[]>([]);
const showAddNewModal = ref(false);
const showSuccessAlert = ref(false);

onMounted(() => {
  fetch('https://localhost:52167/RecipeCard', {
    method: 'GET'
  })
    .then(response => {
      response.json().then(res => {
        recipes.value = res;
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

  showSuccessAlert.value = true;
}

</script>

<template>
  <div>
    <div class="float-left mr-12">
      <RecipesAddCard @add-recipe="showAddNewModal = true" />
    </div>
    <swiper :slidesPerView="'auto'" :spaceBetween="30" :loop="false" :navigation="true" :mousewheel="true" :keyboard="{
      enabled: true,
    }" :modules="[Navigation, Keyboard, Mousewheel]" class="mySwiper">
      <swiper-slide v-for="recipe in recipes" :key="recipe.id" class="w-52">
        <RecipeCardTemplate :recipe="recipe"></RecipeCardTemplate>
      </swiper-slide>
    </swiper>
  </div>
  <RecipesAddNewModal v-if="showAddNewModal" @cancel-recipe="showAddNewModal = false" @import-recipe="(url) => importRecipe(url)"></RecipesAddNewModal>
  <RecipesAddSuccessAlert v-if="showSuccessAlert" @dismiss-alert="showSuccessAlert = false"></RecipesAddSuccessAlert>
</template>

<style scoped>
.swiper-slide {
  width: 13rem;
}
</style>
