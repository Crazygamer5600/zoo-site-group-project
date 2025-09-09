using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectStudioApp.Models;

public partial class ZooliranteDbContext : DbContext
{
    public ZooliranteDbContext()
    {
    }

    public ZooliranteDbContext(DbContextOptions<ZooliranteDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Merchandise> Merchandises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-D46DJ95O\\SQLEXPRESS;Initial Catalog=ZooliranteDB;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("pk_account");

            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contactNumber");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastName");
        });

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.AnimalId).HasName("pk_animalID");

            entity.Property(e => e.AnimalId).HasColumnName("animalID");
            entity.Property(e => e.AnimalLocation)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("animalLocation");
            entity.Property(e => e.AnimalPhoto)
                .IsUnicode(false)
                .HasColumnName("animalPhoto");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ExtraInfo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("extraInfo");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Species)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("species");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("pk_bookingID");

            entity.Property(e => e.BookingId).HasColumnName("bookingID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.ReservationSize).HasColumnName("reservationSize");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("pk_events");

            entity.Property(e => e.EventId).HasColumnName("eventID");
            entity.Property(e => e.EventEnd)
                .HasColumnType("datetime")
                .HasColumnName("eventEnd");
            entity.Property(e => e.EventName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("eventName");
            entity.Property(e => e.EventPhoto)
                .IsUnicode(false)
                .HasColumnName("eventPhoto");
            entity.Property(e => e.EventStart)
                .HasColumnType("datetime")
                .HasColumnName("eventStart");
            entity.Property(e => e.EventType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("eventType");
            entity.Property(e => e.MaxAttendeeSize).HasColumnName("maxAttendeeSize");

            entity.HasMany(d => d.Animals).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventAnimal",
                    r => r.HasOne<Animal>().WithMany()
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__EventAnim__anima__3C69FB99"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__EventAnim__event__3B75D760"),
                    j =>
                    {
                        j.HasKey("EventId", "AnimalId").HasName("PK__EventAni__2B40F80AB1F123C8");
                        j.ToTable("EventAnimals");
                        j.IndexerProperty<int>("EventId").HasColumnName("eventID");
                        j.IndexerProperty<int>("AnimalId").HasColumnName("animalID");
                    });

            entity.HasMany(d => d.Bookings).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventAttendee",
                    r => r.HasOne<Booking>().WithMany()
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__EventAtte__booki__4222D4EF"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__EventAtte__event__412EB0B6"),
                    j =>
                    {
                        j.HasKey("EventId", "BookingId").HasName("PK__EventAtt__E1AABED72C70C49F");
                        j.ToTable("EventAttendees");
                        j.IndexerProperty<int>("EventId").HasColumnName("eventID");
                        j.IndexerProperty<int>("BookingId").HasColumnName("bookingID");
                    });
        });

        modelBuilder.Entity<Merchandise>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("pk_itemID");

            entity.ToTable("Merchandise");

            entity.Property(e => e.ItemId).HasColumnName("itemID");
            entity.Property(e => e.ItemCost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("itemCost");
            entity.Property(e => e.ItemDescription)
                .IsUnicode(false)
                .HasColumnName("itemDescription");
            entity.Property(e => e.ItemImage)
                .IsUnicode(false)
                .HasColumnName("itemImage");
            entity.Property(e => e.ItemName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("itemName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
