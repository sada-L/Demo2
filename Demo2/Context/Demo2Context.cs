using System;
using System.Collections.Generic;
using Demo2.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo2.Context;

public partial class Demo2Context : DbContext
{
    public Demo2Context()
    {
    }

    public Demo2Context(DbContextOptions<Demo2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productphoto> Productphotos { get; set; }

    public virtual DbSet<Productsale> Productsales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=89.110.53.87;Port=5522;Database=demo2;Username=postgres;password=QWEasd123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_product");

            entity.ToTable("product");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Mainimagepath)
                .HasMaxLength(1000)
                .HasColumnName("mainimagepath");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.Description)
              .HasColumnName("description");

            entity.HasMany(d => d.Attachedproducts).WithMany(p => p.Mainproducts)
                .UsingEntity<Dictionary<string, object>>(
                    "Attachedproduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("Attachedproductid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_attachedproduct_product1"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("Mainproductid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_attachedproduct_product"),
                    j =>
                    {
                        j.HasKey("Mainproductid", "Attachedproductid").HasName("pk_attachedproduct");
                        j.ToTable("attachedproduct");
                        j.IndexerProperty<int>("Mainproductid").HasColumnName("mainproductid");
                        j.IndexerProperty<int>("Attachedproductid").HasColumnName("attachedproductid");
                    });

            entity.HasMany(d => d.Mainproducts).WithMany(p => p.Attachedproducts)
                .UsingEntity<Dictionary<string, object>>(
                    "Attachedproduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("Mainproductid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_attachedproduct_product"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("Attachedproductid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_attachedproduct_product1"),
                    j =>
                    {
                        j.HasKey("Mainproductid", "Attachedproductid").HasName("pk_attachedproduct");
                        j.ToTable("attachedproduct");
                        j.IndexerProperty<int>("Mainproductid").HasColumnName("mainproductid");
                        j.IndexerProperty<int>("Attachedproductid").HasColumnName("attachedproductid");
                    });
        });

        modelBuilder.Entity<Productphoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_productphoto");

            entity.ToTable("productphoto");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Photopath)
                .HasMaxLength(1000)
                .HasColumnName("photopath");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Product).WithMany(p => p.Productphotos)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productphoto_product");
        });

        modelBuilder.Entity<Productsale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_productsale");

            entity.ToTable("productsale");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Saledate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("saledate");

            entity.HasOne(d => d.Product).WithMany(p => p.Productsales)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productsale_product");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
