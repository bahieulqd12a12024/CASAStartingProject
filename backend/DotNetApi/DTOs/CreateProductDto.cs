namespace DotNetApi.DTOs // NEW: Added DTO namespace to separate API request models from database entities.
{
    public class CreateProductDto // NEW: Added DTO for creating a product.
    {
        public string Name { get; set; } = string.Empty; // NEW: Client can send product name when creating.

        public string Description { get; set; } = string.Empty; // NEW: Client can send product description when creating.

        public decimal Price { get; set; } // NEW: Client can send product price when creating.
    }
}
