using System.ComponentModel.DataAnnotations;

namespace TC.Application.Entities
{
    public class TicketsCatalog
    {
        public int Id { get; set; }

        [StringLength(255, MinimumLength = 1, ErrorMessage = "The Name field must be between 1 and 255 characters.")]
        public required string Name { get; set; }

        public DateTime DataTime { get; set; }
        public required User Owner { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public List<Ticket> Tasks { get; set; } = new List<Ticket>();
    }
}
