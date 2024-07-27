using System;
using System.Collections.Generic;
using E_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Project.Data;

public partial class FullWebsiteContext : DbContext
{
    public FullWebsiteContext()
    {
    }

    public FullWebsiteContext(DbContextOptions<FullWebsiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<RecipientEmail> RecipientEmails { get; set; }

    public virtual DbSet<SendEmailList> SendEmailLists { get; set; }

    public virtual DbSet<SubscriptionDetail> SubscriptionDetails { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTemplateImage> UserTemplateImages { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__5C66259BB9D4A729");

            entity.ToTable("Contact");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Message).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF62EE36E7D");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.FeedbackDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Feedback__UserID__440B1D61");
        });

        modelBuilder.Entity<RecipientEmail>(entity =>
        {
            entity.HasKey(e => e.RecipientId).HasName("PK__Recipien__F0A601AD9D63E814");

            entity.Property(e => e.RecipientId).HasColumnName("RecipientID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RecipientEmails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Recipient__UserI__44FF419A");
        });

        modelBuilder.Entity<SendEmailList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__sendEmai__3213E83F49039344");

            entity.ToTable("sendEmailList");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.TempImageId).HasColumnName("tempImageId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.TempImage).WithMany(p => p.SendEmailLists)
                .HasForeignKey(d => d.TempImageId)
                .HasConstraintName("FK__sendEmail__tempI__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.SendEmailLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__sendEmail__userI__72C60C4A");
        });

        modelBuilder.Entity<SubscriptionDetail>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B24BD91F674A2");

            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.TemplateId).HasName("PK__Template__F87ADD07DECB9596");

            entity.Property(e => e.TemplateId).HasColumnName("TemplateID");
            entity.Property(e => e.ImagePath).IsUnicode(false);
            entity.Property(e => e.TemplateCategory)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TemplateContent).IsUnicode(false);
            entity.Property(e => e.TemplateName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B33BEC420");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Transacti__UserI__45F365D3");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC155297CA");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SubscriptionStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Inactive");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserTemplateImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__userTemp__3213E83F62E2D20F");

            entity.ToTable("userTemplateImage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserTempImage).HasColumnName("userTempImage");

            entity.HasOne(d => d.User).WithMany(p => p.UserTemplateImages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__userTempl__userI__6FE99F9F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
