namespace electro.Models.Repositories
{
    // Interface de repository pour les commandes
    public interface ICommandeRepository : IRepository<Commande>
    {
        Task<IEnumerable<Commande>> GetUnpaidOrdersAsync();
        Task<IEnumerable<Commande>> GetOrdersByUserAsync(string userId); 


        public List<Commande> Search(string term);
        void AddToCart(int articleId);
    }

}
