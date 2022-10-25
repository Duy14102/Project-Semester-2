using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FirstAspNetApp.Models
{
    public partial class OrderDBContext : DbContext
    {
        public OrderDBContext()
        {
        }

        public OrderDBContext(DbContextOptions<OrderDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Announ> Announs { get; set; } = null!;
        public virtual DbSet<History> Histories { get; set; } = null!;
        public virtual DbSet<OrderHistory> OrderHistories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user id=vtca;password=vtcacademy;port=3306;database=OrderDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.CustomerAddress)
                    .HasMaxLength(200)
                    .HasColumnName("customer_address");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .HasColumnName("customer_name");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.Property(e => e.HistoryID).HasColumnName("history_id");

                entity.Property(e => e.HistoryName)
                    .HasMaxLength(500)
                    .HasColumnName("history_name");

                entity.Property(e => e.HistoryFullname)
                .HasMaxLength(200)
                .HasColumnName("history_fullname");

                entity.Property(e => e.HistoryAddress)
                .HasMaxLength(200)
                .HasColumnName("history_address");

                entity.Property(e => e.HistoryEmail)
                .HasMaxLength(200)
                .HasColumnName("history_email");

                entity.Property(e => e.HistoryPrice)
                    .HasPrecision(20, 2)
                    .HasColumnName("history_price")
                    .HasDefaultValueSql("'0.00'");

                entity.Property(e => e.HistoryQuantity).HasColumnName("history_quantity").HasDefaultValueSql("'1.00");

                entity.Property(e => e.HistoryPhone)
                .HasMaxLength(200)
                .HasColumnName("history_phone");

                entity.Property(e => e.HistoryOrderId).HasColumnName("history_orderid");

                entity.HasOne(d => d.OrderHistory)
                    .WithMany(p => p.Histories)
                    .HasForeignKey(d => d.HistoryOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_histories_orderhistories");
            });

            modelBuilder.Entity<OrderHistory>(entity =>
            {
                entity.Property(e => e.OrderHistoryID).HasColumnName("orderh_id");


                entity.Property(e => e.OrderHistoryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("orderh_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.OrderHistoryUser)
                    .HasMaxLength(500)
                    .HasColumnName("orderh_user");

                entity.Property(e => e.OrderHistoryStatus).HasColumnName("orderh_status").HasDefaultValueSql("'1.00'"); ;
            });

            modelBuilder.Entity<Announ>(entity =>
            {
                entity.Property(e => e.AnnounId).HasColumnName("announ_id");

                entity.Property(e => e.AnnounName)
                    .HasMaxLength(500)
                    .HasColumnName("announ_name");

                entity.Property(e => e.AnnounStory)
                    .HasMaxLength(500)
                    .HasColumnName("announ_story");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("user_id");
                
                entity.Property(e => e.Phone)
                    .HasMaxLength(200)
                    .HasColumnName("user_phone");

                entity.Property(e => e.UserName)
                    .HasMaxLength(500)
                    .HasColumnName("user_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .HasColumnName("user_password");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(500)
                    .HasColumnName("user_fullname");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .HasColumnName("user_email");

                entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("user_address");

                entity.Property(e => e.Role).HasColumnName("user_role").HasDefaultValue(2);

                entity.Property(e => e.UserDate)
                    .HasColumnType("datetime")
                    .HasColumnName("user_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Image)
                    .HasMaxLength(500)
                    .HasColumnName("user_image");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Category)
                    .HasMaxLength(500)
                    .HasColumnName("category");

                entity.Property(e => e.ItemStory)
                .HasMaxLength(500)
                .HasColumnName("item_story");

                entity.Property(e => e.ItemDescription)
                    .HasMaxLength(500)
                    .HasColumnName("item_description");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(200)
                    .HasColumnName("item_name");

                entity.Property(e => e.ItemStatus).HasColumnName("item_status");

                entity.Property(e => e.UnitPrice)
                    .HasPrecision(20, 2)
                    .HasColumnName("unit_price")
                    .HasDefaultValueSql("'0.00'");
            });


            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");

                entity.Property(e => e.FeedbackName)
                    .HasMaxLength(500)
                    .HasColumnName("feedback_name");

                entity.Property(e => e.FeedbackStory)
                .HasMaxLength(500)
                .HasColumnName("feedback_story");

                entity.Property(e => e.FeedbackLink)
                .HasMaxLength(500)
                .HasColumnName("feedback_link");

                entity.Property(e => e.FeedbackDate)
                    .HasColumnType("datetime")
                    .HasColumnName("feedback_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.CustomerId, "fk_Orders_Customers");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.OrderStatus).HasColumnName("order_status");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("fk_Orders_Customers");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ItemId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasIndex(e => e.ItemId, "fk_OrderDetails_Items");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UnitPrice)
                    .HasPrecision(20, 2)
                    .HasColumnName("unit_price");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_OrderDetails_Orders");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
