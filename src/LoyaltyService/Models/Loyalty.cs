using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoyaltyService.Models;

[Table("loyalty")]
public class Loyalty
{
    [Column("id")]
    public int Id { get; set; }

    [Column("username"), MaxLength(80)]
    public string Username { get; set; } = "";

    [Column("reservation_count")]
    public int ReservationCount { get; set; }

    [Column("status"), MaxLength(80)]
    public string Status { get; set; } = "BRONZE";

    [Column("discount")]
    public int Discount { get; set; }
}