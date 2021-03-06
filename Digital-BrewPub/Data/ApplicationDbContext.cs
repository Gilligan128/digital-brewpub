﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Digital.BrewPub.Features.Shared;
using Digital.BrewPub.Features.Note;

namespace Digital.BrewPub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Note>().HasKey(x => x.Id);
            builder.Entity<Note>().Property(x => x.AuthorId).IsRequired();
            builder.Entity<Note>().Property(x => x.Brewery).IsRequired().HasMaxLength(50);
            builder.Entity<Note>().HasIndex(n => n.Brewery);

        }


        public DbSet<Note> Notes
        {
            get
            {
                return this.Set<Note>();
            } 
        }
    }
}
