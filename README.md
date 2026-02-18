# Recipe Book

A full-stack recipe management application that allows you to import, store, and view recipes from across the web. Built with ASP.NET Core 8, Vue 3, and MongoDB.

## Features

- **Recipe Import**: Import recipes from any URL that supports JSON-LD structured data
- **Recipe Storage**: Store recipes in MongoDB with flexible schema support
- **Recipe Browsing**: Browse recipes in an interactive carousel interface
- **Recipe Details**: View ingredients, instructions, and nutritional information
- **Multi-tenancy Ready**: Architecture supports user-based data isolation

## Technology Stack

### Backend
- **ASP.NET Core 8** - Web API framework
- **C# with nullable reference types** - Type-safe language features
- **MongoDB** - NoSQL database for flexible recipe storage
- **HtmlAgilityPack** - HTML parsing for recipe scraping
- **Swashbuckle** - OpenAPI/Swagger documentation

### Frontend
- **Vue 3** - Progressive JavaScript framework
- **TypeScript** - Type-safe JavaScript
- **Vite** - Fast build tool and dev server
- **Tailwind CSS** - Utility-first CSS framework
- **Swiper** - Touch-enabled slider/carousel

### Infrastructure
- **Docker & Docker Compose** - Containerization and local orchestration
- **MongoDB Atlas** - Managed cloud database (free tier)
- **Azure Container Apps** - Serverless container hosting with scale-to-zero
- **Azure Container Registry** - Private Docker image registry
- **Terraform** - Infrastructure as code for Azure resources
- **GitHub Actions** - CI/CD pipeline for automated deployments

## Project Structure

```
recipe-book/
├── RecipeBook.Api/           # ASP.NET Core 8 REST API
│   ├── Controllers/          # API endpoints
│   ├── Models/              # Data transfer objects
│   └── Program.cs           # Application entry point
├── RecipeBook.Data/          # Data access layer
│   ├── Entities/            # Database entities
│   ├── Persistence/         # Repository implementation
│   └── Configuration/       # Database configuration
├── recipebook.web/          # Vue 3 frontend
│   ├── src/
│   │   ├── components/      # Vue components
│   │   ├── models/          # TypeScript models
│   │   ├── router/          # Vue Router configuration
│   │   └── config/          # API configuration
│   └── public/              # Static assets
├── docker-compose.yml       # Docker orchestration
├── docker-compose.override.yml  # Development overrides
├── infra/                   # Terraform infrastructure definitions
│   ├── main.tf              # Provider config and resource group
│   ├── variables.tf         # Input variables
│   ├── outputs.tf           # Output values (URLs, registry server)
│   ├── acr.tf               # Azure Container Registry
│   ├── container_apps.tf    # Container App Environment and apps
│   ├── identity.tf          # Managed identity and role assignments
│   └── monitoring.tf        # Log Analytics workspace
├── .github/
│   └── workflows/
│       └── deploy.yml       # CI/CD pipeline
└── .vscode/                 # VS Code debug configurations
```

## Prerequisites

Choose one of the following setups:

