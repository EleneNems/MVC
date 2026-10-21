using System.ComponentModel.DataAnnotations;

namespace Lecture_3.Models;

public class RecipeIngredient
{
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
    public int IngredientId { get; set; }
    public Ingredient Ingredient { get; set; } = null!;
    [StringLength(60)] public string Quantity { get; set; } = string.Empty;
}
