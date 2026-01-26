<script setup lang="ts">
import type { Category } from '@/models/category';

const props = defineProps<{
  category: Category
}>();

const emit = defineEmits<{
  confirmDelete: []
  cancelDelete: []
}>();
</script>

<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg shadow-xl p-6 w-full max-w-md mx-4">
      <h2 class="text-2xl font-bold mb-4">Delete Category</h2>

      <p class="text-gray-700 mb-2">
        Are you sure you want to delete the category
        <span class="font-semibold">"{{ category.name }}"</span>?
      </p>

      <p class="text-gray-600 text-sm mb-6">
        This will remove the category from all {{ category.recipeCount }} recipe(s).
        The recipes themselves will not be deleted.
      </p>

      <div class="flex justify-end gap-3">
        <button
          @click="emit('cancelDelete')"
          class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-md transition-colors">
          Cancel
        </button>
        <button
          @click="emit('confirmDelete')"
          class="px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded-md transition-colors">
          Delete
        </button>
      </div>
    </div>
  </div>
</template>
