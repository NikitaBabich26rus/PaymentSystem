using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Models;

public class UpdateUserProfileModel
{
    [Required(ErrorMessage = "Не указано имя")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Не указана фамилия")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Не указан Email")]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Не указанa роль")]
    public string Role { get; set; }
    
    [Required(ErrorMessage = "Не указан статус")]
    public string Status { get; set; }
}