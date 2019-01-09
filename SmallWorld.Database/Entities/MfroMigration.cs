using System.ComponentModel.DataAnnotations.Schema;

namespace SmallWorld.Database.Entities
{
    [Table("__MfroMigrationsHistory")]
    public class MfroMigration
    {
        public int Id { get; set; }

        public string Identifier { get; set; }
    }
}
