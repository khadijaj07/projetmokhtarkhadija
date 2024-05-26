using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


    namespace electro.Models
    {
        public class AppDbContext : IdentityDbContext
	{
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            public DbSet<Article> Articles { get; set; } 
            public DbSet<Categorie> Categories { get; set; } 
            public DbSet<Commande> Commandes { get; set; }
            public DbSet<Utilisateur> Utilisateurs { get; set; }
            public DbSet<Admin> Admin { get; set; }

        }
    }
