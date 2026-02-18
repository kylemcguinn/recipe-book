<script setup lang="ts">
import type { RecipeContainer } from '@/models/recipe';

const props = defineProps<{
    recipe: RecipeContainer
}>();

const emit = defineEmits(['recipeClicked', 'deleteRecipe']);

function handleDeleteClick(event: MouseEvent) {
    event.preventDefault();
    event.stopPropagation();
    emit('deleteRecipe', props.recipe);
}

</script>
<template>
    <a href="#" @click="$emit('recipeClicked')" :class="{'selected': props.recipe.isSelected}" class="group relative block bg-black">
        <img alt=""
            :src="props.recipe?.recipeCard.image[0].url"
            class="absolute inset-0 h-full w-full object-cover opacity-75 transition-opacity group-hover:opacity-50" />

        <!-- Delete Button -->
        <button
            @click="handleDeleteClick"
            class="absolute top-2 right-2 z-10 p-2 bg-red-600 bg-opacity-80 hover:bg-opacity-100 rounded-lg transition-all opacity-0 group-hover:opacity-100"
            title="Delete recipe">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-5 h-5 text-white">
                <path stroke-linecap="round" stroke-linejoin="round" d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" />
            </svg>
        </button>

        <div class="relative p-2 sm:p-4 lg:p-6 w-full md:w-52 h-60">
            <p class="text-xl font-bold text-white line-clamp-2 h-14">{{ props.recipe == null ? null : props.recipe.recipeCard.name }}</p>

            <div class="">
                <div
                    class="translate-y-8 transform opacity-0 transition-all group-hover:translate-y-0 group-hover:opacity-100">
                    <p class="text-sm text-white line-clamp-7">
                        {{ props.recipe?.recipeCard.description }}
                    </p>
                </div>
            </div>
        </div>
    </a>
</template>

<style scoped>
.selected {
    box-shadow: 0px 12px 22px 5px #3b5cb8;
}
</style>