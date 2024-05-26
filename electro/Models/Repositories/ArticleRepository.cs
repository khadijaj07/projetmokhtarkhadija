    using Microsoft.EntityFrameworkCore;

    namespace electro.Models.Repositories
    {
        public class ArticleRepository : IArticleRepository
        {
            private readonly AppDbContext context;

            public ArticleRepository(AppDbContext context)
            {
                this.context = context;
            }

        public void Addd(Article article)
            {
                context.Add(article);
                context.SaveChanges();
            }

            public Article Add(Article t)
            {
                context.Articles.Add(t);
                context.SaveChanges();
                return t;

            }

            public async Task<Article> AddToCartAsync(Article article, string userId)
            {
                var user = await context.Utilisateurs.FindAsync(userId);
                if(user == null)
            
                    return null;
                user.Commandes ??= new List<Commande>();
                var commande = new Commande
                {
                    UtilisateurId = userId,
                    DateCommande = "Date",
                    Etat = false,

                };
                commande.Articles.Add(article);
                context.Commandes.Add(commande);
                await context.SaveChangesAsync();

                return article;
            

            }

        public Article bestArticles()
        {
            return context.Articles
                .Include(a => a.Categorie)
                .OrderByDescending(a => a.ChiffreAffaire)
                .FirstOrDefault();
        }
        public Article Delete(int Id)
            {
                var article = context.Articles.Find(Id);
                if(article != null)
                {
                    context.Articles.Remove(article);
                    context.SaveChanges();

                }
                return article;
            }

            public Article Delete(Article t)
            {
                throw new NotImplementedException();
            }

            public Article Get(int Id)
            {
                return context.Articles.Find(Id);
            }

            public IEnumerable<Article> GetAll()
            {
               return context.Articles.ToList();
            }

            public async Task<IEnumerable<Article>> GetByCategoryAsync(int categoryId)
            {
               return await context.Articles.Where(a => a.CategorieId == categoryId).ToListAsync();
            }

            public async Task<IEnumerable<Article>> GetFilteredArticlesAsync(string searchString)
            {
                return await context.Articles.Where(a => a.Désignation.Contains(searchString)).ToListAsync(); 
            }

       
            public Article Update(Article article)
            {
                Article c1 = context.Articles.Find(article.Id);
                if (c1 == null)
                {
                    context.Articles.Add(article);
                }
                else
                {
                    c1.Désignation = article.Désignation;
                    c1.Quantite = article.Quantite;
                    c1.Prix = article.Prix;
                    c1.CategorieId = article.CategorieId;
                }

                context.SaveChanges();

                return article;
            }


        public async Task<IEnumerable<Article>> GetMostSoldArticlesAsync(int topCount)
        {
            var mostSoldArticleIds = await context.Commandes
                .SelectMany(c => c.Articles.Select(a => a.Id))
                .GroupBy(id => id)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(topCount)
                .ToListAsync();

            return await context.Articles
                .Where(a => mostSoldArticleIds.Contains(a.Id))
                .Include(a => a.Categorie)
                .ToListAsync();
        }


        public async Task<IEnumerable<Article>> Search(string name)
        {
            return await context.Articles
                .Where(a => a.Désignation.Contains(name))
                .ToListAsync();
        }
    }

}

