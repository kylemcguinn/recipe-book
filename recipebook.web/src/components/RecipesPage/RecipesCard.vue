<script setup lang="ts">
import type { RecipeContainer } from '@/models/recipe';

const props = defineProps<{
    recipe: RecipeContainer
}>();

defineEmits(['recipeClicked']);

</script>
<template>
    <a href="#" @click="$emit('recipeClicked')" :class="{'selected': props.recipe.isSelected}" class="group relative block bg-black">
        <img alt=""
            :src="props.recipe?.recipeCard.image[0].url"
            class="absolute inset-0 h-full w-full object-cover opacity-75 transition-opacity group-hover:opacity-50" />

        <div class="relative p-2 sm:p-4 lg:p-6 w-52 h-60">
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