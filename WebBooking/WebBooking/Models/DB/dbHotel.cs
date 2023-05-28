using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebBooking.Models.DB
{
    public partial class dbHotel : DbContext
    {
        public dbHotel()
            : base("name=dbHotel")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryNew> CategoryNews { get; set; }
        public virtual DbSet<CategoryService> CategoryServices { get; set; }
        public virtual DbSet<Credit> Credits { get; set; }
        public virtual DbSet<New> News { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Servive> Servives { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<StatusCredit> StatusCredits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Bill>()
                .Property(e => e.total)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Bill>()
                .Property(e => e.totalprice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Booking>()
                .Property(e => e.phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Booking>()
                .Property(e => e.identiyid)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Booking>()
                .Property(e => e.total)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Booking>()
                .HasMany(e => e.Bills)
                .WithRequired(e => e.Booking)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Room>()
                .Property(e => e.promotion)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.Room)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Servive>()
                .Property(e => e.price)
                .HasPrecision(19, 4);
        }
    }
}
