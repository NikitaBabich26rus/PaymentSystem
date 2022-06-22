namespace PaymentSystem.Models;

public class ErrorModel
{
    public string? RequestId { get; set; }
    
    public string? ErrorMessage { get; set; }
    
    public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}