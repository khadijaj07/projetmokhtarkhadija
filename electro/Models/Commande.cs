    using System.ComponentModel.DataAnnotations;

    namespace electro.Models
    {
        public class Commande
        {
            public int Id { get; set; }
        
            public string DateCommande { get; set; } 
            public bool Etat { get; set; }  

            public String UtilisateurId { get; set; }
            public Utilisateur Utilisateur { get; set; }

            public float somme { get; set; }
     
            public ICollection<Article> Articles { get; set; }

    }
    }
