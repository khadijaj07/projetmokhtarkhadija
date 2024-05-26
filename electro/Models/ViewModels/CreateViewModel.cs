using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace electro.Models.ViewModels
{
    public class CreateViewModel
    {
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "Le champ Designation est requis.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "La désignation doit avoir entre 5 et 50 caractères.")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Le champ Prix est requis.")]
        [Display(Name = "Prix en dinar :")]
        public float Prix { get; set; }

        [Required(ErrorMessage = "Le champ Quantité est requis.")]
        [Display(Name = "Quantité en unité :")]
        public int Quantite { get; set; }

        [Display(Name = "Image :")]
        public IFormFile ImagePath { get; set; }

        [Required(ErrorMessage = "Le champ Catégorie est requis.")]
        [Display(Name = "Catégorie :")]
        public int CategorieId { get; set; }
    }
}
