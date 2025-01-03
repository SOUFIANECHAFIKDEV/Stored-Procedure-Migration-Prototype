namespace StoredProcedureMigrationPrototype.API2.DataAccessLayer.Models
{
    public class OrderCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
