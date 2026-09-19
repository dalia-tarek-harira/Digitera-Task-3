using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Task_3.Domain.Models;
using Task_3.Infrastructure.Identity;

namespace Task_3.Infrastructure.Data
{
    public class AppDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs => Set<Job>();

        public DbSet<JobApplication> JobApplications =>
            Set<JobApplication>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           // Get - ChildItemadd
            builder.Entity<Job>()
                .HasKey(x => x.Id);

            builder.Entity<Job>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Entity<Job>()
                .Property(x => x.Description)
                .IsRequired();

            builder.Entity<JobApplication>()
                .HasKey(x => x.Id);

            builder.Entity<JobApplication>()
                .HasOne(x => x.Job)
                .WithMany(x => x.Applications)
                .HasForeignKey(x => x.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobApplication>()
                .Property(x => x.Status)
                .HasConversion<string>();
        }
    }
}
