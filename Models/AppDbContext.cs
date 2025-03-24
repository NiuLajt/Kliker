using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kliker.Models;

public partial class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration): base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<PlayersAchievement> PlayersAchievments { get; set; }

    public virtual DbSet<PlayersUpgrade> PlayersUpgrades { get; set; }

    public virtual DbSet<Upgrade> Upgrades { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Primary Key");

            entity.ToTable("achievements", tb => tb.HasComment("Przechowuje informacje o osiągnięciach. Każdy rekord to jedno dostępne do zdobycia osiągnięcie reprezentowane poprzez nazwę oraz opis."));

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PlayersAchievement>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PlayersAchievments_pkey");

            entity.ToTable("PlayersAchievments", tb => tb.HasComment("Tabela pośrednia pomiędzy tabelą użytkowników a tabelą osiągnięć. Przechowuje ona informacje o zdobyciu przez konkretnego użytkownika konkretnego osiągnięcia wraz z datą tego wydarzenia."));

            entity.Property(e => e.PlayerId)
                .ValueGeneratedOnAdd()
                .HasColumnName("playerId");
            entity.Property(e => e.AchievementId).HasColumnName("achievementId");
            entity.Property(e => e.DateIfBeingAchieved).HasColumnName("dateIfBeingAchieved");

            entity.HasOne(d => d.Achievement).WithMany(p => p.PlayersAchievements)
                .HasForeignKey(d => d.AchievementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Foreign Key for achievements");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayersAchievements)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Foreign Key for users");
        });

        modelBuilder.Entity<PlayersUpgrade>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.UpgradeId }).HasName("PlayersUpgrades_pkey");

            entity.ToTable("PlayerUpgrades", tb => tb.HasComment("Tabela pomocnicza zawierająca informacje o odblokowaniu ulepszeń przez graczy. Każdy rekord to reprezentacja odblokowania ulepszenia o konkretnym ID przez gracza o konkretnym ID."));

            entity.Property(e => e.PlayerId).HasColumnName("PlayerId");
            entity.Property(e => e.UpgradeId).HasColumnName("UpgradeId");
            entity.Property(e => e.DateOfBeingUpgraded).HasColumnName("DateOfBeingUpgraded");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayersUpgrades)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerId foreign key");

            entity.HasOne(d => d.Upgrade).WithMany(p => p.PlayersUpgrades)
                .HasForeignKey(d => d.UpgradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UpgradeId foreign key");
        });

        modelBuilder.Entity<Upgrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Upgrades_pkey");

            entity.ToTable("Upgrades", tb => tb.HasComment("Przechowuje informacje o ulepszeniach nabywanych przez użytkowników. Każdy rekord to pojedyncze ulepszenie reprezentowane przez nazwę i opis."));

            entity.HasIndex(e => e.Name, "name is unique").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.Description).HasColumnName("Description");
            entity.Property(e => e.LevelRequired).HasColumnName("LevelRequired");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("Name");
            entity.Property(e => e.Price)
                .HasPrecision(1)
                .HasColumnName("Price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.ToTable("Users", tb => tb.HasComment("Zawiera informacje o kontach użytkowników. Każdy rekord to reprezentacja jednego użytkownika w postaci loginu, maila, hasła..."));

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Points)
                .HasDefaultValue(0)
                .HasColumnName("Points");
            entity.Property(e => e.Banned)
                .HasDefaultValue(false)
                .HasColumnName("Banned");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Email");
            entity.Property(e => e.Lvl)
                .HasDefaultValue(0)
                .HasColumnName("Lvl");
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasColumnName("PasswordHash");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(35)
                .HasColumnName("Username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
