using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Common.Models;
namespace Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aianalysis> Aianalyses { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Sla> Slas { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketAssignment> TicketAssignments { get; set; }

    public virtual DbSet<TicketClosure> TicketClosures { get; set; }

    public virtual DbSet<TicketStatusHistory> TicketStatusHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TicketManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aianalysis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AIAnalys__3214EC0785E3FF06");

            entity.ToTable("AIAnalysis");

            entity.Property(e => e.AnalyzedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PredictedCategoryId).HasColumnName("PredictedCategoryID");
            entity.Property(e => e.RiskLevel).HasMaxLength(50);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.UrgencyScore).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.PredictedCategory).WithMany(p => p.Aianalyses)
                .HasForeignKey(d => d.PredictedCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AI_Categories");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Aianalyses)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AI_Tickets");
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attachme__3214EC0757ABB881");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileType).HasMaxLength(50);
            entity.Property(e => e.FileUrl).HasMaxLength(500);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UploadedByUserId).HasColumnName("UploadedByUserID");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachments_Tickets");

            entity.HasOne(d => d.UploadedByUser).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.UploadedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attachments_Users");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC075FEF34B8");

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC079BC65C75");

            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC07D2AD804C");

            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Messages)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Tickets");

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_Users");
        });

        modelBuilder.Entity<Sla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SLA__3214EC07C27A0166");

            entity.ToTable("SLA");

            entity.HasIndex(e => new { e.CategoryId, e.UrgencyLevel }, "UQ_SLA_Category_Urgency").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

            entity.HasOne(d => d.Category).WithMany(p => p.Slas)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SLA_Categories");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC07109F5126");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CurrentStatus).HasMaxLength(50);
            entity.Property(e => e.OpenedByUserId).HasColumnName("OpenedByUserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Categories");

            entity.HasOne(d => d.OpenedByUser).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.OpenedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Users");
        });

        modelBuilder.Entity<TicketAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketAs__3214EC07B9E93069");

            entity.ToTable("TicketAssignment");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.RoleInTicket).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketAssignments)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Tickets");

            entity.HasOne(d => d.User).WithMany(p => p.TicketAssignments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_Users");
        });

        modelBuilder.Entity<TicketClosure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketCl__3214EC0799F36CE3");

            entity.ToTable("TicketClosure");

            entity.HasIndex(e => e.TicketId, "UQ__TicketCl__712CC626E457E331").IsUnique();

            entity.Property(e => e.ClosedAt).HasColumnType("datetime");
            entity.Property(e => e.ClosureReason).HasMaxLength(100);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");

            entity.HasOne(d => d.Ticket).WithOne(p => p.TicketClosure)
                .HasForeignKey<TicketClosure>(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Closure_Tickets");
        });

        modelBuilder.Entity<TicketStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketSt__3214EC07B2F920D3");

            entity.ToTable("TicketStatusHistory");

            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ChangedByUserId).HasColumnName("ChangedByUserID");
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.NewStatus).HasMaxLength(50);
            entity.Property(e => e.PreviousStatus).HasMaxLength(50);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");

            entity.HasOne(d => d.ChangedByUser).WithMany(p => p.TicketStatusHistories)
                .HasForeignKey(d => d.ChangedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_Users");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketStatusHistories)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_Tickets");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07CBC68B84");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployeeId).HasMaxLength(50).HasColumnName("EmployeeId");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.JobTitle).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.ResetPasswordToken).HasMaxLength(500);
            entity.Property(e => e.ResetPasswordTokenExpiry).HasColumnType("datetime");

            entity.HasOne(d => d.Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Departments");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_UserTypes");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserType__3214EC0760FC239C");

            entity.Property(e => e.Description).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
