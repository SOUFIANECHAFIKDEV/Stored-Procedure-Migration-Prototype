namespace StoredProcedureMigrationPrototype.Data.Attributes
{
    /// <summary>
    /// Attribute to mark entities that should be excluded from EF Core migrations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExcludeFromMigrationsAttribute : Attribute
    {
    }
}
