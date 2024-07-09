<script setup lang="ts">
  import { defineEmits, ref } from 'vue';

  const props = defineProps<{
    title: string,
    body: String
  }>();

  const emit = defineEmits(['dismissAlert']);
  const visible = ref(false);
  setTimeout(() => dismiss(), 5000);
  setTimeout(() => visible.value = true, 100);


  function dismiss() {
    visible.value = false;
    setTimeout(() => emit('dismissAlert'), 1000);
  }

</script>
<template>
  <div
    class="bg-green-100 border-t-4 border-green-500 rounded-b text-green-900 px-4 py-3 shadow-md absolute z-50 top-12 -translate-x-1/2 left-1/2 w-96 transition-opacity ease-in duration-700"
    :class="{ 'opacity-100': visible, 'opacity-0': !visible }"
    role="alert">
    <div class="flex relative">
      <div class="py-1"><svg class="fill-current h-6 w-6 text-green-500 mr-4" xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 20 20">
          <path
            d="M2.93 17.07A10 10 0 1 1 17.07 2.93 10 10 0 0 1 2.93 17.07zm12.73-1.41A8 8 0 1 0 4.34 4.34a8 8 0 0 0 11.32 11.32zM9 11V9h2v6H9v-4zm0-6h2v2H9V5z" />
        </svg></div>
      <div>
        <p class="font-bold">{{ props.title }}</p>
        <p class="text-sm">{{ props.body }}</p>
      </div>

      <button class="text-gray-500 transition hover:text-gray-600 absolute right-0"  @click="dismiss">
        <span class="sr-only">Dismiss popup</span>

        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor"
          class="h-6 w-6">
          <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </div>
  </div>
</template>

<style scoped>
.fade-in-out {
  visibility: visible;
  opacity: 1;
  transition: opacity 2s linear;
}
</style>