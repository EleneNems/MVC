namespace Quiz1.Models;

public class Trip
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string HotelName { get; set; } = "";
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public bool IsRefundable { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Rooms { get; set; } = 1;
    public decimal OldPrice { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = "/img/hotel.png";

    public int Nights => (CheckOut.Date - CheckIn.Date).Days;
}
