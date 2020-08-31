namespace Models
{
    using Models.DataModels;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MobileShopContext : DbContext
    {
        public MobileShopContext()
            : base("name=MobileShopContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MobileShopContext, Migrations.Configuration>("MobileShopContext"));
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<TypeAttr> TypeAttrs { get; set; }
        public virtual DbSet<Models.DataModels.Attribute> Attributes { get; set; }
        public virtual DbSet<ProductAttr> ProductAttrs { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<GroupRole> GroupRoles { get; set; }
        public virtual DbSet<AddToCart> AddToCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }


        //self category
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOptional(x => x.category)
                .WithMany()
                .HasForeignKey(x => x.ParentId);
            base.OnModelCreating(modelBuilder);
        }
    }

}