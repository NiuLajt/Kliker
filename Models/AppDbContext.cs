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

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<PlayersAchievement> PlayersAchievments { get; set; }

    public virtual DbSet<Upgrade> Upgrades { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
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
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PlayersAchievement>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("players&achievments_pkey");

            entity.ToTable("players&achievments", tb => tb.HasComment("Tabela pośrednia pomiędzy tabelą użytkowników a tabelą osiągnięć. Przechowuje ona informacje o zdobyciu przez konkretnego użytkownika konkretnego osiągnięcia wraz z datą tego wydarzenia."));

            entity.Property(e => e.PlayerId)
                .ValueGeneratedOnAdd()
                .HasColumnName("playerID");
            entity.Property(e => e.AchievementId).HasColumnName("achievementID");
            entity.Property(e => e.DateIfBeingAchieved).HasColumnName("dateIfBeingAchieved");

            entity.HasOne(d => d.Achievement).WithMany(p => p.PlayersAchievments)
                .HasForeignKey(d => d.AchievementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Foreign Key for achievements");

            entity.HasOne(d => d.Player).WithOne(p => p.PlayersAchievement)
                .HasForeignKey<PlayersAchievement>(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Foreign Key for users");
        });

        modelBuilder.Entity<Upgrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("upgrades_pkey");

            entity.ToTable("upgrades", tb => tb.HasComment("Przechowuje informacje o ulepszeniach nabywanych przez użytkowników. Każdy rekord to pojedyncze ulepszenie reprezentowane przez nazwę i opis."));

            entity.HasIndex(e => e.Name, "name is unique").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LevelRequired).HasColumnName("levelRequired");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(1)
                .HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", tb => tb.HasComment("Zawiera informacje o kontach użytkowników. Każdy rekord to reprezentacja jednego użytkownika w postaci loginu, maila, hasła..."));

            entity.HasIndex(e => e.Email, "email_unique").IsUnique();

            entity.HasIndex(e => e.Username, "username_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Banned)
                .HasDefaultValue(false)
                .HasColumnName("banned");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Lvl)
                .HasDefaultValue(0)
                .HasColumnName("lvl");
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(35)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
