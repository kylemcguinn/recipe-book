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