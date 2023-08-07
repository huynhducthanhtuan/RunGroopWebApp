using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
    }
}
