namespace DotNetApi.DTOs // NEW: Added DTO namespace to separate API request models from database entities.
{
    public class UpdateProductDto // NEW: Added DTO for updating an existing product.
    {
        public string Name { get; set; } = string.Empty; // NEW: Client can update product name.

        public string Description { get; set; } = string.Empty; // NEW: Client can update product description.

        public decimal Price { get; set; } // NEW: Client can update product price.
    }
}
