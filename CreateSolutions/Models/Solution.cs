using System;

namespace CreateSolutions.Models
{
    public sealed class Solution
    {
        public string Id { get; set; }
        public string SolutionId => Id;
        public string Name { get; set; }
        public string SolutionName => Name;
        public string Version { get; set; }
        public string SolutionVersion => Version;
        public string SupplierId { get; set; } = "100000";
        public int PublishedStatusId { get; set; } = 3;
        public int AuthorityStatusId { get; set; } = 1;
        public int SupplierStatusId { get; set; } = 1;
        public Guid SolutionDetailId { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public Guid LastUpdatedBy { get; set; } = Guid.Empty;
    }
}
