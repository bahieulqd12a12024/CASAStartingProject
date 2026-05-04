using Microsoft.EntityFrameworkCore;
using DotNetApi.Models;

namespace DotNetApi.Data {
    public class AppDbContext : DbContext {

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
       {}

       public DbSet<Product> Products {get; set;}
    }
}