output "acr_login_server" {
  value = azurerm_container_registry.acr.login_server
  description = "The login server URL of the Azure Container Registry."
}

output "acr_admin_username" {
  value = azurerm_container_registry.acr.admin_username
  description = "The admin username for the Azure Container Registry."
}

