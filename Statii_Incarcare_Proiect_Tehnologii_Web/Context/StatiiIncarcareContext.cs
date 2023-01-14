using Microsoft.EntityFrameworkCore;
using Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Context;

public partial class StatiiIncarcareContext : DbContext
{
    public StatiiIncarcareContext()
    {
    }

    public StatiiIncarcareContext(DbContextOptions<DbContext> options)
        : base(options)
    {
        
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Plug> Plugs { get; set; }

    public virtual DbSet<PlugType> PlugTypes { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<StationToAdmin> StationToAdmins { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=LENOVO;Database=Statii;Trusted_Connection=True;encrypt=false;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Bookings__3213E83F0764AEC1");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.end_time).HasColumnType("datetime");
            entity.Property(e => e.start_time).HasColumnType("datetime");

            entity.HasOne(d => d.plug).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.plug_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Bookings_fk0");

            entity.HasOne(d => d.user).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.user_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Bookings_fk1");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Cars__3213E83F289103ED");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.car_plate)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.user).WithMany(p => p.Cars)
                .HasForeignKey(d => d.user_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Cars_fk0");
        });

        modelBuilder.Entity<Plug>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Plugs__3213E83F244B6333");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.is_charging)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.station).WithMany(p => p.Plugs)
                .HasForeignKey(d => d.station_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Plugs_fk0");

            entity.HasOne(d => d.typeNavigation).WithMany(p => p.Plugs)
                .HasForeignKey(d => d.type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Plugs_fk1");
        });

        modelBuilder.Entity<PlugType>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Plug_typ__3213E83F4B7156D4");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Stations__3213E83FF6AB155E");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.city)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.coordX).HasColumnType("float");
            entity.Property(e => e.coordY).HasColumnType("float");
            entity.Property(e => e.name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StationToAdmin>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__StationT__3213E83F8DF11D1A");

            entity.ToTable("StationToAdmin");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.admin).WithMany(p => p.StationToAdmins)
                .HasForeignKey(d => d.admin_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StationToAdmin_fk1");

            entity.HasOne(d => d.station).WithMany(p => p.StationToAdmins)
                .HasForeignKey(d => d.station_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StationToAdmin_fk0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Users__3213E83F2A3802A7");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.is_admin)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.is_charging)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.password)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
