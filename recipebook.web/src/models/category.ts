export interface Category {
    id: string;
    name: string;
    color?: string;
    displayOrder: number;
    recipeCount: number;
}

export interface CategoryCreateRequest {
    name: string;
    color?: string;
}

export interface CategoryUpdateRequest {
    name: string;
    color?: string;
    displayOrder: number;
}
