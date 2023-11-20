using Clothings_Store.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Clothings_Store.Data
{
    public class StoreContext : IdentityDbContext<AppUser>
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Color> Colors { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<JobTitle> JobTitles { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

        public virtual DbSet<Payment> Payments { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Promotion> Promotions { get; set; }

        public virtual DbSet<Size> Sizes { get; set; }

        public virtual DbSet<Staff> Staff { get; set; }

        public virtual DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Code).HasMaxLength(10);
                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Colors__3214EC074B6E2D47");

                entity.Property(e => e.Ghichu).HasMaxLength(50);
                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0739C90AAD");

                entity.ToTable("Customer");

                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FullName).HasMaxLength(50);
                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.ToTable("Job_title");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC076065A524");

                entity.ToTable("Order");

                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.DeliTime).HasColumnType("date");
                entity.Property(e => e.Note).HasMaxLength(200);
                entity.Property(e => e.OrdTime).HasColumnType("date");
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Order__CustomerI__30C33EC3");

                entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__Order__PaymentId__32AB8735");

                entity.HasOne(d => d.Staff).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK__Order__StaffId__31B762FC");

                entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK__Order__Status__2FCF1A8A");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.StockId }).HasName("PK__tmp_ms_x__F1586153AC1F7631");

                entity.ToTable("OrderDetail");

                entity.Property(e => e.UnitPrice).HasColumnName("unitPrice");

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Order__2EDAF651");

                entity.HasOne(d => d.Stock).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderDeta__Stock__2180FB33");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.Status).HasName("PK__tmp_ms_x__3A15923EEC714ED5");

                entity.ToTable("OrderStatus");

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC07CB929B86");

                entity.ToTable("Payment");

                entity.Property(e => e.Ghichu).HasMaxLength(200);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.CostPrice).HasColumnName("costPrice");
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.StockInDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("date")
                    .HasColumnName("stockInDate");
                entity.Property(e => e.UnitPrice).HasColumnName("unitPrice");

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Product__Categor__267ABA7A");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__2CB9556BBEE408D1");

                entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");
                entity.Property(e => e.DiscountPercentage)
                    .HasColumnType("decimal(5, 2)")
                    .HasColumnName("discount_percentage");
                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");
                entity.Property(e => e.PromotionName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("promotion_name");
                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Sizes__3214EC07D711A6F5");

                entity.Property(e => e.Ghichu).HasMaxLength(50);
                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Staff__3214EC07D20549E0");

                entity.ToTable(tb => tb.HasTrigger("ASP_AbortRoleAdmin"));

                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.Cmt)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CMT");
                entity.Property(e => e.DateOfBirth).HasColumnType("date");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FullName).HasMaxLength(50);
                entity.Property(e => e.JobTitle).HasColumnName("Job_title");
                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.HasOne(d => d.JobTitleNavigation).WithMany(p => p.Staff)
                    .HasForeignKey(d => d.JobTitle)
                    .HasConstraintName("FK_STAFF");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Stock__3214EC078808D165");

                entity.ToTable("Stock");

                entity.Property(e => e.Stock1).HasColumnName("Stock");
                entity.Property(e => e.StockInDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("date")
                    .HasColumnName("stockInDate");

                entity.HasOne(d => d.Color).WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.ColorId)
                    .HasConstraintName("FK__Stock__ColorId__19DFD96B");

                entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Stock__ProductId__18EBB532");

                entity.HasOne(d => d.Size).WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.SizeId)
                    .HasConstraintName("FK__Stock__SizeId__1AD3FDA4");
            });

            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

        }

    }
}
