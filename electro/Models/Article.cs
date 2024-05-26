using System.ComponentModel.DataAnnotations;

namespace electro.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Désignation { get; set; }


        [Required]
        [Display(Name = "Prix en dinar :")]
        public float Prix { get; set; }

        [Required]
        [Display(Name = "Quantité en unité :")]
        public int Quantite { get; set; }

        [Required]

        [Display(Name = "Image")]
        public string Image { get; set; }
        public int ChiffreAffaire { get; set; }
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }
    }
}
