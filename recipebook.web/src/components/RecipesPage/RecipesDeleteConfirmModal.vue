<script setup lang="ts">
import type { RecipeContainer } from '@/models/recipe';

const props = defineProps<{
  recipe: RecipeContainer
}>();

const emit = defineEmits(['confirmDelete', 'cancelDelete']);
</script>

<template>
  <div class="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
    <div class="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white">
      <div class="mt-3">
        <h3 class="text-lg leading-6 font-medium text-gray-900 mb-4">
          Delete Recipe?
        </h3>
        <p class="text-sm text-gray-500 mb-2">
          Are you sure you want to delete:
        </p>
        <p class="text-sm font-semibold text-gray-700 mb-4">
          {{ props.recipe.recipeCard.name }}
        </p>
        <p class="text-xs text-gray-500 mb-4">
          This action cannot be undone.
        </p>
        <div class="flex gap-3">
          <button
            @click="emit('cancelDelete')"
            class="flex-1 px-4 py-2 bg-gray-200 text-gray-800 text-base font-medium rounded-md hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-300">
            Cancel
          </button>
          <button
            @click="emit('confirmDelete')"
            class="flex-1 px-4 py-2 bg-red-600 text-white text-base font-medium rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500">
            Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
