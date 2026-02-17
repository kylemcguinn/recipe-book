resource "azurerm_container_registry" "main" {
  name                = var.acr_name
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location

  # Basic SKU is sufficient for personal use (~$5/month)
  # Upgrade to Standard if you need geo-replication or anonymous pull
  sku           = "Basic"
  admin_enabled = false

  tags = var.tags
}
