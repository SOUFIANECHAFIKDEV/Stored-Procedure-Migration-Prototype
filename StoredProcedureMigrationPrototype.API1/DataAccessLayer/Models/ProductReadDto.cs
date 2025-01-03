namespace StoredProcedureMigrationPrototype.API1.DataAccessLayer.Models
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string ProductNameEdited { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal PriceAfterReduction { get; set; }
        public string Category { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
