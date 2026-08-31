using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities
{
    [Table("ComputerInfo")]
    public class ComputerInfoEntity
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string PCModel { get; set; } = string.Empty;
        public string SystemType { get; set; } = string.Empty;
        public string CountOfCpu { get; set; } = string.Empty;
        public string SystemStart { get; set; } = string.Empty;
        public string StatusOfStart { get; set; } = string.Empty;
    }
}
