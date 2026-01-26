<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { Category } from '@/models/category';
import { API_ENDPOINTS } from '@/config/api';

const props = defineProps<{
  currentCategoryIds: string[]
}>();

const emit = defineEmits<{
  save: [categoryIds: string[]]
  cancel: []
}>();

const categories = ref<Category[]>([]);
const selectedCategoryIds = ref<string[]>([]);

onMounted(async () => {
  selectedCategoryIds.value = [...props.currentCategoryIds];
  await loadCategories();
});

async function loadCategories() {
  try {
    const response = await fetch(API_ENDPOINTS.CATEGORY);
    if (response.ok) {
      categories.value = await response.json();
    }
  } catch (error) {
    console.error('Failed to load categories:', error);
  }
}

function toggleCategory(categoryId: string) {
  const index = selectedCategoryIds.value.indexOf(categoryId);
  if (index > -1) {
    selectedCategoryIds.value.splice(index, 1);
  } else {
    selectedCategoryIds.value.push(categoryId);
  }
}

function handleSave() {
  emit('save', selectedCategoryIds.value);
}
</script>

<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg shadow-xl p-6 w-full max-w-md mx-4 max-h-[80vh] overflow-y-auto">
      <h2 class="text-2xl font-bold mb-4">Select Categories</h2>

      <div v-if="categories.length === 0" class="text-center py-8 text-gray-500">
        <p>No categories available.</p>
        <p class="text-sm mt-2">Create categories in Settings first.</p>
      </div>

      <div v-else class="space-y-2 mb-6">
        <button
          v-for="category in categories"
          :key="category.id"
          @click="toggleCategory(category.id)"
          class="w-full flex items-center gap-3 p-3 border rounded-lg transition-all"
          :class="{
            'bg-blue-50 border-blue-500': selectedCategoryIds.includes(category.id),
            'hover:bg-gray-50': !selectedCategoryIds.includes(category.id)
          }">
          <div
            class="w-6 h-6 rounded-full flex-shrink-0"
            :style="{ backgroundColor: category.color || '#6B7280' }"></div>
          <span class="font-medium">{{ category.name }}</span>
          <div class="ml-auto">
            <svg
              v-if="selectedCategoryIds.includes(category.id)"
              class="w-5 h-5 text-blue-600"
              fill="currentColor"
              viewBox="0 0 20 20">
              <path
                fill-rule="evenodd"
                d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                clip-rule="evenodd" />
            </svg>
          </div>
        </button>
      </div>

      <div class="flex justify-end gap-3">
        <button
          @click="emit('cancel')"
          class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-md transition-colors">
          Cancel
        </button>
        <button
          @click="handleSave"
          class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md transition-colors">
          Save
        </button>
      </div>
    </div>
  </div>
</template>
