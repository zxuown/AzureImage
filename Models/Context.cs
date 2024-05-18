using Microsoft.EntityFrameworkCore;

namespace AzureImage.Models;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options) { }

    public virtual DbSet<Image> Images { get; set; } = null!;
}
