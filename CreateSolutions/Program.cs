using Bogus;
using CreateSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreateSolutions
{
    class Program
    {   
        static void Main(string[] args)
        {
            var connectionString = args.Length > 0 ? args[0] : DefaultValues.ConnectionString;

            var faker = new Faker();

            var numFoundationSolutions = 10;
            var numNonFoundationSolutions = 490;

            string solutionId;

            var capabilities = GenerateCapabilities.GetAllCapabilities(connectionString);

            for (int i = 0; i < numFoundationSolutions; i++)
            {
                solutionId = $"{50000000 + i}";

                InsertSolution(connectionString, solutionId, capabilities, faker);
                GenerateSolution.SetAsFoundation(connectionString, solutionId);

                Console.WriteLine($"FoundationSolutions: {i +1}, id={solutionId}");
            }

            for (int j = 0; j < numNonFoundationSolutions; j++)
            {
                solutionId = $"{60000000 + j}";

                InsertSolution(connectionString, solutionId, capabilities, faker);

                Console.WriteLine($"NonFoundationSolutions: {j + 1}, id={solutionId}");
            }
        }

        private static void InsertSolution(string connectionString, string solutionId, IEnumerable<Capability> capabilities, Faker faker)
        {
            var solutionCaps = faker.Random.Int(8, 17);
            var solCaps = GetRandomCaps(capabilities, solutionCaps);

            var solution = GenerateSolution.NewSolution(faker, solutionId);
            var solutionDetail = GenerateSolution.NewSolutionDetail(faker, solutionId);
            GenerateSolution.InsertSolution(connectionString, solution);
            GenerateSolution.InsetSolutionDetail(connectionString, solutionDetail);
            GenerateSolution.Update(connectionString, solutionId, solutionDetail.SolutionDetailId);

            GenerateCapabilities.InsertSolutionCapabilities(connectionString, solCaps, solutionId);
            GenerateCapabilities.InsertSolutionEpics(connectionString, solCaps, solutionId);

            var contact = GenerateContacts.NewContactDetail(solutionId);
            GenerateContacts.InsertContact(connectionString, contact);
        }

        private static IEnumerable<Capability> GetRandomCaps(IEnumerable<Capability> capabilities, int CapsCount)
        {
            return capabilities.OrderBy(s => Guid.NewGuid()).Take(CapsCount);
        }
    }
}