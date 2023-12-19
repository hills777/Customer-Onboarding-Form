using Customer_Onboarding_Form.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Customer_Onboarding_Form.Context
{

    public class CustomerContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserModel> Customers { get; set; }
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {

        }

    }


}