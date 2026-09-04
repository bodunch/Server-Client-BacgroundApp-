using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities
{
    [Table("CurrentCPUInfo")]
    public class DynamicCpuInfoEntity
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string JsonPayload { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
    }
}
