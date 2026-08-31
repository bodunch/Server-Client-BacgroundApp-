using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities
{
    [Table("CpuInfo")]
    public class CpuInfoEntity
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CPUName { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string NumOfCores { get; set; } = string.Empty;
        public string NumOfStreams { get; set; } = string.Empty;
    }
}
