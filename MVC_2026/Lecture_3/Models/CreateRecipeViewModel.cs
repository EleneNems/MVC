using System.ComponentModel.DataAnnotations;

namespace Lecture_3.Models;

public class CreateRecipeViewModel
{
    [Required, StringLength(150)]
    [Display(Name = "Recipe name")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [StringLength(2000)]
    [Display(Name = "Ingredients (one per line)")]
    public string Ingredients { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Photo URL")]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(2000)]
    [Display(Name = "Making process")]
    public string MakingProcess { get; set; } = string.Empty;

    [StringLength(500), Url]
    [Display(Name = "Video link")]
    public string VideoLink { get; set; } = string.Empty;
}
