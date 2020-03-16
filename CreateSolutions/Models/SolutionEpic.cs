using System;

namespace CreateSolutions.Models
{
    class SolutionEpic
    {
        public string SupplierID { get; set; }
        public string SolutionID { get; set; }
        public string AdditionalServiceID { get; set; } = string.Empty;
        public Guid CapabilityID { get; set; }
        public string EpicID { get; set; }
        public string Level { get; set; }
        public int EpicFinalAssessmentResult { get; set; } = 1;


    }
}
