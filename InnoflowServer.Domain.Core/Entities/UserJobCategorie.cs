namespace InnoflowServer.Domain.Core.Entities
{
    public class UserJobCategorie 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int JobCaregorieId { get; set; }
        public JobCategorie JobCategorie { get; set; }
    }
}
