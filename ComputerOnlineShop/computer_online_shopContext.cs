using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IO;

namespace ComputerOnlineShop
{
    public partial class computer_online_shopContext : DbContext
    {
        public computer_online_shopContext()
        {
        }

        public computer_online_shopContext(DbContextOptions<computer_online_shopContext> options)
            : base(options)
        {
        }

        public static DbContextOptions<computer_online_shopContext> GetOptions()
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<computer_online_shopContext>();
            var options = optionsBuilder
                .UseNpgsql(connectionString)
                .Options;

            return options;
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ClientProduct> ClientProducts { get; set; } = null!;
        public virtual DbSet<DataType> DataTypes { get; set; } = null!;
        public virtual DbSet<MeasurementUnit> MeasurementUnits { get; set; } = null!;
        public virtual DbSet<Moderator> Moderators { get; set; } = null!;
        public virtual DbSet<Operator> Operators { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<PointsOfIssue> PointsOfIssues { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductsInOrder> ProductsInOrders { get; set; } = null!;
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<PropertyType> PropertyTypes { get; set; } = null!;
        public virtual DbSet<Refund> Refunds { get; set; } = null!;
        public virtual DbSet<Subcategory> Subcategories { get; set; } = null!;
        public virtual DbSet<WaysToReceive> WaysToReceives { get; set; } = null!;

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(DBConnectInfo.ConnectString);
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100)
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Balance)
                    .HasColumnType("money")
                    .HasColumnName("balance")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.BonusCount)
                    .HasColumnType("money")
                    .HasColumnName("bonus_count")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("clients_to_person");
            });

            modelBuilder.Entity<ClientProduct>(entity =>
            {
                entity.ToTable("client_products");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientProducts)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("clients");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ClientProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("products");
            });

            modelBuilder.Entity<DataType>(entity =>
            {
                entity.ToTable("data_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataTypeName)
                    .HasMaxLength(10)
                    .HasColumnName("data_type_name");
            });

            modelBuilder.Entity<MeasurementUnit>(entity =>
            {
                entity.ToTable("measurement_units");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MeasurementUnitName)
                    .HasMaxLength(25)
                    .HasColumnName("measurement_unit_name");
            });

            modelBuilder.Entity<Moderator>(entity =>
            {
                entity.ToTable("moderators");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Moderators)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("moderators_to_person");
            });

            modelBuilder.Entity<Operator>(entity =>
            {
                entity.ToTable("operators");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Operators)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("operators_to_person");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasIndex(e => e.OrderNumber, "orders_order_number_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActualDateOfReceipt).HasColumnName("actual_date_of_receipt");

                entity.Property(e => e.ActualReceiptTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("actual_receipt_time");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(255)
                    .HasColumnName("contact_email");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(255)
                    .HasColumnName("contact_name");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(32)
                    .HasColumnName("contact_phone");

                entity.Property(e => e.DateOfReceipt).HasColumnName("date_of_receipt");

                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(255)
                    .HasColumnName("delivery_address");

                entity.Property(e => e.OrderNumber).HasColumnName("order_number");

                entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");

                entity.Property(e => e.PaymentMethodId).HasColumnName("payment_method_id");

                entity.Property(e => e.PointOfIssueId).HasColumnName("point_of_issue_id");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.ReceiptTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("receipt_time");

                entity.Property(e => e.RegDate).HasColumnName("reg_date");

                entity.Property(e => e.RegTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("reg_time");

                entity.Property(e => e.WayToReceiveId).HasColumnName("way_to_receive_id");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orders_to_clients");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orders_to_order_statuses");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orders_to_payment_methods");

                entity.HasOne(d => d.PointOfIssue)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PointOfIssueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orders_to_points_of_issue");

                entity.HasOne(d => d.WayToReceive)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.WayToReceiveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orders_to_ways_to_receive");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("order_statuses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderStatusName)
                    .HasMaxLength(255)
                    .HasColumnName("order_status_name");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.ToTable("payment_methods");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PaymentMethodName)
                    .HasMaxLength(32)
                    .HasColumnName("payment_method_name");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .HasColumnName("last_name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(32)
                    .HasColumnName("phone");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(255)
                    .HasColumnName("user_password");
            });

            modelBuilder.Entity<PointsOfIssue>(entity =>
            {
                entity.ToTable("points_of_issue");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.WorkTimeEnd).HasColumnName("work_time_end");

                entity.Property(e => e.WorkTimeStart).HasColumnName("work_time_start");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdditionalBonusCount)
                    .HasColumnType("money")
                    .HasColumnName("additional_bonus_count")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ImagePath).HasColumnName("image_path");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.ProductDesc).HasColumnName("product_desc");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .HasColumnName("product_name");

                entity.Property(e => e.QuantityInStock).HasColumnName("quantity_in_stock");

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SubcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("products_to_subcategories");
            });

            modelBuilder.Entity<ProductsInOrder>(entity =>
            {
                entity.ToTable("products_in_orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductsInOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("products_in_orders_to_orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsInOrders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("products_in_orders_to_products");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("properties");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.PropertyTypeId).HasColumnName("property_type_id");

                entity.Property(e => e.PropertyValue)
                    .HasMaxLength(255)
                    .HasColumnName("property_value");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("properties_to_products");

                entity.HasOne(d => d.PropertyType)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.PropertyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("properties_to_property_types");
            });

            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.ToTable("property_types");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataTypeId).HasColumnName("data_type_id");

                entity.Property(e => e.MeasurementUnitId).HasColumnName("measurement_unit_id");

                entity.Property(e => e.PropertyName)
                    .HasMaxLength(100)
                    .HasColumnName("property_name");

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.HasOne(d => d.DataType)
                    .WithMany(p => p.PropertyTypes)
                    .HasForeignKey(d => d.DataTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("property_types_to_data_types");

                entity.HasOne(d => d.MeasurementUnit)
                    .WithMany(p => p.PropertyTypes)
                    .HasForeignKey(d => d.MeasurementUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("property_types_to_measurement_units");

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.PropertyTypes)
                    .HasForeignKey(d => d.SubcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("property_types_to_subcategories");
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                entity.ToTable("refunds");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateOfRefund).HasColumnName("date_of_refund");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ReasonForRefund).HasColumnName("reason_for_refund");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Refunds)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("refunds_to_orders");
            });

            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.ToTable("subcategories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.SubcategoryName)
                    .HasMaxLength(100)
                    .HasColumnName("subcategory_name");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Subcategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("subcategories_to_categories");
            });

            modelBuilder.Entity<WaysToReceive>(entity =>
            {
                entity.ToTable("ways_to_receive");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.WayToReceiveName)
                    .HasMaxLength(32)
                    .HasColumnName("way_to_receive_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
