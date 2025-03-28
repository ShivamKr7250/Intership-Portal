﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Internship_Portal.Model;
using Microsoft.AspNetCore.Identity;

namespace Internship_Portal.Data_Access.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<RegistrationForm> RegistrationForms { get; set; }
        public DbSet<InternshipSubmit> SubmitedInternship { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<BlogPost> Blogs { get; set; }
        public DbSet<BlogComment> BlogsComment { get; set; }
        public DbSet<Interaction> Interaction { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Student> StudentsData { get; set; }
        public DbSet<AppliedDrive> AppliedDrive { get; set; }
        public DbSet<MentorAllocation> MentorAllocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogCategory>().HasData(
               new BlogCategory { CategoryId = 1, Name = "IT" },
               new BlogCategory { CategoryId = 2, Name = "Software" },
               new BlogCategory { CategoryId = 3, Name = "Web Development" },
               new BlogCategory { CategoryId = 4, Name = "App Development" },
               new BlogCategory { CategoryId = 5, Name = "AI & ML" },
               new BlogCategory { CategoryId = 6, Name = "New Tech" },
               new BlogCategory { CategoryId = 7, Name = "Blockchain" },
               new BlogCategory { CategoryId = 8, Name = "Frontend Development" },
               new BlogCategory { CategoryId = 9, Name = "Backend Development" }
               );

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogPost>()
                .HasKey(p => p.PostId);

            modelBuilder.Entity<BlogPost>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<BlogComment>()
                .HasKey(c => c.CommentId);

            modelBuilder.Entity<BlogComment>()
                .HasOne(c => c.BlogPost)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<BlogComment>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Interaction>()
                .HasKey(i => i.InteractionId);

            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.BlogPost)
                .WithMany(p => p.Interactions)
                .HasForeignKey(i => i.PostId);

            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.ApplicationUser)
                .WithMany(u => u.Interactions)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<AppliedDrive>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict); // 🔹 Prevents cascade delete on Student

            modelBuilder.Entity<AppliedDrive>()
                .HasOne(a => a.BlogPost)
                .WithMany()
                .HasForeignKey(a => a.DriveId)
                .OnDelete(DeleteBehavior.Cascade); // 🔹 Keeps cascade delete on BlogPost

            modelBuilder.Entity<MentorAllocation>()
           .HasOne(ma => ma.Mentor)
           .WithMany()
           .HasForeignKey(ma => ma.UserId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MentorAllocation>()
                .HasOne(ma => ma.Student)
                .WithMany()
                .HasForeignKey(ma => ma.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

        }


    }
}
