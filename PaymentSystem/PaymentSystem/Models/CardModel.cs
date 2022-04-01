using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Models;

public class CardModel
{
    [MaxLength(16)]
    [MinLength(16)]
    public string CardNumber { get; set; }

    [MaxLength(3)]
    [MinLength(3)]
    public string CardCvc { get; set; }
    
    [MaxLength(4)]
    [MinLength(4)]
    public string CardDate { get; set; }
    
    [MaxLength(20)]
    public string AmountOfMoney { get; set; }
}