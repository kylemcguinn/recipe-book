variable "subscription_id" {
  description = "Azure subscription ID"
  type        = string
}

variable "tenant_id" {
  description = "Azure tenant ID"
  type        = string
  default     = "d9963c60-174c-4e11-8a50-6417ef7951cf"
}

variable "resource_group_name" {
  description = "Name of the resource group"
  type        = string
  default     = "recipe-book-rg"
}

variable "location" {
  description = "Azure region for all resources"
  type        = string
  default     = "eastus"
}

variable "acr_name" {
  description = "Azure Container Registry name (must be globally unique, alphanumeric only)"
  type        = string
  default     = "recipebooknexusacr"
}

variable "container_app_environment_name" {
  description = "Name of the Container App Environment"
  type        = string
  default     = "recipe-book-env"
}

variable "api_container_app_name" {
  description = "Name of the API Container App"
  type        = string
  default     = "recipe-book-api"
}

variable "frontend_container_app_name" {
  description = "Name of the frontend Container App"
  type        = string
  default     = "recipe-book-web"
}

variable "mongodb_connection_string" {
  description = "MongoDB Atlas connection string"
  type        = string
  sensitive   = true
}

variable "log_analytics_workspace_name" {
  description = "Name of the Log Analytics workspace"
  type        = string
  default     = "recipe-book-logs"
}

variable "tags" {
  description = "Tags to apply to all resources"
  type        = map(string)
  default = {
    environment = "production"
    project     = "recipe-book"
    managed_by  = "terraform"
  }
}
