using electro.Models;
using electro.Models.Repositories;
using electro.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;


namespace electro.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        private readonly AppDbContext context;
        private readonly ICommandeRepository commandeRepository;
        private readonly IArticleRepository articleRepository;
        private readonly IRepository<Categorie> categoryRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public AdminController(AppDbContext context, ICommandeRepository commandeRepository, IArticleRepository articleRepository, IRepository<Categorie> categoryRepository, IWebHostEnvironment hostingEnvironment)
        {
            this.context = context;
            this.commandeRepository = commandeRepository;
            this.articleRepository = articleRepository;
            this.categoryRepository = categoryRepository;
            this.hostingEnvironment = hostingEnvironment;
        }



        public IActionResult Index()
        {
            return View();
        }


        // GET: AdminController
        public IActionResult listecategories()
        {
            var categories = categoryRepository.GetAll();
            return View(categories);
        }


        // GET:  AdminController/ajoutercategorie
        public IActionResult ajoutercategorie()
        {
            return View();
        }

        // POST: AdminController/ajoutercategorie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ajoutercategorie(Categorie c)
        {
            try
            {
                categoryRepository.Add(c);
                return RedirectToAction(nameof(listecategories));
            }
            catch
            {
                return View();
            }

        }
        // GET: AdminController/Deletecategorie/50
        [HttpGet]
        public ActionResult Deletecategorie(int id)
        {
            var category = categoryRepository.Get(id);
            if (category == null)
            {
                return NotFound(); 
            }
            return View(category);
        }

        


        // POST: AdminController/Deletecategorie/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = categoryRepository.Get(id);
                if (category == null)
                {
                    return NotFound();
                }

                categoryRepository.Delete(category);
                return RedirectToAction(nameof(listecategories));
            }
            catch
            {
                return View();
            }
        }


        public IActionResult BestArticle()
        {
            var bestArticle = articleRepository.bestArticles();
            return View(bestArticle);
        }   



        [HttpGet]
        public IActionResult Editcat(int id)
        {
            var existingCategory = context.Categories.Find(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            return View(existingCategory);
        }
        [HttpPost]
        public IActionResult Editcat(Categorie t)
        {


            try
            {
                var existingCategory = context.Categories.Find(t.Id);
                if (existingCategory == null)
                {
                    return NotFound($"Category with ID {t.Id} not found.");
                }

                existingCategory.Nom = t.Nom;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        public IActionResult listeproduit(string nom)
        {
            IEnumerable<Article> articles;

            if (!string.IsNullOrEmpty(nom))
            {
                articles = articleRepository.Search(nom).Result;
            }
            else
            {
                articles = articleRepository.GetAll();
            }

            return View(articles);
        }
        // GET: ArticleController/Create
        // GET: ArticleController/Create
        public ActionResult Createproduit()
        {
            ViewBag.CategorieId = new SelectList(categoryRepository.GetAll(), "Id", "Nom");
            var viewModel = new CreateViewModel();
            return View(viewModel);
        }

        // POST: ArticleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createproduit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.ImagePath != null)
                {

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImagePath.CopyTo(fileStream);
                    }
                }


                Article newArticle = new Article
                {
                    Désignation = model.Designation,
                    Prix = model.Prix,
                    Quantite = model.Quantite,
                    Image = uniqueFileName,
                    CategorieId = model.CategorieId
                };

                articleRepository.Add(newArticle);

                return RedirectToAction("listeproduit");
            }

            ViewBag.CategorieId = new SelectList(categoryRepository.GetAll(), "Id", "Nom", model.CategorieId);
            return View(model);
        }


        // GET: ArticleController/EditProduit/5
        public ActionResult Editproduit(int id)
        {
            ViewBag.CategorieId = new SelectList(categoryRepository.GetAll(), "CategorieId", "Nom");
            Article article = articleRepository.Get(id);
            EditViewModel articleEditViewModel = new EditViewModel
            {
                Id = article.Id,
                Designation = article.Désignation,
                Prix = article.Prix,
                Quantite = article.Quantite,
                CategorieId = article.CategorieId,
                ExistingImagePath = article.Image,

            };
            return View(articleEditViewModel);

        }

        // POST: ArticleController/EditProduit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduit(EditViewModel model)
        {
       
            if (ModelState.IsValid)
            {
                Article article = articleRepository.Get(model.Id);
                article.Désignation = model.Designation;
                article.Prix = model.Prix;
                article.Quantite = model.Quantite;
                article.CategorieId = model.CategorieId;
                if (model.ImagePath != null)
                {
                    if (model.ExistingImagePath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingImagePath);
                        System.IO.File.Delete(filePath);
                    }
                    article.Image = ProcessUploadedFile(model);
                }
                Article updatedArticle = articleRepository.Update(article);
                ViewBag.CategorieId = new SelectList(categoryRepository.GetAll(), "CategorieId", "Nom");
                if (updatedArticle != null)
                    return RedirectToAction("listeproduit");
                else
                    return NotFound();

            }
            return View(model);
        }

        [NonAction]
        private string ProcessUploadedFile(EditViewModel model)
        {
            string uniqueFileName = null;
            if (model.ImagePath != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }







        // GET: AdminController/Deleteproduit/5

        [HttpGet]
        public ActionResult Deleteproduit(int id)
        {
            var article = articleRepository.Get(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }


   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteproduitConfirmed(int id)
    {
        try
        {
            var article = articleRepository.Get(id);
            if (article == null)
            {
                return NotFound();
            }

            articleRepository.Delete(id);
            return RedirectToAction(nameof(listeproduit));
        }
        catch
        {
            return View();
        }
    }



        public IActionResult listecommande(string userId)
        {
            // Retrieve all users along with their IDs
            var users = commandeRepository.GetAll().Select(c => new { Id = c.UtilisateurId, Email = c.Utilisateur.Email }).Distinct();

            // Create a SelectList for the dropdown list
            ViewBag.Users = new SelectList(users, "Id", "Email");

            IEnumerable<Commande> commandes;

            if (!string.IsNullOrEmpty(userId))
            {
                // Filter orders by user ID
                commandes = commandeRepository.GetAll().Where(c => c.UtilisateurId == userId);
            }
            else
            {
                // If no user ID is provided, return all orders
                commandes = commandeRepository.GetAll();
            }

            return View(commandes);
        }




        public IActionResult ClientsFideles()
        {
            var usersWithOrders = commandeRepository.GetAll()
                .Where(c => c.Etat)
                .Select(c => c.Utilisateur)
                .Distinct();


            var usersWithTotalAmount = usersWithOrders.Select(user => new
            {
                User = user,
                TotalAmount = user.Commandes.Where(c => c.Etat).Sum(c => c.somme)
            });

            var sortedUsers = usersWithTotalAmount.OrderByDescending(u => u.TotalAmount);

            return View(sortedUsers);
        }




    }
}
