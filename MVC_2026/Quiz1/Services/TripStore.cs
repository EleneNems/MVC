using Quiz1.Models;

namespace Quiz1.Services;

public class TripStore
{
    private readonly List<Trip> _trips = new()
    {
        new() { Id = 1, UserId = UserStore.EleneID, HotelName = "Lakeside Motel Warefront", Rating = 4.5, ReviewCount = 1200,
                IsRefundable = false, CheckIn = new(2022, 3, 18), CheckOut = new(2022, 3, 20), Rooms = 1, OldPrice = 150, Price = 130 },
        new() { Id = 2, UserId = UserStore.EleneID, HotelName = "Lakeside Motel Warefront", Rating = 4.5, ReviewCount = 1200,
                IsRefundable = false, CheckIn = new(2022, 3, 18), CheckOut = new(2022, 3, 20), Rooms = 1, OldPrice = 150, Price = 130 },
        new() { Id = 3, UserId = UserStore.GiorgiId, HotelName = "Lakeside Motel Warefront", Rating = 4.5, ReviewCount = 1200,
                IsRefundable = false, CheckIn = new(2022, 3, 18), CheckOut = new(2022, 3, 20), Rooms = 1, OldPrice = 150, Price = 130 },
        new() { Id = 4, UserId = UserStore.GiorgiId, HotelName = "Lakeside Motel Warefront", Rating = 4.5, ReviewCount = 1200,
                IsRefundable = false, CheckIn = new(2022, 3, 18), CheckOut = new(2022, 3, 20), Rooms = 1, OldPrice = 150, Price = 130 },
        new() { Id = 5, UserId = UserStore.GiorgiId, HotelName = "Lakeside Motel Warefront", Rating = 4.5, ReviewCount = 1200,
                IsRefundable = false, CheckIn = new(2022, 3, 18), CheckOut = new(2022, 3, 20), Rooms = 1, OldPrice = 150, Price = 130 },
        new() { Id = 6, UserId = UserStore.GiorgiId, HotelName = "Lakeside Motel Warefront", Rating = 4.5, ReviewCount = 1200,
                IsRefundable = false, CheckIn = new(2022, 3, 18), CheckOut = new(2022, 3, 20), Rooms = 1, OldPrice = 150, Price = 130 },
    };

    public List<Trip> GetForUser(Guid userId) => _trips.Where(t => t.UserId == userId).ToList();
}
