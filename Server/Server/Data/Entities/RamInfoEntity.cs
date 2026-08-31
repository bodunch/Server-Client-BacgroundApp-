using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Server.Data.Entities
{
    [Table("RamInfo")]
    public class RamInfoEntity
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string MemoryCount { get; set; } = string.Empty;
    }
}
