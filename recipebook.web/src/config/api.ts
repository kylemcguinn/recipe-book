/**
 * API Configuration
 *
 * Base URL is configured via environment variable VITE_API_BASE_URL
 * Default: http://localhost:50673 (for local development)
 *
 * In Docker, this is injected at build time via docker-compose build args
 */

export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:50673';

// Log the configured API URL for debugging
console.log('API_BASE_URL configured as:', API_BASE_URL);

/**
 * API Endpoints
 */
export const API_ENDPOINTS = {
  RECIPE_CARD: `${API_BASE_URL}/RecipeCard`,
  RECIPE_IMPORT: `${API_BASE_URL}/RecipeImport`,
  RECIPE: `${API_BASE_URL}/Recipe`,
  DELETE_RECIPE_CARD: (id: string) => `${API_BASE_URL}/RecipeCard/${id}`
} as const;
