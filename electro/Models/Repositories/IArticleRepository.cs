    namespace electro.Models.Repositories
    {
        // Interface de repository pour les articles
        public interface IArticleRepository : IRepository<Article>
        {
            Task<IEnumerable<Article>> GetByCategoryAsync(int categoryId); 
            Task<IEnumerable<Article>> GetFilteredArticlesAsync(string searchString);
            Task<Article> AddToCartAsync(Article article, string userId);
            Task<IEnumerable<Article>> Search(string name);
        Article bestArticles();
    }

    }
