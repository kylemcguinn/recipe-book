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

                <!-- Additional recipe details -->
                <div class="mt-6 space-y-4">
                    <div v-if="props.recipe.recipeCard.recipeIngredient && props.recipe.recipeCard.recipeIngredient.length > 0">
                        <h3 class="text-lg font-semibold text-gray-700 mb-2">Ingredients</h3>
                        <ul class="list-disc list-inside space-y-1">
                            <li v-for="(ingredient, index) in props.recipe.recipeCard.recipeIngredient"
                                :key="index"
                                class="text-gray-600">
                                {{ ingredient }}
                            </li>
                        </ul>
                    </div>

                    <div v-if="props.recipe.recipeCard.recipeInstructions && props.recipe.recipeCard.recipeInstructions.length > 0">
                        <h3 class="text-lg font-semibold text-gray-700 mb-2">Instructions</h3>
                        <ol class="list-decimal list-inside space-y-2">
                            <li v-for="(instruction, index) in props.recipe.recipeCard.recipeInstructions"
                                :key="index"
                                class="text-gray-600">
                                {{ instruction }}
                            </li>
                        </ol>
                    </div>

                    <div v-if="props.recipe.recipeCard.nutrition" class="mt-6">
                        <h3 class="text-lg font-semibold text-gray-700 mb-2">Nutrition Information</h3>
                        <div class="grid grid-cols-2 gap-2 text-sm text-gray-600">
                            <div v-if="props.recipe.recipeCard.nutrition.calories">
                                <span class="font-semibold">Calories:</span> {{ props.recipe.recipeCard.nutrition.calories }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.proteinContent">
                                <span class="font-semibold">Protein:</span> {{ props.recipe.recipeCard.nutrition.proteinContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.carbohydrateContent">
                                <span class="font-semibold">Carbohydrates:</span> {{ props.recipe.recipeCard.nutrition.carbohydrateContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.fatContent">
                                <span class="font-semibold">Fat:</span> {{ props.recipe.recipeCard.nutrition.fatContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.saturatedFatContent">
                                <span class="font-semibold">Saturated Fat:</span> {{ props.recipe.recipeCard.nutrition.saturatedFatContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.unsaturatedFatContent">
                                <span class="font-semibold">Unsaturated Fat:</span> {{ props.recipe.recipeCard.nutrition.unsaturatedFatContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.transFatContent">
                                <span class="font-semibold">Trans Fat:</span> {{ props.recipe.recipeCard.nutrition.transFatContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.cholesterolContent">
                                <span class="font-semibold">Cholesterol:</span> {{ props.recipe.recipeCard.nutrition.cholesterolContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.sodiumContent">
                                <span class="font-semibold">Sodium:</span> {{ props.recipe.recipeCard.nutrition.sodiumContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.fiberContent">
                                <span class="font-semibold">Fiber:</span> {{ props.recipe.recipeCard.nutrition.fiberContent }}
                            </div>
                            <div v-if="props.recipe.recipeCard.nutrition.sugarContent">
                                <span class="font-semibold">Sugar:</span> {{ props.recipe.recipeCard.nutrition.sugarContent }}
                            </div>
                        </div>
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
