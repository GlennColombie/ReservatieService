using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using ReservatieServiceBL.Entities;

namespace ReservatieServiceDL.Repositories;

public partial class ReservatieServiceContext : DbContext
{
    private string _connectionString;
    public ReservatieServiceContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ReservatieServiceContext(DbContextOptions<ReservatieServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gebruiker> Gebruikers { get; set; }

    public virtual DbSet<Locatie> Locaties { get; set; }

    public virtual DbSet<Reservatie> Reservaties { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<Tafel> Tafels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
        //optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ReservatieService;Integrated Security=True;TrustServerCertificate=True;");
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gebruiker>(entity =>
        {
            entity.ToTable("Gebruiker");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsVisible).HasColumnName("Is_visible");
            entity.Property(e => e.Naam)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefoonnummer)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Locatie).WithMany(p => p.Gebruikers)
                .HasForeignKey(d => d.LocatieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gebruiker_Locatie");
        });

        modelBuilder.Entity<Locatie>(entity =>
        {
            entity.ToTable("Locatie");

            entity.Property(e => e.Gemeente)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Huisnummer)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsVisible).HasColumnName("Is_visible");
            entity.Property(e => e.Straat)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Reservatie>(entity =>
        {
            entity.HasKey(e => e.Reservatienummer).HasName("PK_Reservatie_1");

            entity.ToTable("Reservatie");

            entity.Property(e => e.Datum).HasColumnType("datetime");
            entity.Property(e => e.Einduur).HasColumnType("datetime");
            entity.Property(e => e.Uur).HasColumnType("datetime");
            entity.Property(e => e.IsVisible).HasColumnName("Is_visible");

            entity.HasOne(d => d.Gebruiker).WithMany(p => p.Reservaties)
                .HasForeignKey(d => d.GebruikerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservatie_Gebruiker");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Reservaties)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservatie_Restaurant");

            entity.HasOne(d => d.Tafel).WithMany(p => p.Reservaties)
                .HasForeignKey(d => new { d.Tafelnummer, d.RestaurantId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservatie_Tafel");
        });
        
        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.ToTable("Restaurant");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsVisible).HasColumnName("Is_visible");
            entity.Property(e => e.Keuken)
                .HasConversion(
                v => v.ToString(),
                v => (Keuken)Enum.Parse(typeof(Keuken), v));
            entity.Property(e => e.Naam)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefoonnummer)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Locatie).WithMany(p => p.Restaurants)
                .HasForeignKey(d => d.LocatieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Restaurant_Locatie");
        });

        modelBuilder.Entity<Tafel>(entity =>
        {
            entity.HasKey(e => new { e.Tafelnummer, e.RestaurantId });

            entity.ToTable("Tafel");

            entity.Property(e => e.IsVisible).HasColumnName("Is_visible");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Tafels)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tafel_Restaurant");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override int SaveChanges()
    {
        UpdateSoftDeleteStatuses();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateSoftDeleteStatuses();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateSoftDeleteStatuses()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("IsVisible").CurrentValue = 1;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Property("IsVisible").CurrentValue = 0;
                    break;
            }
        }
    }
}