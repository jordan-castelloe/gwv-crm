using System;
using System.Collections.Generic;
using System.Text;
using CRM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Tag> Tag { get; set; }

    }
}
