namespace InnoflowServer.Domain.Core.Entities
{
    public class UserJobCategory 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int JobCaregorieId { get; set; }
        public JobCategory JobCategory { get; set; }
    }
}
