using Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data.Context
{
    public class LivrariaDbContext : DbContext
    {
        public LivrariaDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Editora> Editoras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model
                                                 .GetEntityTypes()
                                                 .SelectMany(e => e.GetProperties()
                                                                   .Where(p => p.ClrType == typeof(string))))
            {
                property.Relational().ColumnType = "varchar(200)";
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LivrariaDbContext).Assembly);

            foreach (var relationShip in modelBuilder.Model
                                                     .GetEntityTypes()
                                                     .SelectMany(e => e.GetForeignKeys()))
            {
                relationShip.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
