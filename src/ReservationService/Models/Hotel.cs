using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationService.Models;

[Table("hotels")]
public class Hotel
{
    [Column("id")]
    public int Id { get; set; }

    [Column("hotel_uid")]
    public Guid HotelUid { get; set; }

    [Column("name"), MaxLength(255)]
    public string Name { get; set; } = "";

    [Column("country"), MaxLength(80)]
    public string Country { get; set; } = "";

    [Column("city"), MaxLength(80)]
    public string City { get; set; } = "";

    [Column("address"), MaxLength(255)]
    public string Address { get; set; } = "";

    [Column("stars")]
    public int? Stars { get; set; }

    [Column("price")]
    public int Price { get; set; }
}