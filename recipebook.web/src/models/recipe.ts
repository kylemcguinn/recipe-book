export interface RecipeContainer {
    recipeCard: RecipeCard,
    // Using any to avoid excessive stack depth errors with the deeply nested Recipe type from schema-dts
    // This can be changed back to Recipe once TypeScript resolves the circular type issue
    recipe?: any,
    isSelected: boolean
}
export interface RecipeCard {
    id: string;
    name: string;
    url: string;
    description: string;
    image: RecipeCardImage[];
    recipeIngredient?: string[];
    recipeInstructions?: string[];
    nutrition?: RecipeNutrition;
}

export interface RecipeCardImage
{
    url: string;
    height: number;
    width: number;
}

export interface RecipeNutrition {
    calories?: string;
    unsaturatedFatContent?: string;
    carbohydrateContent?: string;
    cholesterolContent?: string;
    fatContent?: string;
    fiberContent?: string;
    proteinContent?: string;
    saturatedFatContent?: string;
    sodiumContent?: string;
    sugarContent?: string;
    transFatContent?: string;
}