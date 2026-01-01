using Martaarcadu.Domain.Entities.ApplicationUser;
using Martaarcadu.Domain.Entities.Service;
using Martaarcadu.Domain.Entities.SubscriptionPlan;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
    
        public DbSet<SubscriptionsPlan> SubscriptionPlans { get; set; }
        public DbSet<SubscriptionPlanBenefit> SubscriptionPlanBenefits { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SubscriptionsPlan>()
                    .Property(p => p.Duration)
                    .HasConversion<string>();
        }
    }
}