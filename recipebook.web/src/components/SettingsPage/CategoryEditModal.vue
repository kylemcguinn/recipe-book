<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { Category, CategoryUpdateRequest } from '@/models/category';

const props = defineProps<{
  category: Category
}>();

const emit = defineEmits<{
  updateCategory: [request: CategoryUpdateRequest]
  cancel: []
}>();

const categoryName = ref('');
const categoryColor = ref('#3B82F6');
const displayOrder = ref(0);

onMounted(() => {
  categoryName.value = props.category.name;
  categoryColor.value = props.category.color || '#3B82F6';
  displayOrder.value = props.category.displayOrder;
});

function handleUpdate() {
  if (!categoryName.value.trim()) {
    return;
  }

  emit('updateCategory', {
    name: categoryName.value.trim(),
    color: categoryColor.value,
    displayOrder: displayOrder.value
  });
}
</script>

<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg shadow-xl p-6 w-full max-w-md mx-4">
      <h2 class="text-2xl font-bold mb-4">Edit Category</h2>

      <div class="mb-4">
        <label for="edit-category-name" class="block text-sm font-medium text-gray-700 mb-2">
          Category Name
        </label>
        <input
          id="edit-category-name"
          v-model="categoryName"
          type="text"
          placeholder="Enter category name"
          class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          @keyup.enter="handleUpdate" />
      </div>

      <div class="mb-6">
        <label for="edit-category-color" class="block text-sm font-medium text-gray-700 mb-2">
          Color
        </label>
        <div class="flex items-center gap-3">
          <input
            id="edit-category-color"
            v-model="categoryColor"
            type="color"
            class="w-16 h-10 rounded cursor-pointer" />
          <span class="text-sm text-gray-600">{{ categoryColor }}</span>
        </div>
      </div>

      <div class="flex justify-end gap-3">
        <button
          @click="emit('cancel')"
          class="px-4 py-2 text-gray-700 bg-gray-100 hover:bg-gray-200 rounded-md transition-colors">
          Cancel
        </button>
        <button
          @click="handleUpdate"
          :disabled="!categoryName.trim()"
          class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-md transition-colors disabled:bg-gray-300 disabled:cursor-not-allowed">
          Update
        </button>
      </div>
    </div>
  </div>
</template>
