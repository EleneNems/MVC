namespace Lecture_3.Models;

public class RecipeBookmark
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
