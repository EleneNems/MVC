using System.ComponentModel.DataAnnotations;

namespace Lecture_3.Models;

public class Ingredient
{
    public int Id { get; set; }
    [Required, StringLength(120)] public string Name { get; set; } = string.Empty;
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
