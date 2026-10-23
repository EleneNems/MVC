using Microsoft.AspNetCore.Identity;
using Quiz1.Models;

namespace Quiz1.Services;

public class UserStore
{
    private readonly PasswordHasher<User> _hasher = new();
    private readonly List<User> _users = new();
    private readonly object _lock = new();

    public static readonly Guid EleneID = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid GiorgiId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public UserStore()
    {
        Add(EleneID, "elene@gmail.com", "123");
        Add(GiorgiId, "giorgi@gmail.com", "123");
    }

    private void Add(Guid id, string email, string password)
    {
        var u = new User { Id = id, Email = email, MustChangePassword = true };
        u.PasswordHash = _hasher.HashPassword(u, password);
        _users.Add(u);
    }

    public User? FindByEmail(string email) =>
        _users.FirstOrDefault(u => u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));

    public User? FindById(Guid id) => _users.FirstOrDefault(u => u.Id == id);

    public bool Verify(User user, string password) =>
        _hasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Failed;

    public void SetPassword(User user, string password, bool mustChange)
    {
        lock (_lock)
        {
            user.PasswordHash = _hasher.HashPassword(user, password);
            user.MustChangePassword = mustChange;
        }
    }
}
