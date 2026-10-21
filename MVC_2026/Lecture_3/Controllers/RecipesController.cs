using Lecture_3.Data;
using Lecture_3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Lecture_3.Controllers;

[Authorize]
public class RecipesController : Controller
{
    private readonly ApplicationDbContext _db;
    public RecipesController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Index(string? search)
    {
        int userId = CurrentUserId();
        if (!await _db.Recipes.AnyAsync()) await AddStarterRecipes(userId);

        var query = _db.Recipes.AsNoTracking().Include(r => r.CreatedByUser)
            .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r => r.Title.Contains(search) || r.Description.Contains(search) ||
                r.RecipeIngredients.Any(ri => ri.Ingredient.Name.Contains(search)));

        var recipes = await query.OrderBy(r => r.Id).ToListAsync();
        var liked = await _db.RecipeLikes.Where(x => x.UserId == userId).Select(x => x.RecipeId).ToListAsync();
        var bookmarked = await _db.RecipeBookmarks.Where(x => x.UserId == userId).Select(x => x.RecipeId).ToListAsync();
        ViewBag.Search = search ?? string.Empty;
        return View(recipes.Select(r => new RecipeListItemViewModel {
            Id = r.Id, Title = r.Title, AuthorName = r.CreatedByUser?.FullName ?? "Recipee Community",
            Description = r.Description, ImageUrl = r.ImageUrl,
            Ingredients = r.RecipeIngredients.Select(ri => ri.Ingredient.Name).ToList(),
            IsLiked = liked.Contains(r.Id), IsBookmarked = bookmarked.Contains(r.Id)
        }).ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var recipe = await _db.Recipes.AsNoTracking().Include(r => r.CreatedByUser)
            .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient)
            .SingleOrDefaultAsync(r => r.Id == id);
        if (recipe == null) return NotFound();
        return View(recipe);
    }

    [HttpGet]
    public IActionResult Create() => View(new CreateRecipeViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRecipeViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var recipe = new Recipe
        {
            Title = model.Title.Trim(),
            Description = model.Description?.Trim() ?? string.Empty,
            ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl)
                ? "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=900&q=85"
                : model.ImageUrl.Trim(),
            MakingProcess = model.MakingProcess?.Trim() ?? string.Empty,
            VideoLink = model.VideoLink?.Trim() ?? string.Empty,
            CreatedByUserId = CurrentUserId()
        };

        var ingredientNames = (model.Ingredients ?? string.Empty).Split(new[] { '\r', '\n', ',', ';' },
            StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        
        foreach (var name in ingredientNames)
        {
            var ingredient = await _db.Ingredients.FirstOrDefaultAsync(x => x.Name == name);
            if (ingredient == null)
            {
                ingredient = new Ingredient { Name = name };
                _db.Ingredients.Add(ingredient);
                await _db.SaveChangesAsync();
            }
            recipe.RecipeIngredients.Add(new RecipeIngredient
            {
                IngredientId = ingredient.Id,
                Ingredient = ingredient,
                Recipe = recipe,
                Quantity = string.Empty
            });
        }

        _db.Recipes.Add(recipe);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = recipe.Id });
    }

    [HttpGet]
    public async Task<IActionResult> MyRecipes()
    {
        var userId = CurrentUserId();
        var ids = await _db.RecipeBookmarks.Where(x => x.UserId == userId)
            .Select(x => x.RecipeId).ToListAsync();
        var recipes = await _db.Recipes.AsNoTracking().Where(r => ids.Contains(r.Id))
            .Include(r => r.CreatedByUser)
            .Include(r => r.RecipeIngredients).ThenInclude(ri => ri.Ingredient)
            .OrderByDescending(r => r.Id).ToListAsync();
        ViewBag.Search = string.Empty;
        ViewBag.PageHeading = "Your Bookmarked Recipes";
        var liked = await _db.RecipeLikes.Where(x => x.UserId == userId).Select(x => x.RecipeId).ToListAsync();
        var bookmarked = ids;
        return View("Index", recipes.Select(r => new RecipeListItemViewModel {
            Id = r.Id, Title = r.Title, AuthorName = r.CreatedByUser?.FullName ?? "Recipee Community",
            Description = r.Description, ImageUrl = r.ImageUrl,
            Ingredients = r.RecipeIngredients.Select(ri => ri.Ingredient.Name).ToList(),
            IsLiked = liked.Contains(r.Id), IsBookmarked = bookmarked.Contains(r.Id)
        }).ToList());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLike(int id)
    {
        int userId = CurrentUserId();
        var existing = await _db.RecipeLikes.FindAsync(userId, id);
        if (existing == null && await _db.Recipes.AnyAsync(r => r.Id == id))
            _db.RecipeLikes.Add(new RecipeLike { UserId = userId, RecipeId = id });
        else if (existing != null) _db.RecipeLikes.Remove(existing);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleBookmark(int id)
    {
        int userId = CurrentUserId();
        var existing = await _db.RecipeBookmarks.FindAsync(userId, id);
        if (existing == null && await _db.Recipes.AnyAsync(r => r.Id == id))
            _db.RecipeBookmarks.Add(new RecipeBookmark { UserId = userId, RecipeId = id });
        else if (existing != null) _db.RecipeBookmarks.Remove(existing);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private int CurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private async Task AddStarterRecipes(int userId)
    {
        var data = new[] {
            ("Fresh Garden Salad", "A fresh, colorful salad with crisp greens, tomatoes and a light dressing.", "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=900&q=85", new[] {"Lettuce", "Cherry tomatoes", "Cucumber", "Olive oil"}),
            ("Healthy Grain Bowl", "A nourishing bowl with grains, vegetables and a creamy topping.", "https://images.unsplash.com/photo-1511690743698-d9d85f2fbf38?auto=format&fit=crop&w=900&q=85", new[] {"Brown rice", "Avocado", "Green beans", "Tomatoes"}),
            ("Avocado Lunch Bowl", "An easy lunch bowl packed with fresh vegetables and herbs.", "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=900&q=85", new[] {"Avocado", "Mixed greens", "Carrot", "Lemon"})
        };
        foreach (var item in data)
        {
            var recipe = new Recipe { Title = item.Item1, Description = item.Item2, ImageUrl = item.Item3, CreatedByUserId = userId };
            foreach (var name in item.Item4)
            {
                var ingredient = await _db.Ingredients.SingleOrDefaultAsync(i => i.Name == name);
                if (ingredient == null) { ingredient = new Ingredient { Name = name }; _db.Ingredients.Add(ingredient); await _db.SaveChangesAsync(); }
                recipe.RecipeIngredients.Add(new RecipeIngredient { IngredientId = ingredient.Id, Ingredient = ingredient, Recipe = recipe, Quantity = "" });
            }
            _db.Recipes.Add(recipe);
        }
        await _db.SaveChangesAsync();
    }
}
