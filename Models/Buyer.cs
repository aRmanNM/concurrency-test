namespace webapi.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public bool IsPaid { get; set; }
    }
}