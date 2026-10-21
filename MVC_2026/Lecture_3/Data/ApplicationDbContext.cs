using Microsoft.EntityFrameworkCore;
using Lecture_3.Models;

namespace Lecture_3.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public DbSet<RecipeLike> RecipeLikes => Set<RecipeLike>();
    public DbSet<RecipeBookmark> RecipeBookmarks => Set<RecipeBookmark>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(e => {
            e.ToTable("Users"); e.HasKey(x => x.Id);
            e.Property(x => x.FullName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
        });
        modelBuilder.Entity<Recipe>(e => {
            e.ToTable("Recipes"); e.HasKey(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(150).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.ImageUrl).HasMaxLength(500);
            e.Property(x => x.MakingProcess).HasMaxLength(2000);
            e.Property(x => x.VideoLink).HasMaxLength(500);
            e.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Ingredient>(e => { e.ToTable("Ingredients"); e.HasKey(x => x.Id); e.Property(x => x.Name).HasMaxLength(120).IsRequired(); e.HasIndex(x => x.Name).IsUnique(); });
        modelBuilder.Entity<RecipeIngredient>(e => {
            e.ToTable("RecipeIngredients"); e.HasKey(x => new { x.RecipeId, x.IngredientId });
            e.Property(x => x.Quantity).HasMaxLength(60);
            e.HasOne(x => x.Recipe).WithMany(x => x.RecipeIngredients).HasForeignKey(x => x.RecipeId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Ingredient).WithMany(x => x.RecipeIngredients).HasForeignKey(x => x.IngredientId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<RecipeLike>(e => { e.ToTable("RecipeLikes"); e.HasKey(x => new { x.UserId, x.RecipeId }); e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade); e.HasOne(x => x.Recipe).WithMany(x => x.Likes).HasForeignKey(x => x.RecipeId).OnDelete(DeleteBehavior.Cascade); });
        modelBuilder.Entity<RecipeBookmark>(e => { e.ToTable("RecipeBookmarks"); e.HasKey(x => new { x.UserId, x.RecipeId }); e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade); e.HasOne(x => x.Recipe).WithMany(x => x.Bookmarks).HasForeignKey(x => x.RecipeId).OnDelete(DeleteBehavior.Cascade); });
    }
}
