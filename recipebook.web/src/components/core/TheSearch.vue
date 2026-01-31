
<script setup lang="ts">
import { ref, watch, onMounted } from 'vue';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();
const route = useRoute();

// Local state for input binding
const searchTerm = ref('');

// Initialize from URL on mount
onMounted(() => {
  const urlSearch = route.query.search;
  if (typeof urlSearch === 'string') {
    searchTerm.value = urlSearch;
  }
});

// Watch for external URL changes (browser back/forward)
watch(() => route.query.search, (newSearch) => {
  if (typeof newSearch === 'string') {
    searchTerm.value = newSearch;
  } else if (newSearch === undefined) {
    searchTerm.value = '';
  }
});

// Debounced search update function
let debounceTimer: ReturnType<typeof setTimeout> | null = null;

function updateSearch(value: string) {
  // Clear existing timer
  if (debounceTimer) {
    clearTimeout(debounceTimer);
  }

  // Debounce for 300ms to avoid excessive URL updates
  debounceTimer = setTimeout(() => {
    const trimmedValue = value.trim();

    if (trimmedValue === '') {
      // Remove search param if empty
      const { search, ...restQuery } = route.query;
      router.push({ query: restQuery });
    } else {
      // Update search param
      router.push({
        query: { ...route.query, search: trimmedValue }
      });
    }
  }, 300);
}

// Handle clear button
function clearSearch() {
  searchTerm.value = '';
  updateSearch('');
}

// Watch local input for changes
watch(searchTerm, (newValue) => {
  updateSearch(newValue);
});
</script>

<template>
    <div class="relative">
      <label for="Search" class="sr-only"> Search </label>

      <input
        v-model="searchTerm"
        type="text"
        id="Search"
        placeholder="Search recipes..."
        class="w-full rounded-md border-gray-200 py-2.5 pe-10 shadow-sm sm:text-sm"
      />

      <span class="absolute inset-y-0 end-0 grid w-10 place-content-center">
        <!-- Show clear button when there's text, otherwise show search icon -->
        <button
          v-if="searchTerm"
          type="button"
          @click="clearSearch"
          class="text-gray-600 hover:text-gray-700">
          <span class="sr-only">Clear search</span>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
            class="h-4 w-4">
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>

        <div v-else class="text-gray-400 pointer-events-none">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
            class="h-4 w-4">
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M21 21l-5.197-5.197m0 0A7.5 7.5 0 105.196 5.196a7.5 7.5 0 0010.607 10.607z" />
          </svg>
        </div>
      </span>
    </div>
  </template>