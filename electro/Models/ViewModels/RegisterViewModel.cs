using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace electro.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Le champ Nom est requis.")]
        [Display(Name = "Nom :")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le champ Prénom est requis.")]
        [Display(Name = "Prénom :")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le champ Adresse est requis.")]
        [Display(Name = "Adresse :")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Le champ Email est requis.")]
        [Display(Name = "Email :")]
        [EmailAddress(ErrorMessage = "Veuillez entrer une adresse email valide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le champ Mot de passe est requis.")]
        [Display(Name = "Mot de passe :")]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
