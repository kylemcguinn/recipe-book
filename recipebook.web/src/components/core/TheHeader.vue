<script setup lang="ts">
import TheMenu from './TheMenu.vue'
import TheSearch from './TheSearch.vue'
</script>

<template>
    <header>
      <div class="mx-auto max-w-screen-xl px-4 py-8 sm:px-6 sm:py-12 lg:px-8">
        <div class="sm:flex sm:items-center sm:justify-between">
          <div class="text-center sm:text-left">
            <h1 class="text-2xl font-bold text-gray-900 sm:text-3xl">Recipe Book</h1>
    
            <p class="mt-1.5 text-sm text-gray-500">Let's get cooking!</p>
          </div>
    
          <div>
            <TheMenu />
          </div>
          <div>
            <TheSearch />
          </div>
        </div>
      </div>
    </header>
  </template>