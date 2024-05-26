using System;
using System.Collections.Generic;
using System.Linq;
using electro.Models;

namespace electro.Models.Repositories
{
    public class CategoryRepository : IRepository<Categorie>
    {
        private readonly AppDbContext context;

        public CategoryRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Categorie Add(Categorie t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            context.Categories.Add(t);
            context.SaveChanges();
            return t;
        }

        public Categorie Delete(Categorie c)
        {
            if (c == null)
                throw new ArgumentNullException(nameof(c));

            var category = context.Categories.Find(c.Id);
            if (category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
            return category;
        }

        public Categorie Delete(int Id)
        {
            var category = context.Categories.Find(Id);
            if (category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
            return category;
        }

        public Categorie Get(int Id)
        {
            return context.Categories.Find(Id);
        }

        public IEnumerable<Categorie> GetAll()
        {
            return context.Categories.ToList();
        }

        public void Update(Categorie t)
        {
           
            var Category = context.Categories.Find(t.Id);

           
            if (Category != null)
            {

                Category.Nom = t.Nom;

                context.SaveChanges();
            }

        }

        Categorie IRepository<Categorie>.Update(Categorie t)
        {
            throw new NotImplementedException();
        }
    }
}
