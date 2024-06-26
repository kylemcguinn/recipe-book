<script setup lang="ts">
import type { RecipeCard } from '@/models/recipe';
import { onMounted, ref } from 'vue'

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
  <h1>Your Recipes</h1>
  <p>
    <RouterLink to="/">Go to Home</RouterLink>
  </p>

  <li v-for="recipe in recipes" :key="recipe.id">
      {{ recipe.name }}
    </li>
</template>

<style scoped></style>
