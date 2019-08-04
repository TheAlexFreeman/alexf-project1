using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebStore.Data.Entities
{
    public partial class StoreDBContext : DbContext
    {
        public StoreDBContext()
        {
        }

        public StoreDBContext(DbContextOptions<StoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<InventoryItem> InventoryItem { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductItem> ProductItem { get; set; }
        public virtual DbSet<ProductLocation> ProductLocation { get; set; }
        public virtual DbSet<ProductOrder> ProductOrder { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "Store");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DefaultStoreId).HasColumnName("DefaultStoreID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.HasOne(d => d.DefaultStore)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.DefaultStoreId)
                    .HasConstraintName("FK_Customers_DefaultStoreID_to_Location");
            });

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.ItemId })
                    .HasName("PK_LocationID_ItemID");

                entity.ToTable("InventoryItem", "Store");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InventoryItem)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_InventoryItem_ItemID_to_Item");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.InventoryItem)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_InventoryItem_LocationID_to_Location");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item", "Store");

                entity.HasIndex(e => e.Name)
                    .HasName("U_ItemName")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cost).HasColumnType("money");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "Store");

                entity.HasIndex(e => e.Name)
                    .HasName("U_StoreName")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order", "Store");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BuyerId).HasColumnName("BuyerID");

                entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SellerId).HasColumnName("SellerID");

                entity.Property(e => e.Start).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("FK_Order_BuyerID_to_Customer");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_SellerID_to_Location");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product", "Store");

                entity.HasIndex(e => e.Name)
                    .HasName("U_ProductName")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<ProductItem>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ItemId })
                    .HasName("PK_ProductID_ItemID");

                entity.ToTable("ProductItem", "Store");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ProductItem)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ProductItem_ItemID_to_Item");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductItem)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductItem_ProductID_to_Product");
            });

            modelBuilder.Entity<ProductLocation>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.LocationId })
                    .HasName("PK_ProductID_LocationID");

                entity.ToTable("ProductLocation", "Store");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.ProductLocation)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_ProductLocation_LocationID_to_Location");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductLocation)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductLocation_ProductID_to_Product");
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK_OrderID_ProductID");

                entity.ToTable("ProductOrder", "Store");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_ProductOrder_OrderID_to_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOrder)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductOrder_ProductID_to_Product");
            });
        }
    }
}
