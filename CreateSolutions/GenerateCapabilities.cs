using System;
using CreateSolutions.Models;
using System.Collections.Generic;
using CreateSolutions.Utils;

namespace CreateSolutions
{
    internal static class GenerateCapabilities
    {
        internal static IEnumerable<Capability> GetAllCapabilities(string connectionString)
        {
            var query = "SELECT * FROM Capability";

            return SqlExecutor.Execute<Capability>(connectionString, query, null);
        }

        internal static IEnumerable<Epic> GetAllEpicsForCapability(string connectionString, Guid CapabilityId)
        {
            var query = "SELECT * FROM Epic WHERE CapabilityId=@CapabilityId";

            return SqlExecutor.Execute<Epic>(connectionString, query, new {CapabilityId});
        }

        internal static void InsertSolutionCapabilities(string connectionString, IEnumerable<Capability> capabilities, string SolutionId)
        {
            var solutionCapabilities = new List<SolutionCapability>();

            foreach (var cap in capabilities)
            {
                solutionCapabilities.Add(new SolutionCapability { CapabilityId = cap.Id, SolutionID = SolutionId});
            }

            var query =
                "INSERT INTO SolutionCapability (CapabilityId, SolutionId, StatusId, LastUpdated, LastUpdatedBy) VALUES (@CapabilityId, @SolutionId, 1, GetUtcDate(), '00000000-0000-0000-0000-000000000000')";
            foreach (var solCap in solutionCapabilities)
            {
                SqlExecutor.Execute<SolutionCapability>(connectionString, query,
                    new {solCap.CapabilityId, SolutionId });
            }
        }

        internal static void InsertSolutionEpics(string connectionString, IEnumerable<Capability> capabilities,
            string SolutionId)
        {
            var solEpics = new List<SolutionEpic>();

            foreach (var cap in capabilities)
            {
                var epics = GetAllEpicsForCapability(connectionString, cap.Id);
                foreach (var epic in epics)
                {
                    var solEpic = new SolutionEpic
                    {
                        CapabilityID = cap.Id,
                        EpicFinalAssessmentResult = 1,
                        EpicID = epic.Id
                    };

                    solEpics.Add(solEpic);
                }
            }

            var query =
                "INSERT INTO SolutionEpic (SolutionId, CapabilityId, EpicId, StatusId, LastUpdated, LastUpdatedBy) VALUES (@SolutionId, @CapabilityId, @EpicId, 1, GetUtcDate(), '00000000-0000-0000-0000-000000000000')";
            
            foreach (var epic in solEpics)
            {
                SqlExecutor.Execute<SolutionCapability>(connectionString, query,
                    new { CapabilityId = epic.CapabilityID, SolutionId, EpicId = epic.EpicID });
            }
        }
    }
}
