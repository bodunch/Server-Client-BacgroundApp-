using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities
{
    [Table("SystemInfo")]
    public class SystemInfoEntity
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string OperatingSystem { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ComputerName { get; set; } = string.Empty;
        public string RegisteredUser { get; set; } = string.Empty;
        public string LastBootTime { get; set; } = string.Empty;
    }
}
