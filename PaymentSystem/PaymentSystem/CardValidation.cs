namespace PaymentSystem;

public static class CardValidation
{
    public const string CardNumberRegularExpression = @"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$";

    public const string CardDateRegularExpression = @"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$";
    
    public const string CardCvcRegularExpression = @"^\d{3}$";
}