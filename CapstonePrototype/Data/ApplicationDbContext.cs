using Microsoft.EntityFrameworkCore;
using CapstonePrototype.Models;

namespace CapstonePrototype.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompCategory> CompCategories { get; set; }
    public DbSet<CompCat> CompCats { get; set; }
    public DbSet<Rfq> Rfqs { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<PurchasedProduct> PurchasedProducts { get; set; }
    public DbSet<RfqBid> RfqBids { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(9, 1, 0)));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity=>{
            entity.HasKey(e=>e.Id);
        });
        modelBuilder.Entity<Company>(entity=>{
            entity.HasKey(e=>e.Id);
        });
        modelBuilder.Entity<CompCategory>(entity=>{
            entity.HasKey(e=>e.Id);
        });
        modelBuilder.Entity<CompCat>(entity=>{
            entity.HasKey(e=>e.Id);
            entity.HasOne(e=>e.Company)
            .WithMany()
            .HasForeignKey(e=>e.CompanyId);
            entity.HasOne(e=>e.CompCategory)
            .WithMany()
            .HasForeignKey(e=>e.CompCategoryId);
        });
        modelBuilder.Entity<Rfq>(entity=>{
            entity.HasKey(e=>e.Id);
        });
        modelBuilder.Entity<Product>(entity=>{
            entity.HasKey(e=>e.Id);
            entity.HasOne(e=>e.Company)
            .WithMany()
            .HasForeignKey(e=>e.CompanyId);
        });
        modelBuilder.Entity<PurchasedProduct>(entity=>{
            entity.HasKey(e=>e.Id);
        });
        modelBuilder.Entity<RfqBid>(entity=>{
            entity.HasKey(e=>e.Id);
            entity.HasOne(e=>e.Rfq)
            .WithMany()
            .HasForeignKey(e=>e.RfqId);
            entity.HasOne(e=>e.Company)
            .WithMany()
            .HasForeignKey(e=>e.CompanyId);
        });
    }
}