<script setup lang="ts">
import { onMounted, ref } from 'vue';
import type { Category, CategoryCreateRequest, CategoryUpdateRequest } from '@/models/category';
import { API_ENDPOINTS } from '@/config/api';
import CategoryCreateModal from './CategoryCreateModal.vue';
import CategoryEditModal from './CategoryEditModal.vue';
import CategoryDeleteConfirmModal from './CategoryDeleteConfirmModal.vue';

const categories = ref<Category[]>([]);
const showAddCategoryModal = ref(false);
const showEditCategoryModal = ref(false);
const showDeleteConfirmModal = ref(false);
const categoryToEdit = ref<Category | null>(null);
const categoryToDelete = ref<Category | null>(null);

onMounted(async () => {
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

async function createCategory(request: CategoryCreateRequest) {
  try {
    const response = await fetch(API_ENDPOINTS.CREATE_CATEGORY, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request)
    });

    if (response.ok) {
      await loadCategories();
      showAddCategoryModal.value = false;
    }
  } catch (error) {
    console.error('Failed to create category:', error);
  }
}

function initiateEdit(category: Category) {
  categoryToEdit.value = category;
  showEditCategoryModal.value = true;
}

async function updateCategory(request: CategoryUpdateRequest) {
  if (!categoryToEdit.value) return;

  try {
    const response = await fetch(API_ENDPOINTS.UPDATE_CATEGORY(categoryToEdit.value.id), {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request)
    });

    if (response.ok) {
      await loadCategories();
      showEditCategoryModal.value = false;
      categoryToEdit.value = null;
    }
  } catch (error) {
    console.error('Failed to update category:', error);
  }
}

function initiateDelete(category: Category) {
  categoryToDelete.value = category;
  showDeleteConfirmModal.value = true;
}

async function confirmDelete() {
  if (!categoryToDelete.value) return;

  try {
    const response = await fetch(API_ENDPOINTS.DELETE_CATEGORY(categoryToDelete.value.id), {
      method: 'DELETE'
    });

    if (response.ok) {
      await loadCategories();
      showDeleteConfirmModal.value = false;
      categoryToDelete.value = null;
    }
  } catch (error) {
    console.error('Failed to delete category:', error);
  }
}

function cancelDelete() {
  showDeleteConfirmModal.value = false;
  categoryToDelete.value = null;
}

function cancelEdit() {
  showEditCategoryModal.value = false;
  categoryToEdit.value = null;
}
</script>

<template>
  <div class="container mx-auto p-6 max-w-4xl">
    <h1 class="text-3xl font-bold mb-8">Settings</h1>

    <section class="bg-white rounded-lg shadow-lg p-6">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-2xl font-semibold">Recipe Categories</h2>
        <button
          @click="showAddCategoryModal = true"
          class="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded transition-colors">
          Add Category
        </button>
      </div>

      <div v-if="categories.length === 0" class="text-center py-8 text-gray-500">
        <p>No categories yet. Create your first category to organize your recipes!</p>
      </div>

      <div v-else class="space-y-2">
        <div
          v-for="category in categories"
          :key="category.id"
          class="flex items-center justify-between p-4 border rounded-lg hover:bg-gray-50 transition-colors">
          <div class="flex items-center gap-4">
            <div
              class="w-8 h-8 rounded-full flex-shrink-0"
              :style="{ backgroundColor: category.color || '#6B7280' }"></div>
            <div>
              <span class="font-medium text-lg">{{ category.name }}</span>
              <span class="text-gray-500 text-sm ml-2">({{ category.recipeCount }} recipes)</span>
            </div>
          </div>
          <div class="flex gap-2">
            <button
              @click="initiateEdit(category)"
              class="text-blue-600 hover:text-blue-800 font-medium px-3 py-1 rounded hover:bg-blue-50 transition-colors">
              Edit
            </button>
            <button
              @click="initiateDelete(category)"
              class="text-red-600 hover:text-red-800 font-medium px-3 py-1 rounded hover:bg-red-50 transition-colors">
              Delete
            </button>
          </div>
        </div>
      </div>
    </section>
  </div>

  <CategoryCreateModal
    v-if="showAddCategoryModal"
    @create-category="createCategory"
    @cancel="showAddCategoryModal = false" />

  <CategoryEditModal
    v-if="showEditCategoryModal && categoryToEdit"
    :category="categoryToEdit"
    @update-category="updateCategory"
    @cancel="cancelEdit" />

  <CategoryDeleteConfirmModal
    v-if="showDeleteConfirmModal && categoryToDelete"
    :category="categoryToDelete"
    @confirm-delete="confirmDelete"
    @cancel-delete="cancelDelete" />
</template>

<style scoped>
/* Additional component-specific styles if needed */
</style>
