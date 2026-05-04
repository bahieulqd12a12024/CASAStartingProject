using DotNetApi.Data; // NEW: Service uses AppDbContext to access the database.
using DotNetApi.DTOs; // NEW: Service accepts and returns DTOs.
using DotNetApi.Models; // NEW: Service maps DTOs to the Product database entity.

namespace DotNetApi.Services // NEW: Added service layer namespace.
{
    public class ProductService : IProductService // NEW: Added concrete service that implements the product service contract.
    {
        private readonly AppDbContext _context; // NEW: Database context moved from controller into service.

        public ProductService(AppDbContext context) // NEW: ASP.NET injects AppDbContext into the service.
        {
            _context = context; // NEW: Store injected AppDbContext for service methods.
        }

        public List<ProductResponseDto> GetAll() // NEW: Moved "get all products" logic from controller into service.
        {
            return _context.Products
                .Select(product => ToResponseDto(product)) // NEW: Convert each Product entity to a response DTO.
                .ToList(); // NEW: Execute query and return list.
        }

        public ProductResponseDto? GetById(int id) // NEW: Moved "get product by id" logic from controller into service.
        {
            var product = _context.Products.Find(id); // NEW: Find product entity by primary key.
            return product == null ? null : ToResponseDto(product); // NEW: Return null when missing, otherwise return response DTO.
        }

        public ProductResponseDto Create(CreateProductDto dto) // NEW: Moved "create product" logic from controller into service.
        {
            var product = new Product // NEW: Create database entity manually from allowed DTO fields.
            {
                Name = dto.Name, // NEW: Allow only Name from request DTO.
                Description = dto.Description, // NEW: Allow only Description from request DTO.
                Price = dto.Price // NEW: Allow only Price from request DTO.
            };

            _context.Products.Add(product); // NEW: Add entity to EF Core tracking.
            _context.SaveChanges(); // NEW: Save new product to database.

            return ToResponseDto(product); // NEW: Return response DTO instead of raw entity.
        }

        public ProductResponseDto? Update(int id, UpdateProductDto dto) // NEW: Moved "update product" logic from controller into service.
        {
            var product = _context.Products.Find(id); // NEW: Find existing product entity.
            if (product == null)
                return null; // NEW: Tell controller product was not found.

            product.Name = dto.Name; // NEW: Update only allowed Name field.
            product.Description = dto.Description; // NEW: Update only allowed Description field.
            product.Price = dto.Price; // NEW: Update only allowed Price field.

            _context.SaveChanges(); // NEW: Save updated product to database.

            return ToResponseDto(product); // NEW: Return response DTO instead of raw entity.
        }

        public bool Delete(int id) // NEW: Moved "delete product" logic from controller into service.
        {
            var product = _context.Products.Find(id); // NEW: Find existing product entity.
            if (product == null)
                return false; // NEW: Tell controller product was not found.

            _context.Products.Remove(product); // NEW: Mark entity for deletion.
            _context.SaveChanges(); // NEW: Save deletion to database.

            return true; // NEW: Tell controller deletion succeeded.
        }

        private static ProductResponseDto ToResponseDto(Product product) // NEW: Private helper controls which fields leave the API.
        {
            return new ProductResponseDto // NEW: Map Product entity to safe response shape.
            {
                Id = product.Id, // NEW: Include Id in response.
                Name = product.Name, // NEW: Include Name in response.
                Description = product.Description, // NEW: Include Description in response.
                Price = product.Price // NEW: Include Price in response.
            };
        }
    }
}
