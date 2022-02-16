using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "Не указано имя")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Не указана фамилия")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Не указан Email")]
    [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
    public string Email { get; set; }
         
    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
         
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароль введен неверно")]
    public string ConfirmPassword { get; set; }
}