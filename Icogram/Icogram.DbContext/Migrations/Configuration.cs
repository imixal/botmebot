using Icogram.Enums;
using Icogram.Models.CompanyModels;
using Icogram.Models.UserModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Icogram.DbContext.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IcogramDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Icogram.DbContext.IcogramDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var roleAdmin = new IdentityRole { Name = "Admin" };
                manager.Create(roleAdmin);
            }
            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var roleManager = new IdentityRole { Name = "Manager" };
                manager.Create(roleManager);
            }

            if (!context.Roles.Any(r => r.Name == "Customer"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var roleCustomer = new IdentityRole { Name = "Customer" };
                manager.Create(roleCustomer);
            }

            if (!context.Companies.Any(c => c.Name == "Icogram"))
            {
                var company = new Company
                {
                    Name = "ManageCompany",
                    Type = GlobalEnums.CompanyType.ManagementCompany
                };
                context.Companies.Add(company);
            }

            if (!context.Users.Any(u => u.UserName == "imixal"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "imixal" };
                if (context.Companies.Any(c => c.Name == "ManageCompany"))
                {
                    user.CompanyId = 1;
                }

                manager.Create(user, "ilya9511");
                manager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "adminNikita"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "adminNikita" };
                if (context.Companies.Any(c => c.Name == "ManageCompany"))
                {
                    user.CompanyId = 1;
                }

                manager.Create(user, "12345671");
                manager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
