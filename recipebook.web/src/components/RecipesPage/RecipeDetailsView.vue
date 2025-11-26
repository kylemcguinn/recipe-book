<script setup lang="ts">
import type { RecipeContainer } from '@/models/recipe';

const props = defineProps<{
    recipe: RecipeContainer | null
}>();
</script>

<template>
    <div v-if="props.recipe" class="mt-8 bg-white rounded-lg shadow-lg p-6">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <!-- Recipe Image -->
            <div class="flex justify-center items-start">
                <img
                    v-if="props.recipe.recipeCard.image && props.recipe.recipeCard.image.length > 0"
                    :src="props.recipe.recipeCard.image[0].url"
                    :alt="props.recipe.recipeCard.name"
                    class="rounded-lg shadow-md max-w-full h-auto"
                />
            </div>

            <!-- Recipe Details -->
            <div>
                <h2 class="text-3xl font-bold text-gray-900 mb-4">
                    {{ props.recipe.recipeCard.name }}
                </h2>

                <div class="mb-6">
                    <h3 class="text-lg font-semibold text-gray-700 mb-2">Description</h3>
                    <p class="text-gray-600">
                        {{ props.recipe.recipeCard.description }}
                    </p>
                </div>

                <div v-if="props.recipe.recipeCard.url" class="mb-4">
                    <a
                        :href="props.recipe.recipeCard.url"
                        target="_blank"
                        rel="noopener noreferrer"
                        class="text-blue-600 hover:text-blue-800 underline"
                    >
                        View Original Recipe →
                    </a>
                </div>

                <!-- Additional recipe details if available -->
                <div v-if="props.recipe.recipe" class="mt-6 space-y-4">
                    <div v-if="props.recipe.recipe.recipeIngredient && props.recipe.recipe.recipeIngredient.length > 0">
                        <h3 class="text-lg font-semibold text-gray-700 mb-2">Ingredients</h3>
                        <ul class="list-disc list-inside space-y-1">
                            <li v-for="(ingredient, index) in props.recipe.recipe.recipeIngredient"
                                :key="index"
                                class="text-gray-600">
                                {{ ingredient }}
                            </li>
                        </ul>
                    </div>

                    <div v-if="props.recipe.recipe.recipeInstructions && props.recipe.recipe.recipeInstructions.length > 0">
                        <h3 class="text-lg font-semibold text-gray-700 mb-2">Instructions</h3>
                        <ol class="list-decimal list-inside space-y-2">
                            <li v-for="(instruction, index) in props.recipe.recipe.recipeInstructions"
                                :key="index"
                                class="text-gray-600">
                                {{ typeof instruction === 'string' ? instruction : instruction.text }}
                            </li>
                        </ol>
                    </div>

                    <div v-if="props.recipe.recipe.totalTime || props.recipe.recipe.prepTime || props.recipe.recipe.cookTime"
                         class="flex flex-wrap gap-4 text-sm text-gray-600">
                        <div v-if="props.recipe.recipe.prepTime" class="flex items-center">
                            <span class="font-semibold mr-2">Prep Time:</span>
                            <span>{{ props.recipe.recipe.prepTime }}</span>
                        </div>
                        <div v-if="props.recipe.recipe.cookTime" class="flex items-center">
                            <span class="font-semibold mr-2">Cook Time:</span>
                            <span>{{ props.recipe.recipe.cookTime }}</span>
                        </div>
                        <div v-if="props.recipe.recipe.totalTime" class="flex items-center">
                            <span class="font-semibold mr-2">Total Time:</span>
                            <span>{{ props.recipe.recipe.totalTime }}</span>
                        </div>
                    </div>

                    <div v-if="props.recipe.recipe.recipeYield" class="text-sm text-gray-600">
                        <span class="font-semibold mr-2">Servings:</span>
                        <span>{{ props.recipe.recipe.recipeYield }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div v-else class="mt-8 bg-gray-100 rounded-lg p-8 text-center">
        <p class="text-gray-500 text-lg">Select a recipe to view details</p>
    </div>
</template>

<style scoped>
</style>
