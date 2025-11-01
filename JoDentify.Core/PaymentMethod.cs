namespace JoDentify.Core
{
    
    public enum PaymentMethod
    {
        Cash,          // كاش
        CreditCard,    // فيزا
        VodafoneCash,  // فودافون كاش (أو أي محفظة إلكترونية)
        BankTransfer,  // تحويل بنكي
        Other          // أخرى
    }
}