using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Models;

public class CardModel
{
    [Required]
    [RegularExpression(CardValidation.CardNumberRegularExpression, ErrorMessage = "Invalid card number")]
    public string CardNumber { get; set; }

    [Required]
    [RegularExpression(CardValidation.CardCvcRegularExpression, ErrorMessage = "Invalid card cvc")]
    public string CardCvc { get; set; }
    
    [Required]
    [RegularExpression(CardValidation.CardDateRegularExpression, ErrorMessage = "Invalid card date")]
    public string CardDate { get; set; }
    
    [Required]
    [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    public decimal AmountOfMoney { get; set; }
}