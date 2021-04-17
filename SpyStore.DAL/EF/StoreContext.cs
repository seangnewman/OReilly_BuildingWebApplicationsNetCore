using Microsoft.EntityFrameworkCore;
using SpyStore.Models.Entities;
using System;

namespace SpyStore.DAL.EF
{
    public class StoreContext : DbContext
    {


        public StoreContext()
        {

        }
        public StoreContext(DbContextOptions options) : base(options)
        {
            try
            {
                Database.Migrate();
            }
            catch (Exception)
            {

                // Should do something intelligent here
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\mssqllocaldb;Database=SpyStore;Trusted_Connection=True;MultipleActiveResultSets=true;"
                        , b => b.MigrationsAssembly("SpyStore.DAL")


                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create a unique index for EmailAddress field
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress).HasDatabaseName("IX_Customers").IsUnique();
            });

            // Setting default values for Order 
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.ShipDate).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.OrderTotal).HasColumnType("money").HasComputedColumnSql("Store.GetOrderTotal([Id])");
            });


            // Create the Computed columns for OrderDetails
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.LineItemTotal).HasColumnType("money").HasComputedColumnSql("[Quantity] * [UnitCost]");
                entity.Property(e => e.UnitCost).HasColumnType("money");
            });

            // Specify the SQL Server Money Type for the Product table
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.UnitCost).HasColumnType("money");
                entity.Property(e => e.CurrentPrice).HasColumnType("money");
            });

            // Updating the ShoppingCard Record Model
            modelBuilder.Entity<ShoppingCartRecord>(entity =>
            {
                entity.HasIndex(e => new { ShoppingCartRecordId = e.Id, e.ProductId, e.CustomerId })
                    .HasDatabaseName("IX_ShoppingCart")
                    .IsUnique();

                entity.Property(e => e.DateCreated).HasColumnType("datetime").HasDefaultValueSql("getdate()");
                entity.Property(e => e.Quantity).HasDefaultValue(1);

            });

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartRecord> ShoppingCartRecords { get; set; }
    }
}
