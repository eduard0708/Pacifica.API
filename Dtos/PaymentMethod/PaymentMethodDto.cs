namespace Pacifica.API.Dtos.PaymentMethod
{
    public class PaymentMethodDto :AuditDetails
    {
        public int Id { get; set; }
        public string? PaymentMethodName { get; set; }
    }
}