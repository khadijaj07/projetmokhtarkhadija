using electro.Models;
using electro.Models.Repositories;
using electro.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace electro.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUtilisateurRepository utilisateurRepository;
        private readonly ICommandeRepository commandeRepository;
        private readonly IRepository<Categorie> categoryRepository;
        private readonly IArticleRepository articleRepository;

        public ClientController(AppDbContext context, ICommandeRepository commandeRepository, IArticleRepository articleRepository, IRepository<Categorie> categoryRepository, IUtilisateurRepository utilisateurRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.commandeRepository = commandeRepository;
            this.articleRepository = articleRepository;
            this.categoryRepository = categoryRepository;
            this.utilisateurRepository = utilisateurRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        // GET: Commande/Index
        public IActionResult Index()
        {
            var articles = articleRepository.GetAll();
            var categories = categoryRepository.GetAll();

            ViewBag.Articles = articles;
            ViewBag.Categories = categories;

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Utilisateur
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Adresse = model.Adresse,
                    PasswordHash = model.Password,
                    Email = model.Email
                };

                var result = await utilisateurRepository.Register(user, model.Password);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetString("Email", model.Email);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Email et mot de passe requis");
            }

            var isValid = await utilisateurRepository.VerifyEmailAndPasswordAsync(model.Email, model.Password);

            if (!isValid)
            {
                return BadRequest("Email ou mot de passe incorrect");
            }

            HttpContext.Session.SetString("Email", model.Email);

            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddToCart()
        {
            return View();
        }

    
        [HttpPost]
        public IActionResult AddToCart(int articleId)
        {
            var userEmail = httpContextAccessor.HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Error", "Home");
            }

            var user = context.Utilisateurs.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var article = context.Articles.Find(articleId);

            if (article == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var cart = context.Commandes
                              .Include(c => c.Articles)
                              .FirstOrDefault(c => c.UtilisateurId == user.Id && !c.Etat);

            if (cart == null)
            {
                cart = new Commande
                {
                    UtilisateurId = user.Id,
                    DateCommande = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Proper DateTime format
                    Etat = false,
                    Articles = new List<Article>()
                };
                context.Commandes.Add(cart);
                context.SaveChanges(); 
            }

         
            if (cart.Articles == null)
            {
                cart.Articles = new List<Article>();
            }

        
            if (!cart.Articles.Any(a => a.Id == article.Id))
            {
                cart.Articles.Add(article);
                context.SaveChanges(); 
            }

            return RedirectToAction("Cart", "Client");
        }



        public IActionResult Cart()
        {
            var userEmail = httpContextAccessor.HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Client");
            }

            var user = context.Utilisateurs.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var cart = context.Commandes
                .Include(c => c.Articles)
                .FirstOrDefault(c => c.UtilisateurId == user.Id && !c.Etat);

            if (cart == null)
            {
                
                return View("EmptyCart"); 
            }

            return View(cart);
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
          
            var commande = context.Commandes.Include(c => c.Articles).FirstOrDefault(c => c.Articles.Any(a => a.Id == id));

            if (commande != null)
            {
             
                var articleToRemove = commande.Articles.FirstOrDefault(a => a.Id == id);

                if (articleToRemove != null)
                {
                   
                    commande.Articles.Remove(articleToRemove);

                    
                    context.SaveChanges();
                }
            }

           
            return RedirectToAction("Cart", "Client");
        }


       .

        [HttpPost]
        public IActionResult Logout()
        {
           
            HttpContext.Session.Remove("Email");

  
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Search term cannot be empty.");
            }

            IEnumerable<Article> articles = await articleRepository.Search(name);

            return View(articles);
        }

        // GET: ClientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(float somme)
        {
            var userEmail = httpContextAccessor.HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Error", "Home");
            }

            var utilisateur = await context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (utilisateur != null)
            {
                try
                {
                    var commande = await context.Commandes.FirstOrDefaultAsync(c => c.UtilisateurId == utilisateur.Id && !c.Etat);

                    if (commande != null)
                    {
                        commande.DateCommande = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        commande.Etat = true;
                        commande.somme = somme;

                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        commande = new Commande
                        {
                            DateCommande = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            Etat = true,
                            UtilisateurId = utilisateur.Id,
                            somme = somme
                        };

                        context.Add(commande);
                        await context.SaveChangesAsync();
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }



    }
}
