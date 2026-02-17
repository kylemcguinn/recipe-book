resource "azurerm_container_app_environment" "main" {
  name                       = var.container_app_environment_name
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  logs_destination           = "log-analytics"
  log_analytics_workspace_id = azurerm_log_analytics_workspace.main.id

  tags = var.tags
}

# -----------------------------------------------------------------------------
# API Container App (ASP.NET Core / .NET 10)
# -----------------------------------------------------------------------------
resource "azurerm_container_app" "api" {
  name                         = var.api_container_app_name
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.container_apps.id]
  }

  registry {
    server   = azurerm_container_registry.main.login_server
    identity = azurerm_user_assigned_identity.container_apps.id
  }

  secret {
    name  = "mongodb-connection-string"
    value = var.mongodb_connection_string
  }

  template {
    min_replicas = 0
    max_replicas = 1

    container {
      name   = "recipebook-api"
      image  = "${azurerm_container_registry.main.login_server}/recipebookapi:latest"
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }

      env {
        name  = "ASPNETCORE_HTTP_PORTS"
        value = "8080"
      }

      env {
        name        = "RecipeBookDatabase__ConnectionString"
        secret_name = "mongodb-connection-string"
      }
    }
  }

  ingress {
    external_enabled = true
    target_port      = 8080

    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  tags = var.tags

  depends_on = [
    azurerm_role_assignment.acr_pull,
  ]
}

# -----------------------------------------------------------------------------
# Frontend Container App (Vue 3 / Vite / http-server)
# The VITE_API_BASE_URL is baked in at build time via Docker build arg,
# so the container app itself only needs to serve the static dist files.
# -----------------------------------------------------------------------------
resource "azurerm_container_app" "frontend" {
  name                         = var.frontend_container_app_name
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.container_apps.id]
  }

  registry {
    server   = azurerm_container_registry.main.login_server
    identity = azurerm_user_assigned_identity.container_apps.id
  }

  template {
    min_replicas = 0
    max_replicas = 1

    container {
      name   = "recipebook-web"
      image  = "${azurerm_container_registry.main.login_server}/recipebookweb:latest"
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }

  ingress {
    external_enabled = true
    target_port      = 8080

    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  tags = var.tags

  depends_on = [
    azurerm_role_assignment.acr_pull,
  ]
}
