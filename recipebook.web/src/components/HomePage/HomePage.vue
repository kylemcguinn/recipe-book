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
  <div class="flex">
    <div v-for="recipe in recipes" :key="recipe.id"
      class="p-6 max-w-sm mx-auto bg-white rounded-xl shadow-md flex items-center space-x-4 max-w-200">
      <div>
        <div class="text-xl font-medium text-black">{{ recipe.name }}</div>
        <p class="text-gray-500">{{ recipe.description }}</p>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
