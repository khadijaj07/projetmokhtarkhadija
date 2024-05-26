using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace electro.Models.Repositories
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly AppDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CommandeRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
            
        public Commande Add(Commande commande)
        {
            context.Commandes.Add(commande);
            context.SaveChanges();
            return commande;
        }

        public void AddToCart(int articleId)
        {
            // Implementation to add the article to the cart
            var article = context.Articles.Find(articleId);
            if (article != null)
            {
                // Logic to add article to the user's cart
            }
        }
        public Commande Delete(int id)
        {
            var commande = context.Commandes.Find(id);
            if (commande != null)
            {
                context.Commandes.Remove(commande);
                context.SaveChanges();
            }
            return commande;
        }

        public Commande Delete(Commande t)
        {
            throw new NotImplementedException();
        }

        public Commande Get(int id)
        {
            return context.Commandes.Find(id);
        }

        public IEnumerable<Commande> GetAll()
        {
            return  context.Commandes
                           .Include(c => c.Utilisateur) 
                           .Include(c => c.Articles)    
                           .Where(c => c.Etat == true)
                           .ToList();
        }

        public async Task<IEnumerable<Commande>> GetOrdersByUserAsync(string userId)
        {
            return await context.Commandes.Where(c => c.UtilisateurId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetUnpaidOrdersAsync()
        {
            return await context.Commandes.Where(c => c.Etat != true).ToListAsync();
        }

        public Commande Update(Commande commande)
        {
            var commandeToUpdate = context.Commandes.Attach(commande);
            commandeToUpdate.State = EntityState.Modified;
            context.SaveChanges();
            return commande;
        }
        public List<Commande> Search(string term)
        {
            if (!string.IsNullOrEmpty(term))
                return context.Commandes.Where(p => p.UtilisateurId.Contains(term)).ToList();
            else
                return context.Commandes.ToList();
        }

    }
}
