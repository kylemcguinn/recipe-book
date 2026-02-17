# User-assigned managed identity for Container Apps to pull images from ACR
# This avoids storing any registry credentials in plain text
resource "azurerm_user_assigned_identity" "container_apps" {
  name                = "recipe-book-container-apps-identity"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name

  tags = var.tags
}

# Grant the managed identity permission to pull images from ACR
resource "azurerm_role_assignment" "acr_pull" {
  scope                = azurerm_container_registry.main.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.container_apps.principal_id

  depends_on = [
    azurerm_user_assigned_identity.container_apps,
    azurerm_container_registry.main,
  ]
}
