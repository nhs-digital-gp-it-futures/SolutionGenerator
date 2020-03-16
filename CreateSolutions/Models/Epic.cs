using System;
using System.Collections.Generic;
using System.Text;

namespace CreateSolutions.Models
{
    public sealed class Epic
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Guid CapabilityId { get; set; }
	}
}
