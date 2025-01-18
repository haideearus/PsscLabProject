using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PsscFinalProject.Data.Models;

namespace PsscFinalProject.Data;

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

    public virtual DbSet<OrderItemDto> OrderItems { get; set; }

    public virtual DbSet<ProductDto> Products { get; set; }

    public virtual DbSet<UserDto> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer("Server=andreea-rus.database.windows.net;Database=PSSC_DataBase;User Id=andreea;Password=ProiectPSSC12!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillDto>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK_BILLS");

            entity.ToTable("BILL");

            entity.Property(e => e.BillId).HasColumnName("Bill_ID");
            entity.Property(e => e.BillingDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            //entity.HasOne(d => d.Order).WithMany(p => p.Bills)
            //    .HasForeignKey(d => d.OrderId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_BILLS_ORDER");
        });

        modelBuilder.Entity<ClientDto>(entity =>
        {
            entity.ToTable("CLIENT");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

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

            entity.HasOne(d => d.EmailNavigation).WithOne(p => p.Client)
                .HasForeignKey<ClientDto>(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CLIENT_Users");
        });

        modelBuilder.Entity<DeliveryDto>(entity =>
        {
            entity.ToTable("DELIVERY");

            entity.Property(e => e.DeliveryId).HasColumnName("Delivery_ID");
            entity.Property(e => e.Courier)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(100)
                .IsUnicode(false);

            //entity.HasOne(d => d.Order).WithMany(p => p.Deliveries)
            //    .HasForeignKey(d => d.OrderId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_DELIVERY_ORDER");
        });

        modelBuilder.Entity<OrderItemDto>(entity =>
        {
            entity.ToTable("ORDERITEM");
            entity.Property(e => e.OrderItemId).HasColumnName("OrderItem_Id");
            entity.Property(e => e.Price)
                  .HasColumnName("Price")
                  .HasColumnType("decimal(10, 2)"); // Define precision
            entity.Property(e => e.ProductCode).HasColumnName("ProductCode");
            entity.Property(e => e.Quantity).HasColumnName("Quantity");
        });

        // Configure ProductDto
        modelBuilder.Entity<ProductDto>(entity =>
        {
            entity.HasKey(e => e.ProductId); // Define the primary key
            entity.ToTable("PRODUCT");

            entity.Property(e => e.ProductId)
                .HasColumnName("Product_ID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.QuantityType)
                .HasMaxLength(30)
                .IsUnicode(false);

            // Define navigation property
            entity.HasMany(p => p.OrderItems)
                .WithOne()
                .HasForeignKey(o => o.ProductCode)
                .HasPrincipalKey(p => p.Code)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OrderDto>(entity =>
        {
            entity.ToTable("ORDER");
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.OrderDate).HasColumnName("OrderDate");
            entity.Property(e => e.PaymentMethod).HasColumnName("PaymentMethod");
            entity.Property(e => e.TotalAmount)
                  .HasColumnName("TotalAmount")
                  .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ShippingAddress).HasColumnName("ShippingAddress");
            entity.Property(e => e.State).HasColumnName("State");
            entity.Property(e => e.ClientEmail).HasColumnName("ClientEmail");
        });

        modelBuilder.Entity<UserDto>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK_Email");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasColumnType("text");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
