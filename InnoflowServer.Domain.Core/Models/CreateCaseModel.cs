using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoflowServer.Domain.Core.Models
{
    public class CreateCaseModel
    {

        public string CompanyName { get; set; }

        public int JobCategorieId { get; set; }

        public string JobLevelOfComplexity { get; set; }

        public string JobLink { get; set; }

        public string CustomerNotes { get; set; }

        public bool CaseDevelopment { get; set; }

        public bool CriteriaAndFeedback { get; set; }

        public bool UnbiasedScoring { get; set; }

        public bool CaseAccepted { get; set; }

        public string UserId { get; set; }
    }
}
