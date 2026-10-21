namespace Lecture_3.Models;

public class RecipeListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> Ingredients { get; set; } = new();
    public bool IsLiked { get; set; }
    public bool IsBookmarked { get; set; }
}
