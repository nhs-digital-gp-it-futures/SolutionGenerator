using System;
using System.Collections.Generic;
using System.Text;

namespace CreateSolutions.Models
{
    public sealed class Capability
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CapabilityRef { get; set; }
    }
}
