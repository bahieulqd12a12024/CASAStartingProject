namespace DotNetApi.Models
{
    public class Product
    {
        public int Id { get; set; }

        // REMOVED: public string Name {get; set; }
        public string Name { get; set; } = string.Empty; // NEW: Added default value to satisfy nullable reference type safety.

        // REMOVED: public string Description {get; set;}
        public string Description { get; set; } = string.Empty; // NEW: Added default value to satisfy nullable reference type safety.

        // REMOVED: public decimal Price {get; set; }
        public decimal Price { get; set; } // NEW: Same property, reformatted for consistent C# style.
    }
}
