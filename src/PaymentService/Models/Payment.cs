using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentService.Models;

[Table("payment")]
public class Payment
{
    [Column("id")]
    public int Id { get; set; }

    [Column("payment_uid")]
    public Guid PaymentUid { get; set; }

    [Column("status"), MaxLength(20)]
    public string Status { get; set; } = "PAID";

    [Column("price")]
    public int Price { get; set; }
}