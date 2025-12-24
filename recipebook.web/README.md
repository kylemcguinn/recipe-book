# recipebook.web

Vue 3 + TypeScript + Vite frontend for the Recipe Book application.

## Recommended IDE Setup

[VSCode](https://code.visualstudio.com/) + [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Configuration

### Environment Variables

The application uses environment variables for configuration. API base URL is configured via `.env` files:

- **`.env`** - Default configuration for local development
- **`.env.local`** - Local overrides (gitignored, create if needed)
- **`.env.production`** - Production build defaults

#### Available Variables

- `VITE_API_BASE_URL` - Base URL for the Recipe Book API (default: `http://localhost:50673`)

#### Example `.env.local`

```sh
# Override API URL for local development
VITE_API_BASE_URL=http://localhost:5281
```

### API Configuration

All API endpoints are centralized in `src/config/api.ts`:

```typescript
import { API_ENDPOINTS } from '@/config/api'

// Available endpoints:
// - API_ENDPOINTS.RECIPE_CARD
// - API_ENDPOINTS.RECIPE_IMPORT
// - API_ENDPOINTS.RECIPE
```

## Project Setup

```sh
npm install
```

### Compile and Hot-Reload for Development

```sh
npm run dev
```

The dev server will start on `http://localhost:5173` and use the API URL from `.env` or `.env.local`.

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Lint with [ESLint](https://eslint.org/)

```sh
npm run lint
```

## Docker

The application can be built and run using Docker. The API base URL is injected at build time via Docker Compose build arguments.

See `docker-compose.override.yml` for configuration:

```yaml
app:
  build:
    args:
      - VITE_API_BASE_URL=http://localhost:50673
```

To rebuild with a different API URL, modify the build arg in `docker-compose.override.yml` and run:

```sh
docker-compose up --build
```

## Customize configuration

See [Vite Configuration Reference](https://vitejs.dev/config/).
