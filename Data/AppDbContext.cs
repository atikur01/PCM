﻿using Microsoft.EntityFrameworkCore;
using PCM.Models;

namespace PCM.Data
{
    public class AppDbContext : DbContext
    {
       
        public DbSet<User> Users { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Item> Items { get; set; }


 

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }




    }
}

