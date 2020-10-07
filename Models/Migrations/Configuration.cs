namespace Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MobileShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MobileShopContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var administator = context.Users.Any(u => u.IsAdmin == true);
            if (administator == false)
            {
                context.Users.Add(new Models.DataModels.User { UserName = "Admin", FullName = "Hoang Truong", Email = "hqtruong27@gmail.com", Password = "$2a$10$taMaQNurgzOppm.X6vgtYO8xjtFGunf9wg.QD57WO02j4PcMMOJVa", Phone = "0963712001", Avatar = "", IsAdmin = true, Status = 1, mStatus = 1, isEmailVerified = true, ActiveCode = new Guid(), ResetPasswordCode = "", GroupId = null });
            }
            context.Roles.AddOrUpdate(new Models.DataModels.Role { RoleId = "VIEW", RoleName = "VIEW", Status = 1 });
            context.Roles.AddOrUpdate(new Models.DataModels.Role { RoleId = "CREATE", RoleName = "CREATE", Status = 1 });
            context.Roles.AddOrUpdate(new Models.DataModels.Role { RoleId = "UPDATE", RoleName = "UPDATE", Status = 1 });
            context.Roles.AddOrUpdate(new Models.DataModels.Role { RoleId = "DELETE", RoleName = "DELETE", Status = 1 });
            context.SaveChanges();
        }
    }
}
