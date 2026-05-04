using Microsoft.AspNetCore.Mvc;
// REMOVED: using DotNetApi.Data;
// REMOVED: using DotNetApi.Models;
using DotNetApi.DTOs; // NEW: Controller now receives create/update DTOs from the request body.
using DotNetApi.Services; // NEW: Controller now calls the product service instead of AppDbContext directly.

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // REMOVED: private readonly AppDbContext _context;
        private readonly IProductService _productService; // NEW: Controller depends on the service contract.

        // REMOVED: public ProductsController(AppDbContext context)
        public ProductsController(IProductService productService) // NEW: ASP.NET injects ProductService through IProductService.
        {
            // REMOVED: _context = context;
            _productService = productService; // NEW: Store injected service for controller actions.
        }

        // GET: api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            // REMOVED: var products = _context.Products.ToList();
            var products = _productService.GetAll(); // NEW: Ask service for products instead of querying database here.
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // REMOVED: var product = _context.Products.Find(id);
            var product = _productService.GetById(id); // NEW: Ask service to find product by id.
            if (product == null)
                return NotFound();
            
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        // REMOVED: public IActionResult Create([FromBody] Product product)
        public IActionResult Create([FromBody] CreateProductDto dto) // NEW: Accept create DTO instead of database entity.
        {
            // REMOVED: if (product == null)
            if (dto == null) // NEW: Validate DTO request body exists.
                return BadRequest();

            // REMOVED: _context.Products.Add(product);
            // REMOVED: _context.SaveChanges();
            var product = _productService.Create(dto); // NEW: Service maps DTO to Product and saves it.

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        // REMOVED: public IActionResult Update(int id, [FromBody] Product product)
        public IActionResult Update(int id, [FromBody] UpdateProductDto dto) // NEW: Accept update DTO instead of database entity.
        {
            if (dto == null) // NEW: Validate DTO request body exists.
                return BadRequest();

            // REMOVED: var existingProduct = _context.Products.Find(id);
            // REMOVED: if (existingProduct == null)
            // REMOVED:     return NotFound();
            // REMOVED: existingProduct.Name = product.Name;
            // REMOVED: existingProduct.Description = product.Description;
            // REMOVED: existingProduct.Price = product.Price;
            // REMOVED: _context.SaveChanges();
            var product = _productService.Update(id, dto); // NEW: Service finds, updates, and saves the product.
            if (product == null)
                return NotFound();

            // REMOVED: return Ok(existingProduct);
            return Ok(product); // NEW: Return response DTO from service.
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // REMOVED: var product = _context.Products.Find(id);
            // REMOVED: if (product == null)
            // REMOVED:     return NotFound();
            // REMOVED: _context.Products.Remove(product);
            // REMOVED: _context.SaveChanges();
            var deleted = _productService.Delete(id); // NEW: Service finds and deletes the product.
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
