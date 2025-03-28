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

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameGenre> GameGenres { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("Game");

            entity.Property(e => e.GameId)
                .ValueGeneratedNever()
                .HasColumnName("Game_id");
            entity.Property(e => e.GameEngine)
                .HasMaxLength(25)
                .HasColumnName("game_engine");
            entity.Property(e => e.GameGenreId)
                .HasMaxLength(25)
                .HasColumnName("GameGenre_id");
            entity.Property(e => e.IsOnline).HasColumnName("isOnline");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");
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
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.PostId)
                .ValueGeneratedNever()
                .HasColumnName("Post_id");
            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.GameId).HasColumnName("Game_id");
            entity.Property(e => e.Message)
                .HasMaxLength(1250)
                .HasColumnName("message");
            entity.Property(e => e.ReactionId).HasColumnName("reaction_id");
            entity.Property(e => e.Title)
                .HasMaxLength(75)
                .HasColumnName("title");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
