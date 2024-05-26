using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace electro.Models.ViewModels
{
    public class EditViewModel : CreateViewModel
    {
        public int Id { get; set; }
        public string ExistingImagePath { get; set; }
    }
}


