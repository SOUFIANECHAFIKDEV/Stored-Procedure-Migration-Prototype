using StoredProcedureMigrationPrototype.Data.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoredProcedureMigrationPrototype.Data.Models.Api1.Model
{
    //[ExcludeFromMigrations] // Mark this entity to be excluded from migrations
    public class Product
    {
        [Key] // Marks this property as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configures auto-increment for the Id
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } // Renamed from "Name" To "ProductName"
        public string Description { get; set; } // New column

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        [Required]
        public string CategoryEdit { get; set; }

        public string Category2 { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // Automatically set by the database
        [Column(TypeName = "datetime2")]
        public DateTime DateAdded { get; set; }
    }
}
