<script setup lang="ts">
import type { RecipeCard } from '@/models/recipe';
import { onMounted, ref } from 'vue'
import RecipeCardTemplate from '../RecipesPage/RecipesCard.vue'
import RecipesAddCard from '../RecipesPage/RecipesAddCard.vue';

const recipes = ref<RecipeCard[]>([]);

onMounted(() => {
  fetch('https://localhost:63983/RecipeCard', {
    method: 'GET'
  })
    .then(response => {
      response.json().then(res => {
        console.log(res);
        recipes.value = res;
      });
    })
    .catch(err => {
      console.error(err);
    });
});

</script>

<template>
  <div class="flex flex-wrap justify-center">
    <div class="ml-2 mb-2"><RecipesAddCard/></div>
    <div v-for="recipe in recipes" :key="recipe.id" class="ml-2 mb-2">
      <RecipeCardTemplate :recipe="recipe"></RecipeCardTemplate>
    </div>
  </div>
</template>

<style scoped></style>
