using Microsoft.EntityFrameworkCore;
using SystemBackend.Models.Entities;

namespace SystemBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PendingUser> PendingUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Civilian> Civilians { get; set; }
        public DbSet<RoomMember> RoomMembers { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.HasKey(a =>  a.Id);

                entity.Property(a => a.Username).IsRequired();
                entity.Property(a => a.Password).IsRequired();
                entity.Property(a => a.Role).IsRequired()
                    .HasDefaultValue(UserRoleType.Admin)
                    .HasConversion<string>();
                entity.Property(a => a.Email).IsRequired();

                entity.HasIndex(a => a.Email).IsUnique();
                entity.HasIndex(a => a.Username).IsUnique();
            });

            modelBuilder.Entity<PendingUser>(entity => {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Username).IsRequired();
                entity.Property(a => a.Password).IsRequired();
                entity.Property(a => a.Role).IsRequired()
                    .HasDefaultValue(UserRoleType.Admin)
                    .HasConversion<string>();
                entity.Property(a => a.Email).IsRequired();

                entity.HasIndex(a => a.Email);
                entity.HasIndex(a => a.Username).IsUnique();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Token);

                entity.Property(rt => rt.CreatedAt).IsRequired();
                entity.Property(rt => rt.ExpiredAt).IsRequired();
                entity.Property(rt => rt.UserId).IsRequired();

                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name).IsRequired();
                entity.Property(r => r.Location).IsRequired();

                entity.HasIndex(r => new { r.Name, r.Location }).IsUnique();
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasOne(d => d.Room)
                    .WithMany(r => r.Devices)
                    .HasForeignKey(d => d.RoomId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Civilian>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name).IsRequired();
                entity.Property(c => c.DateOfBirth).IsRequired();
                entity.Property(c => c.Hometown).IsRequired();
            });

            modelBuilder.Entity<RoomMember>(entity =>
            {
                entity.HasKey(rm => rm.Id);

                entity.Property(rm => rm.RoomId).IsRequired();
                entity.Property(rm => rm.MemberId).IsRequired();
                entity.Property(rm => rm.StartTime).IsRequired()
                        .HasDefaultValue(DateTime.MinValue);
                entity.Property(rm => rm.EndTime).IsRequired()
                        .HasDefaultValue(DateTime.MaxValue);

                entity.HasOne(rm => rm.Room)
                    .WithMany(r => r.Members)
                    .HasForeignKey(rm => rm.RoomId);

                entity.HasOne(rm => rm.Member)
                    .WithMany(m => m.RoomMembers)
                    .HasForeignKey(rm => rm.MemberId);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CHK_RoomMember_MainPeriodValid", "StartTime < EndTime");

                    t.HasCheckConstraint("CHK_RoomMember_DisabledPeriodValid",
                        "(DisabledStartTime IS NULL AND DisabledEndTime IS NULL) OR " +
                        "(DisabledStartTime IS NOT NULL AND DisabledEndTime IS NOT NULL AND DisabledStartTime < DisabledEndTime)");

                    t.HasCheckConstraint("CHK_RoomMember_DisabledPeriodWithinMain",
                        "(DisabledStartTime IS NULL AND DisabledEndTime IS NULL) OR " +
                        "(DisabledStartTime IS NOT NULL AND DisabledEndTime IS NOT NULL AND DisabledStartTime >= StartTime AND DisabledEndTime <= EndTime)");
                });

                entity.HasIndex(rm => new { rm.MemberId, rm.RoomId }).IsUnique();
                entity.HasIndex(rm => rm.MemberId);
                entity.HasIndex(rm => rm.RoomId);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.ImageUrl).IsRequired();
                entity.Property(l => l.DeviceId).IsRequired();
                entity.Property(l => l.RoomId).IsRequired();
                entity.Property(l => l.CivilianId).IsRequired();
                entity.Property(l => l.CreatedAt).IsRequired();

                entity.HasOne(l => l.Device)
                    .WithMany(d => d.Logs)
                    .HasForeignKey(l => l.DeviceId);

                entity.HasOne(l => l.Room)
                    .WithMany(r => r.Logs)
                    .HasForeignKey(l => l.RoomId);

                entity.HasOne(l => l.Civilian)
                    .WithMany(c => c.Logs)
                    .HasForeignKey(l => l.CivilianId);

                entity.HasIndex(l => l.CreatedAt).IsDescending(false);
                entity.HasIndex(l => l.CreatedAt).IsDescending();
                
                entity.HasIndex(l => l.DeviceId);
                entity.HasIndex(l => l.CivilianId);
                entity.HasIndex(l => l.RoomId);
            });
        }
    }
}
