using BusinessLogic;
using BusinessLogic.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class DataAccessContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Deposit> Deposits { get; set; }
    
    public DbSet<DateRange> DateRanges { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    
    public DbSet<Review> Reviews { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    
    public DbSet<Log> Actions { get; set; }
    
    
    
    public DataAccessContext (DbContextOptions<DataAccessContext> options) : base(options)
    {
        var relationalOptionsExtension = options.Extensions
            .OfType<Microsoft.EntityFrameworkCore.Infrastructure.RelationalOptionsExtension>()
            .FirstOrDefault();
        
        var databaseType = relationalOptionsExtension?.Connection?.GetType().Name;
        if( databaseType != null && databaseType.Contains("Sqlite"))
            this.Database.EnsureCreated();
        else
            this.Database.Migrate();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deposit>()
            .HasMany(d => d.Promotions)
            .WithMany(p => p.Deposits)
            .UsingEntity<Dictionary<string, object>>(
                "DepositPromotion",
                j => j.HasOne<Promotion>().WithMany().HasForeignKey("PromotionId"),
                j => j.HasOne<Deposit>().WithMany().HasForeignKey("DepositId"));

        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Deposit>()
            .HasIndex(d => d.Name)
            .IsUnique();
    }

}