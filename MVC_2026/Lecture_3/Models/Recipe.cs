using System.ComponentModel.DataAnnotations;

namespace Lecture_3.Models;

public class Recipe
{
    public int Id { get; set; }
    [Required, StringLength(150)] public string Title { get; set; } = string.Empty;
    [StringLength(2000)] public string Description { get; set; } = string.Empty;
    [StringLength(500)] public string ImageUrl { get; set; } = string.Empty;
    [StringLength(2000)] public string MakingProcess { get; set; } = string.Empty;
    [StringLength(500)] public string VideoLink { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<RecipeLike> Likes { get; set; } = new List<RecipeLike>();
    public ICollection<RecipeBookmark> Bookmarks { get; set; } = new List<RecipeBookmark>();
}
