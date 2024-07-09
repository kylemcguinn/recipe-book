export interface RecipeContainer {
    recipeCard: RecipeCard,
    recipeJson: any,
    isSelected: boolean
}
export interface RecipeCard {
    id: string;
    name: string;
    url: string;
    description: string;
    image: RecipeCardImage[];
}

export interface RecipeCardImage
{
    url: string;
    height: number;
    width: number;
}