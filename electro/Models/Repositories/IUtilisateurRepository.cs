using Microsoft.AspNetCore.Identity;

namespace electro.Models.Repositories
{
    public interface IUtilisateurRepository : IRepository<Utilisateur>
    {
      
        Task<IdentityResult> Register(Utilisateur user, string password);


        Task<bool> VerifyEmailAndPasswordAsync(string email, string password);
    }
}

