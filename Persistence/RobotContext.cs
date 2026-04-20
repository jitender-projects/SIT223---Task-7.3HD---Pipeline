using Microsoft.EntityFrameworkCore;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public partial class RobotContext : DbContext
{
    public RobotContext()
    {
    }

    public RobotContext(DbContextOptions<RobotContext> options)
        : base(options)
    {
    }

    // These DbSets represent your database tables
    public virtual DbSet<Map> Maps { get; set; }
    public virtual DbSet<RobotCommand> RobotCommands { get; set; }

    // This method configures the database connection and turns on SQL logging
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Note: We are turning on detailed logging here as requested in the task sheet
            optionsBuilder
                .UseNpgsql("Host=localhost; Database=sit331; Username=postgres; Password=Pass1234")
                .LogTo(Console.Write)
                .EnableSensitiveDataLogging();
        }
    }

    // This method maps your PascalCase C# properties to the lowercase PostgreSQL columns
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_map");
            entity.ToTable("map");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasColumnName("Name"); 
            entity.Property(e => e.Columns).HasColumnName("columns");
            entity.Property(e => e.Rows).HasColumnName("rows");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
        });

        modelBuilder.Entity<RobotCommand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_robotcommand");
            entity.ToTable("robotcommand");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsMoveCommand).HasColumnName("ismovecommand");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifieddate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}