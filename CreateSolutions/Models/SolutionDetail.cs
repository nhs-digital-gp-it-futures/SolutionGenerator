using System;
using System.Collections.Generic;
using System.Text;

namespace CreateSolutions.Models
{
    public sealed class SolutionDetail
    {
        public Guid SolutionDetailId { get; set; }
        public string SolutionId { get; set; }
        public string AboutUrl { get; set; }
        public string Features { get; set; }
        public string ClientApplication { get; set; }
        public string Summary { get; set; }
        public string FullDescription { get; set; }
        public string RoadMap { get; set; }
        public string Hosting { get; set; }
        public string IntegrationsUrl { get; set; }
        public string ImplementationDetail { get; set; }
        public Guid LastUpdatedBy { get; set; } = Guid.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
