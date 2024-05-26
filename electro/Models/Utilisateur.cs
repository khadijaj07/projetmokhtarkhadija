using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

namespace electro.Models
{
	public class Utilisateur
	{
		public String Id { get; set; }

		[Required]
		[Display(Name = "Nom :")]
		public String Nom { get; set; }

		[Required]
		[Display(Name = "Prenom :")]
		public String Prenom { get; set; }

		[Required]
		[Display(Name = "Adresse :")]

		public String Adresse { get; set; }

		[Required]
		[Display(Name = "Email :")]

		public String Email { get; set; }

		[Required]
		[Display(Name = "Mot de passe :")]


		public string PasswordHash { get; set; }




		public ICollection<Commande> Commandes { get; set; }

	}
}