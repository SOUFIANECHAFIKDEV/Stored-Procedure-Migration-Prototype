namespace StoredProcedureMigrationPrototype.API2.DataAccessLayer.Models
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
