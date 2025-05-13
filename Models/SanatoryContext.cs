using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace SanatoryApi.Models;

public partial class SanatoryContext : DbContext
{
    public SanatoryContext()
    {
    }

    public SanatoryContext(DbContextOptions<SanatoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cabinet> Cabinets { get; set; }

    public virtual DbSet<Day> Days { get; set; }

    public virtual DbSet<Daytime> Daytimes { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<JobTitle> JobTitles { get; set; }

    public virtual DbSet<Problem> Problems { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusProblem> StatusProblems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.200.13;user=student;password=student;database=sanatory", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.39-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cabinet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Cabinet");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Number).HasColumnType("int(11)");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Day>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Day1)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("Day");
        });

        modelBuilder.Entity<Daytime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Daytime");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");


            entity.HasMany(d => d.Events).WithMany(p => p.Days)
                .UsingEntity<Dictionary<string, object>>(
                    "CrossDayEvent",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CrossDayEvent_Events_ID"),
                    l => l.HasOne<Daytime>().WithMany()
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CrossDayEvent_Daytime_ID"),
                    j =>
                    {
                        j.HasKey("DayId", "EventId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("CrossDayEvent");
                        j.HasIndex(new[] { "EventId" }, "FK_CrossDayEvent_Events_ID");
                        j.IndexerProperty<int>("DayId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("EventId").HasColumnType("int(11)");
                    });
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Date).HasMaxLength(255);
            entity.Property(e => e.Duration).HasColumnType("int(11)");
            entity.Property(e => e.Place)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.RoomId, "FK_Guests_Rooms_ID");

            entity.HasIndex(e => e.UserId, "FK_Guests_Users_Id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Pasport)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Policy)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.RoomId)
                .HasColumnType("int(11)")
                .HasColumnName("RoomID");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Room).WithMany(p => p.Guests)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Guests_Rooms_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Guests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Guests_Users_Id");

            entity.HasMany(d => d.Procedures).WithMany(p => p.Guests)
                .UsingEntity<Dictionary<string, object>>(
                    "CrossGuestProcedure",
                    r => r.HasOne<Procedure>().WithMany()
                        .HasForeignKey("ProcedureId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CrossGuestProcedure_Procedures_ID"),
                    l => l.HasOne<Guest>().WithMany()
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CrossGuestProcedure_Guests_ID"),
                    j =>
                    {
                        j.HasKey("GuestId", "ProcedureId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("CrossGuestProcedure");
                        j.HasIndex(new[] { "ProcedureId" }, "FK_CrossGuestProcedure_Procedures_ID");
                        j.IndexerProperty<int>("GuestId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("ProcedureId").HasColumnType("int(11)");
                    });
        });

        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("JobTitle");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Problem");

            entity.HasIndex(e => e.StatusProblemId, "FK_Problem_StatusProblem_Id");
            entity.HasIndex(e => e.StaffId, "FK_Problem_Staff_ID");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Place)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.StatusProblemId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");
            entity.Property(e => e.StaffId)
               .HasColumnType("int(11)");

            entity.HasOne(d => d.StatusProblem).WithMany(p => p.Problems)
                .HasForeignKey(d => d.StatusProblemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Problem_StatusProblem_Id");

            entity.HasOne(d => d.Staff).WithMany(p => p.Problems)
               .HasForeignKey(d => d.StaffId)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_Problem_Staff_ID");

        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Duration).HasColumnType("int(11)");
            entity.Property(e => e.Price).HasPrecision(19, 2);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.StatusId, "FK_Rooms_Status_Id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Number).HasColumnType("int(11)");
            entity.Property(e => e.Price).HasPrecision(10);
            entity.Property(e => e.StatusId).HasColumnType("int(11)");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");

            entity.HasOne(d => d.Status).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_Status_Id");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.CabinetId, "FK_Staff_Cabinet_ID");

            entity.HasIndex(e => e.JobTitleId, "FK_Staff_JobTitle_Id");

            entity.HasIndex(e => e.UserId, "FK_Staff_Users_Id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.CabinetId)
                .HasColumnType("int(11)")
                .HasColumnName("CabinetID");
            entity.Property(e => e.JobTitleId).HasColumnType("int(11)");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Mail)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Cabinet).WithMany(p => p.Staff)
                .HasForeignKey(d => d.CabinetId)
                .HasConstraintName("FK_Staff_Cabinet_ID");

            entity.HasOne(d => d.JobTitle).WithMany(p => p.Staff)
                .HasForeignKey(d => d.JobTitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_JobTitle_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Staff)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Staff_Users_Id");

            entity.HasMany(d => d.Days).WithMany(p => p.Staff)
                .UsingEntity<Dictionary<string, object>>(
                    "CrossDayStaff",
                    r => r.HasOne<Day>().WithMany()
                        .HasForeignKey("DaysId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CrossDayStaff_Days_ID"),
                    l => l.HasOne<Staff>().WithMany()
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CrossDayStaff_Staff_ID"),
                    j =>
                    {
                        j.HasKey("StaffId", "DaysId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("CrossDayStaff");
                        j.HasIndex(new[] { "DaysId" }, "FK_CrossDayStaff_Days_ID");
                        j.IndexerProperty<int>("StaffId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("DaysId").HasColumnType("int(11)");
                    });
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Status");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<StatusProblem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("StatusProblem");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.RoleId, "FK_Users_Role_Id");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.RoleId).HasColumnType("int(11)");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Role_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
