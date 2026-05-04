using DotNetApi.DTOs; // NEW: Service contract uses DTOs instead of exposing database entities.

namespace DotNetApi.Services // NEW: Added service layer namespace.
{
    public interface IProductService // NEW: Added interface so controller depends on a contract, not direct database code.
    {
        List<ProductResponseDto> GetAll(); // NEW: Contract for returning all products as response DTOs.

        ProductResponseDto? GetById(int id); // NEW: Contract for returning one product or null if missing.

        ProductResponseDto Create(CreateProductDto dto); // NEW: Contract for creating product from create DTO.

        ProductResponseDto? Update(int id, UpdateProductDto dto); // NEW: Contract for updating product from update DTO.

        bool Delete(int id); // NEW: Contract for deleting product and reporting success/failure.
    }
}
