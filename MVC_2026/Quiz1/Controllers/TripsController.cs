using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quiz1.Services;

namespace Quiz1.Controllers;

[Authorize(Policy = "FullAccess")]
public class TripsController : Controller
{
    private readonly TripStore _trips;
    public TripsController(TripStore trips) => _trips = trips;

    public IActionResult Index()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return View(_trips.GetForUser(userId));
    }
}
