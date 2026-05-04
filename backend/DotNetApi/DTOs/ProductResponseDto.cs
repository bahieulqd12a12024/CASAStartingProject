namespace DotNetApi.DTOs // NEW: Added DTO namespace to separate API response models from database entities.
{
    public class ProductResponseDto // NEW: Added DTO for controlling what product data the API returns.
    {
        public int Id { get; set; } // NEW: Return product id to the client.

        public string Name { get; set; } = string.Empty; // NEW: Return product name to the client.

        public string Description { get; set; } = string.Empty; // NEW: Return product description to the client.

        public decimal Price { get; set; } // NEW: Return product price to the client.
    }
}
