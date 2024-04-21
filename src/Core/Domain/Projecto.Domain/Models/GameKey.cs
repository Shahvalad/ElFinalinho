namespace Projecto.Domain.Models
{
    public class GameKey
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsAssigned { get; set; }
        public string? AssignedTo { get; set; }
        public Game Game { get; set; }
        public int GameId { get; set; }
    }
}
