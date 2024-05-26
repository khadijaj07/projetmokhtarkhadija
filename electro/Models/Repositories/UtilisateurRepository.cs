using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace electro.Models.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly AppDbContext context;
        private readonly IPasswordHasher<Utilisateur> passwordHasher;
        private object logger;
       
        public UtilisateurRepository(AppDbContext context, IPasswordHasher<Utilisateur> passwordHasher )
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
        
            }

        public async Task<IdentityResult> Register(Utilisateur user, string password)
        {
            try
            {
                user.Id = Guid.NewGuid().ToString();
                user.PasswordHash = passwordHasher.HashPassword(user, password);
                context.Utilisateurs.Add(user);
                await context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Log the exception details (e.g., using a logging framework)
                // logger.LogError(ex, "Error registering user");

                // Re-throw or handle the exception appropriately
                throw;
            }

        }



            public async Task<bool> VerifyEmailAndPasswordAsync(string email, string password)
        {
            var utilisateur = await context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);

            if (utilisateur == null)
                return false;

            // Vérification du mot de passe
            var result = passwordHasher.VerifyHashedPassword(utilisateur, utilisateur.PasswordHash, password);

            return result == PasswordVerificationResult.Success;
        }
        // Méthode pour vérifier le hachage du mot de passe
    



        



            public Utilisateur Get(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Utilisateur> GetAll()
        {
            throw new NotImplementedException();
        }

        public Utilisateur Add(Utilisateur t)
        {
            throw new NotImplementedException();
        }

        public Utilisateur Update(Utilisateur t)
        {
            throw new NotImplementedException();
        }

        public Utilisateur Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public Utilisateur Delete(Utilisateur t)
        {
            throw new NotImplementedException();
        }

     
     
    }
}
