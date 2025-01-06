using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PsscFinalProject.Data.Models;

public partial class PsscDbContext : DbContext
{
    public PsscDbContext()
    {
    }

    public PsscDbContext(DbContextOptions<PsscDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BillDto> Bills { get; set; }

    public virtual DbSet<ClientDto> Clients { get; set; }

    public virtual DbSet<DeliveryDto> Deliveries { get; set; }

    public virtual DbSet<OrderDto> Orders { get; set; }

    public virtual DbSet<OrderitemDto> Orderitems { get; set; }

    public virtual DbSet<ProductDto> Products { get; set; }

    public virtual DbSet<UserDto> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=andreea-rus.database.windows.net;Database=PSSC_DataBase;User Id=andreea;Password=ProiectPSSC12!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillDto>(entity =>
        {
            entity.ToTable("BILL");

            entity.Property(e => e.BillId)
                .ValueGeneratedNever()
                .HasColumnName("Bill_ID");
            entity.Property(e => e.BillingDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Order_ID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.Bills)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BILL_ORDER");
        });

        modelBuilder.Entity<ClientDto>(entity =>
        {
            entity.ToTable("CLIENT");

            entity.Property(e => e.ClientId).HasColumnName("Client_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CLIENT_Users");
        });

        modelBuilder.Entity<DeliveryDto>(entity =>
        {
            entity.ToTable("DELIVERY");

            entity.Property(e => e.DeliveryId)
                .ValueGeneratedNever()
                .HasColumnName("Delivery_ID");
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Order_ID");
            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DELIVERY_ORDER");
        });

        modelBuilder.Entity<OrderDto>(entity =>
        {
            entity.ToTable("ORDER");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("Order_ID");
            entity.Property(e => e.ClientId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Client_ID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.ShippingAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER_CLIENT");
        });

        modelBuilder.Entity<OrderitemDto>(entity =>
        {
            entity.ToTable("ORDERITEM");

            entity.Property(e => e.OrderItemId)
                .ValueGeneratedNever()
                .HasColumnName("OrderItem_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.ProductId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Product_ID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDERITEM_ORDER");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDERITEM_PRODUCT");
        });

        modelBuilder.Entity<ProductDto>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_PRODUCT_1");

            entity.ToTable("PRODUCT");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.QuantityType)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserDto>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Users_1");

            entity.HasIndex(e => e.Email, "IX_Users").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("User_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Password).HasColumnType("text");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
