namespace electro.Models
{
    public class Admin
    {
        public int Id { get; set; } 
        public string NomUtilisateur { get; set; } 
        public string MotDePasse { get; set; } 

        // Relation avec les articles
        public ICollection<Article> Articles { get; set; } 
        public ICollection<Categorie> Categories { get; set; } 
    }
}
