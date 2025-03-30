using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TP_WS1.Models;

public partial class Tp1Ws1JeuxVideoContext : DbContext
{
    public Tp1Ws1JeuxVideoContext()
    {
    }

    public Tp1Ws1JeuxVideoContext(DbContextOptions<Tp1Ws1JeuxVideoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EnumRole> EnumRoles { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameGenre> GameGenres { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EnumRole>(entity =>
        {
            entity.ToTable("Enum Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("role");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Game");

            entity.Property(e => e.GameId).HasColumnName("Game_id");
            entity.Property(e => e.GameEngine)
                .HasMaxLength(25)
                .HasColumnName("game_engine");
            entity.Property(e => e.GameGenreId)
                .HasMaxLength(25)
                .HasColumnName("GameGenre_id");
            entity.Property(e => e.IsArchived).HasColumnName("isArchived");
            entity.Property(e => e.IsOnline).HasColumnName("isOnline");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.GameGenre).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameGenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Game_GameGenre");

            entity.HasOne(d => d.User).WithMany(p => p.Games)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Game_users");
        });

        modelBuilder.Entity<GameGenre>(entity =>
        {
            entity.ToTable("GameGenre");

            entity.Property(e => e.GameGenreId)
                .HasMaxLength(25)
                .HasColumnName("GameGenre_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasColumnName("fullName");
            entity.Property(e => e.GameTypeId).HasColumnName("game_type_id");
            entity.Property(e => e.IsArchived).HasColumnName("isArchived");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.PostId).HasColumnName("Post_id");
            entity.Property(e => e.GameId).HasColumnName("Game_id");
            entity.Property(e => e.IsArchived).HasColumnName("isArchived");
            entity.Property(e => e.Message)
                .HasMaxLength(1250)
                .HasColumnName("message");
            entity.Property(e => e.ReactionId).HasColumnName("reaction_id");
            entity.Property(e => e.Title)
                .HasMaxLength(75)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Game).WithMany(p => p.Posts)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Game");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_users1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__CB9A1CFF4B730E53");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_users_Enum Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
