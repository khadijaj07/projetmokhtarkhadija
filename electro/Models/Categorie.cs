using System.ComponentModel.DataAnnotations;

namespace electro.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        // Identifiant de la catégorie

        [Required]
        [Display(Name = "Nom :")]
        public string Nom { get; set; }


        // Relation avec les articles
        public ICollection<Article> Articles { get; set; }
    }
}
