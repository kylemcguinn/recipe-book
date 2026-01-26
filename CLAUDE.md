# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Recipe-Book is a full-stack recipe management application with:
- **Backend**: ASP.NET Core 10 Web API (C#)
- **Frontend**: Vue 3 + TypeScript + Vite + Tailwind CSS
- **Database**: MongoDB (NoSQL)
- **Architecture**: Layered architecture with repository pattern

## Project Structure

```
recipe-book/
├── RecipeBook.Api/           # ASP.NET Core 10 REST API
├── RecipeBook.Data/          # Data access layer with MongoDB persistence
├── recipebook.web/           # Vue 3 frontend
├── docker-compose.yml        # Docker orchestration
└── docker-compose.override.yml  # Development overrides
```

## Common Commands

### Frontend (recipebook.web/)

```bash
cd recipebook.web

# Install dependencies
npm install

# Development server (http://localhost:5173)
npm run dev

# Production build with type checking
npm run build

# Linting and auto-fix
npm run lint

# Type checking only
npm run type-check
```

### Backend (.NET)

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run API (from RecipeBook.Api/)
cd RecipeBook.Api
dotnet run
```

### Docker

```bash
# Start all services (API on :50673, MongoDB on :27017, Frontend on :8080)
docker-compose up

# Rebuild and start
docker-compose up --build

# Stop all services
docker-compose down
```

## Architecture Overview

### Data Flow

1. **Recipe Import**: Frontend sends URL → `RecipeImportController` scrapes HTML using HtmlAgilityPack → Extracts JSON-LD structured data → Persists raw JSON as `ExpandoObject` in MongoDB → Returns `RecipeCard` model
2. **Recipe List**: Frontend requests `/RecipeCard` → Controller queries MongoDB via `IPersistence` → Maps `RecipeEntity` to `RecipeCard` → Returns list
3. **Recipe Details**: Frontend displays selected recipe from carousel → Shows ingredients, instructions, and nutrition info from `RecipeCard` model

### Backend Architecture

**Controllers** (all inherit from `BaseController`):
- `RecipeCardController` - GET /RecipeCard - Returns recipe cards for user
- `RecipeImportController` - GET /RecipeImport?url={url} - Scrapes and imports recipe from URL
- `RecipeController` - GET /Recipe?id={id} - Returns raw recipe entities

**Data Layer**:
- `IPersistence` - Generic repository interface with CRUD operations
- `MongoDbPersistence` - MongoDB implementation with userId filtering for multi-tenancy
- All queries automatically filtered by `userId` from `HttpContext`

**Models**:
- `RecipeEntity` - MongoDB document with `recipeJson` (ExpandoObject), `userId`, timestamps
- `RecipeCard` - API response model with mapped fields (name, description, image, ingredients, instructions, nutrition)

### Frontend Architecture

**Main Pages**:
- `HomePage.vue` - Landing page (/)
- `RecipesPage.vue` - Recipe management (/recipes)

**Recipe Components**:
- `RecipesCard.vue` - Individual recipe card in Swiper carousel
- `RecipeDetailsView.vue` - Detailed view with ingredients/instructions/nutrition
- `RecipesAddNewModal.vue` - Import modal with URL input
- `RecipesAddSuccessAlert.vue` - Success notification

**Data Model** (src/models/recipe.ts):
- `RecipeContainer` - Wrapper with `recipeCard`, `recipe`, `isSelected`
- `RecipeCard` - Core recipe data
- `RecipeNutrition` - Nutritional information

### Recipe Import Process

The import controller:
1. Fetches HTML from provided URL
2. Extracts `<script type="application/ld+json">` blocks
3. Handles @graph bundled recipes (selects first Recipe type)
4. Deserializes to `ExpandoObject` for flexible schema storage
5. Persists complete JSON to MongoDB
6. Returns mapped `RecipeCard` for immediate display

This allows storing diverse recipe formats from different websites while maintaining a consistent API response.

## Configuration

**appsettings.json**:
```json
{
  "RecipeBookDatabase": {
    "ConnectionString": "mongodb://localhost:27017/",
    "DatabaseName": "RecipeBook",
    "RecipesCollectionName": "recipes"
  }
}
```

**Docker environment** (docker-compose.override.yml):
- `ASPNETCORE_ENVIRONMENT=Development`
- `ASPNETCORE_URLS=http://+:8080` (HTTP only, no HTTPS certificates needed)
- `RecipeBookDatabase__ConnectionString=mongodb://mongodb:27017/` (uses service name for container networking)

## Development Notes

### Authentication
- Current implementation uses `Constants.TestUserId` for simplified user identification
- `GetUserId()` extension method extracts user from `HttpContext`
- All database queries filtered by `userId` (architecture ready for real auth)

### MongoDB Storage
- Recipes stored as flexible `ExpandoObject` to handle varying JSON-LD schemas from different websites
- `RecipeCard` model provides consistent API responses despite varied source formats
- Helper methods in `RecipeCard` safely access nested dynamic properties

### Frontend State
- Component-level reactive state using Vue 3 Composition API
- No global state management (Vuex/Pinia) currently implemented
- Swiper library handles recipe carousel

### CORS
- Enabled for all origins in Development mode
- Configure appropriately for production deployment

### API Documentation
- Swagger UI available at `/swagger` in Development mode
- OpenAPI spec via Swashbuckle.AspNetCore

## Docker Notes

### Mac Compatibility
- `docker-compose.override.yml` configured for cross-platform compatibility
- Uses `${HOME}` instead of Windows `${APPDATA}` for volume mounts
- HTTPS disabled in containers (use HTTP only for local development)

### Services
- `recipebook.api`: API container (port 50673 → 8080)
- `mongodb`: MongoDB with persistent storage
- `app`: Vue frontend (port 8080)

### MongoDB Connection
- From host: `mongodb://localhost:27017/`
- From API container: `mongodb://mongodb:27017/` (uses Docker service name)

### MongoDB Data Persistence

**Default (Named Volume)**:
- Data persists in Docker-managed volume `mongodb_data`
- Survives container restarts and `docker-compose down`
- Deleted only with `docker-compose down -v`
- Data location managed by Docker

**Optional (Host Directory)**:
To persist data to your local filesystem for easier backup/restore:

1. Edit `docker-compose.override.yml` and uncomment the mongodb volumes section:
   ```yaml
   mongodb:
     volumes:
       - ./data/mongodb:/data/db
   ```

2. Comment out the volume in `docker-compose.yml`:
   ```yaml
   # volumes:
   #   - mongodb_data:/data/db
   ```

3. Restart containers:
   ```bash
   docker-compose down
   docker-compose up
   ```

This creates `data/mongodb/` in your project root containing all MongoDB files. The directory is gitignored by default. You can backup by copying this folder or restore by replacing it.
