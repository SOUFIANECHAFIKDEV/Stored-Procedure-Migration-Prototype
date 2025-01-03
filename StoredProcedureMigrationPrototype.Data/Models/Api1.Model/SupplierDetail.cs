using StoredProcedureMigrationPrototype.Data.Attributes;

namespace StoredProcedureMigrationPrototype.Data.Models.Api1.Model
{
    [ExcludeFromMigrations] // Mark this entity to be excluded from migrations
    public class SupplierDetail
    {
        public int Id { get; set; } // Primary key
        public string Address { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
    }
}
