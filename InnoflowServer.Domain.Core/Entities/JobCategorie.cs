using System.Collections.Generic;

namespace InnoflowServer.Domain.Core.Entities
{
    public class JobCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserJobCategory> UserJobCategories { get; set; }
        
    }
}
