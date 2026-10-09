using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationService.Models;

[Table("reservation")]
public class Reservation
{
    [Column("id")]
    public int Id { get; set; }

    [Column("reservation_uid")]
    public Guid ReservationUid { get; set; }

    [Column("username"), MaxLength(80)]
    public string Username { get; set; } = "";

    [Column("payment_uid")]
    public Guid PaymentUid { get; set; }

    [Column("hotel_id")]
    public int HotelId { get; set; }

    public Hotel Hotel { get; set; } = null!;

    [Column("status"), MaxLength(20)]
    public string Status { get; set; } = "PAID";

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_data")]
    public DateTime EndDate { get; set; }
}