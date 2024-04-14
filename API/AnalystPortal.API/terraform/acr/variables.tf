
variable "resource_group_name" {
  description = "The name of the resource group"
  default     = "Analyst-Portal-RG"
}

variable "location" {
  description = "The Azure region of the ACR"
  default     = "westus2"
}

variable "sku" {
  description = "The SKU name of the ACR"
  default     = "Basic"
}

