<script setup lang="ts">
import { ref } from 'vue';
import RecipesAddImportInput from './RecipesAddImportInput.vue';

const urlText = ref("");
</script>
<template>
    <div class="rounded-2xl border border-blue-100 bg-white p-4 shadow-lg sm:p-6 lg:p-8 absolute z-50 top-32 -translate-x-1/2 left-1/2"
        role="alert">
        <div class="flex items-center gap-4">
            <span class="shrink-0 rounded-full bg-blue-400 p-2 text-white">
                <svg fill="#ffffff" class="h-8 w-8" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg"
                    xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 512 512" xml:space="preserve">
                    <g>
                        <g>
                            <path d="M148.489,171.778l-0.001,44.629c0,8.027-2.286,14.557-6.613,18.883c-1.089,1.089-2.319,2.047-3.676,2.873v-66.384h-30.417
			v66.382c-1.356-0.826-2.584-1.784-3.673-2.872c-4.326-4.326-6.613-10.855-6.612-18.883v-44.625H67.079v44.624
			c-0.001,16.271,5.367,30.239,15.522,40.393c6.824,6.823,15.374,11.472,25.18,13.797v133.594h30.417V270.595
			c9.809-2.324,18.361-6.975,25.184-13.796c10.155-10.154,15.523-24.121,15.523-40.392l0.001-44.627L148.489,171.778z" />
                        </g>
                    </g>
                    <g>
                        <g>
                            <path d="M389.007,166.709c-26.393,0.001-47.865,21.474-47.865,47.864v35.684c0.001,21.076,13.7,39.001,32.658,45.372v108.558
			h30.417V295.631c18.957-6.372,32.656-24.297,32.655-45.372l0.001-35.69C436.869,188.181,415.397,166.71,389.007,166.709z
			M406.456,250.259c0,9.62-7.826,17.446-17.446,17.447c-9.622-0.001-17.449-7.827-17.449-17.447v-35.684
			c0-9.62,7.827-17.446,17.447-17.447c9.62,0.001,17.447,7.828,17.449,17.445L406.456,250.259z" />
                        </g>
                    </g>
                    <g>
                        <g>
                            <path d="M473.751,31.888L297.38,74.602C278.98,78.816,265.422,85.969,256,96.444c-9.423-10.475-22.979-17.627-41.38-21.841
			L38.249,31.888L0,96.779v340.454l227.979,42.619l1.385,0.26h53.272L512,437.234V96.779L473.751,31.888z M215.564,446.588
			L30.417,411.975V119.242l185.147,34.613V446.588z M232.182,126.017L39.324,89.964l13.679-23.206l154.555,37.43l0.196,0.047
			c19.015,4.341,27.524,11.668,30.969,21.783H232.182z M266.016,449.694h-20.035V156.435h20.035V449.694z M279.817,126.018h-6.54
			c3.444-10.116,11.954-17.443,30.969-21.784l154.75-37.477l13.679,23.206L279.817,126.018z M481.583,411.975l-185.148,34.613
			V153.855l185.148-34.613V411.975z" />
                        </g>
                    </g>
                </svg>
            </span>

            <p class="font-medium sm:text-lg">Add a recipe!</p>
        </div>

        <p class="mt-4 text-gray-500">
            <RecipesAddImportInput v-model="urlText"></RecipesAddImportInput>
        </p>

        <div class="mt-6 sm:flex sm:gap-4">
            <a class="inline-block w-full rounded-lg bg-blue-500 px-5 py-3 text-center text-sm font-semibold text-white sm:w-auto"
                href="#" @click="$emit('importRecipe', urlText)">
                Import Recipe
            </a>

            <a class="mt-2 inline-block w-full rounded-lg bg-gray-50 px-5 py-3 text-center text-sm font-semibold text-gray-500 sm:mt-0 sm:w-auto"
                href="#" @click="$emit('cancelRecipe')">
                Cancel
            </a>
        </div>
    </div>
</template>