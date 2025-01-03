using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoredProcedureMigrationPrototype.Data.Models.Api2.Model
{
    public class Order
    {
        [Key] // Marks this property as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configures auto-increment for the Id
        public int Id { get; set; }

        [Required] // Ensures ProductId is required
        public int ProductId { get; set; }

        [Required] // Ensures Quantity is required
        public int Quantity { get; set; }

        [Range(0.01, 100000.00)] // Validates TotalPrice is within the range
        [Column(TypeName = "decimal(18,4)")] // Configures precision and scale for the column
        public decimal TotalPriceEdit { get; set; }

        public decimal OriginalPrice { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Configures the default value to be set by the database
        [Column(TypeName = "datetime2")] // Ensures the correct SQL type for OrderDate
        public DateTime OrderDate { get; set; }
    }
}
