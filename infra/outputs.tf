output "resource_group_name" {
  description = "The name of the resource group"
  value       = azurerm_resource_group.main.name
}

output "acr_login_server" {
  description = "Azure Container Registry login server URL"
  value       = azurerm_container_registry.main.login_server
}

output "container_app_environment_id" {
  description = "Container App Environment resource ID"
  value       = azurerm_container_app_environment.main.id
}

output "api_url" {
  description = "Public URL of the API Container App"
  value       = "https://${azurerm_container_app.api.latest_revision_fqdn}"
}

output "frontend_url" {
  description = "Public URL of the frontend Container App"
  value       = "https://${azurerm_container_app.frontend.latest_revision_fqdn}"
}

output "managed_identity_client_id" {
  description = "Client ID of the user-assigned managed identity"
  value       = azurerm_user_assigned_identity.container_apps.client_id
}