### Option 1: Local Development
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js LTS](https://nodejs.org/) (v20 or later)
- [MongoDB](https://www.mongodb.com/try/download/community) (running on localhost:27017)

### Option 2: Docker Development
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/install/) (included with Docker Desktop)

## Getting Started

### Running with Docker (Recommended)

This is the easiest way to get started. Docker Compose will start all services including MongoDB.

```bash
# Clone the repository
git clone <repository-url>
cd recipe-book

# Start all services
docker-compose up

# Or rebuild and start
docker-compose up --build
```

**Services will be available at:**
- Frontend: http://localhost:8080
- API: http://localhost:50673
- API Swagger: http://localhost:50673/swagger
- MongoDB: localhost:27017

To stop all services:
```bash
docker-compose down
```

### Running Locally

#### 1. Start MongoDB

Make sure MongoDB is running on `localhost:27017`. If you installed MongoDB locally:

```bash
# macOS (using Homebrew)
brew services start mongodb-community

# Windows
# Start MongoDB service from Services panel or:
net start MongoDB

# Linux
sudo systemctl start mongod
```

#### 2. Start the Backend API

```bash
# Navigate to API project
cd RecipeBook.Api

# Restore dependencies
dotnet restore

# Run the API
dotnet run
```

The API will start on http://localhost:5000 (or the port shown in console).
Swagger documentation: http://localhost:5000/swagger

#### 3. Start the Frontend

Open a new terminal:

```bash
# Navigate to frontend project
cd recipebook.web

# Install dependencies (first time only)
npm install

# Start development server
npm run dev
```

The frontend will start on http://localhost:5173

#### 4. Access the Application

Open your browser to http://localhost:5173

## Development

### VS Code Debug Configuration

This repository includes VS Code launch configurations for debugging:

1. **`.NET Core Launch (API)`** - Debug the backend API
2. **`Launch Chrome (Vue.js)`** - Debug the frontend in Chrome
3. **`Launch Edge (Vue.js)`** - Debug the frontend in Edge
4. **`Full Stack (API + Vue.js)`** - Debug both simultaneously (Recommended)

Press `F5` and select your desired configuration from the debug dropdown.

### Backend Development

```bash
cd RecipeBook.Api

# Build the solution
dotnet build

# Run with hot reload
dotnet watch run

# Run tests (if available)
dotnet test
```

**API Endpoints:**
- `GET /RecipeCard` - Get all recipe cards for the user
- `GET /RecipeImport?url={url}` - Import a recipe from a URL
- `GET /Recipe?id={id}` - Get a specific recipe by ID

**Configuration:**

Edit `RecipeBook.Api/appsettings.json` to configure:
- MongoDB connection string
- Database name
- Collection names

### Frontend Development

```bash
cd recipebook.web

# Development server with hot reload
npm run dev

# Type checking
npm run type-check

# Linting and auto-fix
npm run lint

# Production build
npm run build

# Preview production build
npm run preview
```

**Environment Variables:**

Create a `.env.local` file in `recipebook.web/` to override API settings:

```env
VITE_API_BASE_URL=http://localhost:5000
```

See [recipebook.web/README.md](recipebook.web/README.md) for more details.

## Architecture

### Data Flow

1. **Recipe Import**:
   - User provides a recipe URL via the frontend
   - Backend scrapes the URL using HtmlAgilityPack
   - Extracts JSON-LD structured data from `<script>` tags
   - Stores raw JSON as `ExpandoObject` in MongoDB
   - Returns normalized `RecipeCard` model to frontend

2. **Recipe Display**:
   - Frontend fetches recipe cards from `/RecipeCard` endpoint
   - Displays recipes in Swiper carousel
   - Shows detailed view with ingredients, instructions, and nutrition

### Backend Architecture

- **Controllers**: Handle HTTP requests and responses
  - `RecipeCardController` - Recipe card operations
  - `RecipeImportController` - Recipe import from URLs
  - `RecipeController` - Raw recipe data access

- **Data Layer**: Repository pattern with MongoDB
  - `IPersistence` - Generic repository interface
  - `MongoDbPersistence` - MongoDB implementation
  - Automatic user-based data filtering

- **Models**:
  - `RecipeEntity` - MongoDB document (stores raw JSON)
  - `RecipeCard` - API response model (normalized data)

### Frontend Architecture

- **Pages**:
  - `HomePage.vue` - Landing page
  - `RecipesPage.vue` - Main recipe management interface

- **Components**:
  - `RecipesCard.vue` - Individual recipe card in carousel
  - `RecipeDetailsView.vue` - Detailed recipe information
  - `RecipesAddNewModal.vue` - Import recipe modal

- **State Management**: Component-level reactive state (Vue 3 Composition API)

## Database

### MongoDB Schema

The application uses a flexible schema to accommodate various recipe formats:

```javascript
{
  _id: ObjectId,
  recipeJson: {}, // Raw recipe JSON-LD data (flexible structure)
  userId: string,
  createdAtUtc: ISODate,
  updatedAtUtc: ISODate
}
```

### Database Configuration

**Local Development:**
- Connection: `mongodb://localhost:27017/`
- Database: `RecipeBook`
- Collection: `recipes`

**Docker:**
- Connection: `mongodb://mongodb:27017/` (uses Docker service name)
- Data persisted in `mongodb_data` volume

## Docker Details

### Services

The `docker-compose.yml` defines three services:

1. **recipebook.api** - ASP.NET Core API
   - Internal port: 8080
   - External port: 50673
   - Environment: Development
   - HTTP only (no HTTPS certificates required)

2. **mongodb** - MongoDB database
   - Port: 27017
   - Persistent volume: `mongodb_data`

3. **app** - Vue.js frontend
   - Internal port: 8080
   - External port: 8080
   - Built with production optimizations

### Environment Variables

The `docker-compose.override.yml` configures development settings:

```yaml
recipebook.api:
  environment:
    - ASPNETCORE_ENVIRONMENT=Development
    - ASPNETCORE_URLS=http://+:8080
    - RecipeBookDatabase__ConnectionString=mongodb://mongodb:27017/

app:
  build:
    args:
      - VITE_API_BASE_URL=http://localhost:50673
```

### Docker Commands

```bash
# Start services in background
docker-compose up -d

# View logs
docker-compose logs -f

# View logs for specific service
docker-compose logs -f recipebook.api

# Stop services
docker-compose down

# Stop and remove volumes (data loss!)
docker-compose down -v

# Rebuild specific service
docker-compose build recipebook.api

# Restart a service
docker-compose restart recipebook.api
```

## Cloud Deployment

The application is hosted on Azure using Container Apps with MongoDB Atlas as the database. Infrastructure is managed with Terraform and deployments are automated via GitHub Actions.

### Architecture

- **MongoDB Atlas** (free tier) — managed database, no infrastructure to maintain
- **Azure Container Registry** (Basic SKU, ~$5/month) — stores Docker images
- **Azure Container Apps** (Consumption plan) — hosts the API and frontend with scale-to-zero (effectively free for personal use)
- **GitHub Actions** — builds images and deploys on every push to `master`

### Prerequisites

- [Terraform CLI](https://developer.hashicorp.com/terraform/install)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- An Azure subscription
- A [MongoDB Atlas](https://www.mongodb.com/atlas) account with a free cluster

### Provisioning Infrastructure (First Time)

**1. Create your `terraform.tfvars` file:**

```bash
cd infra
cp terraform.tfvars.example terraform.tfvars
```

Edit `terraform.tfvars` and fill in your values:

```hcl
subscription_id           = "00000000-0000-0000-0000-000000000000"
acr_name                  = "yourregistryname"   # must be globally unique
mongodb_connection_string = "mongodb+srv://<user>:<password>@<cluster>.mongodb.net/RecipeBook"
```

**2. Log in to Azure and apply:**

```bash
az login
terraform init
terraform plan
terraform apply
```

Note the `api_url` and `frontend_url` from the output — you'll need these for GitHub secrets.

**3. Create a service principal for GitHub Actions:**

```bash
az ad sp create-for-rbac --name "recipe-book-github-actions" --role Contributor --scopes /subscriptions/<sub-id>/resourceGroups/recipe-book-rg --json-auth
```

Assign the AcrPush role so the pipeline can push images:

```bash
az role assignment create --assignee <appId> --scope $(az acr show --name <acr-name> --query id --output tsv) --role acrpush
```

Configure the service principal for OIDC (passwordless) authentication with GitHub:

```bash
az ad app federated-credential create --id <appId> --parameters '{
  "name": "recipe-book-github-actions",
  "issuer": "https://token.actions.githubusercontent.com",
  "subject": "repo:<your-github-username>/<your-repo-name>:ref:refs/heads/master",
  "audiences": ["api://AzureADTokenExchange"]
}'
```

**4. Set GitHub repository secrets:**

| Secret | Value |
|---|---|
| `AZURE_CLIENT_ID` | Service principal `clientId` |
| `AZURE_TENANT_ID` | Your Azure tenant ID |
| `AZURE_SUBSCRIPTION_ID` | Your Azure subscription ID |
| `ACR_NAME` | ACR name (e.g. `yourregistryname`) |
| `ACR_LOGIN_SERVER` | ACR login server (e.g. `yourregistryname.azurecr.io`) |
| `API_BASE_URL` | `api_url` output from Terraform (stable ingress URL) |
| `RESOURCE_GROUP` | `recipe-book-rg` |
| `API_CONTAINER_APP_NAME` | `recipe-book-api` |
| `WEB_CONTAINER_APP_NAME` | `recipe-book-web` |

### CI/CD Pipeline

The `.github/workflows/deploy.yml` workflow runs on every push to `master`:

1. Authenticates to Azure via OIDC (no stored credentials)
2. Builds the API Docker image and pushes it to ACR tagged with the commit SHA
3. Builds the frontend Docker image with `VITE_API_BASE_URL` baked in, pushes to ACR
4. Updates the API Container App to use the new image
5. Updates the frontend Container App to use the new image

You can also trigger a deployment manually via **Actions → Build and Deploy → Run workflow**.

### Scale-to-Zero

Both Container Apps are configured with `min_replicas = 0`. When idle they scale down to zero and incur no compute cost. The first request after a period of inactivity will have a ~10-30 second cold start delay while the container spins up.

To keep an app always warm, change `min_replicas` to `1` in [infra/container_apps.tf](infra/container_apps.tf) (adds ~$3-4/month per app).

### Useful Commands

```bash
# Get deployed URLs
cd infra && terraform output

# Get API URL directly
az containerapp show --name recipe-book-api --resource-group recipe-book-rg --query "properties.configuration.ingress.fqdn" --output tsv

# View API logs
az containerapp logs show --name recipe-book-api --resource-group recipe-book-rg --follow false

# View revision status
az containerapp revision list --name recipe-book-api --resource-group recipe-book-rg --output table

# Tear down all Azure resources
cd infra && terraform destroy
```

## Troubleshooting

### MongoDB Connection Issues

**Local Development:**
- Ensure MongoDB is running: `mongosh` should connect successfully
- Check connection string in `appsettings.json`

**Docker:**
- API must use `mongodb://mongodb:27017/` (service name, not localhost)
- Verify with: `docker-compose logs mongodb`

### Port Conflicts

If ports are already in use:
- API (50673): Change in `docker-compose.override.yml`
- Frontend (8080 or 5173): Change in respective configuration
- MongoDB (27017): Change connection strings accordingly

### HTTPS/Certificate Errors

For local development, HTTPS is disabled in Docker. If you see certificate errors:
- Ensure `ASPNETCORE_URLS=http://+:8080` is set (HTTP only)
- Remove any HTTPS port configurations

### CORS Issues

The API enables CORS for all origins in Development mode. If you encounter CORS errors:
- Verify `ASPNETCORE_ENVIRONMENT=Development`
- Check browser console for specific CORS errors
- Ensure API URL in frontend matches actual API location

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Recipe data sourced from websites implementing [Schema.org Recipe](https://schema.org/Recipe) markup
- Built with modern web technologies and best practices

