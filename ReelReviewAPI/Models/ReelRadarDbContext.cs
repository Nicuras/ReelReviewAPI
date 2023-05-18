using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ReelReviewAPI.Models;

public partial class ReelRadarDbContext : DbContext
{
    public override int SaveChanges()
    {
        // Create serializer options with reference handling.
        var serializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        // Iterate through tracked entities and serialize any properties marked with [JsonConverter].
        foreach (var entityEntry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            var entity = entityEntry.Entity;
            var properties = entity.GetType().GetProperties().Where(p => p.GetCustomAttribute<JsonConverterAttribute>() != null);

            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                var serializedValue = JsonSerializer.Serialize(value, serializerOptions);
                property.SetValue(entity, serializedValue);
            }
        }

        // Call the base SaveChanges method.
        return base.SaveChanges();
    }
    public ReelRadarDbContext()
    {
    }

    public ReelRadarDbContext(DbContextOptions<ReelRadarDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieActor> MovieActors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI\\SQLEXPRESS;Initial Catalog=ReelRadarDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("Actor");

            entity.Property(e => e.ActorId).HasColumnName("ActorID");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.ToTable("Director");

            entity.Property(e => e.DirectorId).HasColumnName("DirectorID");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");

        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.DirectorId).HasColumnName("DirectorID");
            entity.Property(e => e.MovieLength)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<MovieActor>(entity =>
        {
            entity.HasKey(e => e.MovieActorId).HasName("PK_MovieActor_1");

            entity.ToTable("MovieActor");

            entity.Property(e => e.MovieActorId)
                .ValueGeneratedNever()
                .HasColumnName("MovieActorID");
            entity.Property(e => e.ActorId).HasColumnName("ActorID");


            
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
