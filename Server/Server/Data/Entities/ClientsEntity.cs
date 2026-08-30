using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Entities
{
    [Table("Clients")]
    public class ClientsEntity
    {
        [Key]
        public int Id { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string FirstConnected { get; set; } = string.Empty;
        public string LastSeen { get; set; } = string.Empty;
    }
}
