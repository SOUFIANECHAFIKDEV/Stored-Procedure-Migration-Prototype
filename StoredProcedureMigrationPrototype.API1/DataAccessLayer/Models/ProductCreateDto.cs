namespace StoredProcedureMigrationPrototype.API1.DataAccessLayer.Models
{
    public class ProductCreateDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
