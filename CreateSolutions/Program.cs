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
            var connectionString = args[0] ?? DefaultValues.ConnectionString;

            var faker = new Faker();

            var foundationSolutionCaps = faker.Random.Int(8, 17);
            var nonFoundationSolCaps = faker.Random.Int(3,6);

            var numFoundationSolutions = 10;
            var numNonFoundationSolutions = 490;


            string solutionId;

            var capabilities = GenerateCapabilities.GetAllCapabilities(connectionString);

            for (int i = 0; i < numFoundationSolutions; i++)
            {
                var solCaps = GetRandomCaps(capabilities, foundationSolutionCaps);

                solutionId = $"{50000000 + i}";
                var solution = GenerateSolution.NewSolution(faker, solutionId);
                var solutionDetail = GenerateSolution.NewSolutionDetail(faker, solutionId);
                GenerateSolution.InsertSolution(connectionString, solution);
                GenerateSolution.InsetSolutionDetail(connectionString, solutionDetail);
                GenerateSolution.Update(connectionString, solutionId, solutionDetail.SolutionDetailId);
                GenerateSolution.SetAsFoundation(connectionString, solutionId);

                GenerateCapabilities.InsertSolutionCapabilities(connectionString, solCaps, solutionId);
                GenerateCapabilities.InsertSolutionEpics(connectionString, solCaps, solutionId);

                var contact = GenerateContacts.NewContactDetail(solutionId);
                GenerateContacts.InsertContact(connectionString, contact);

                Console.WriteLine($"FoundationSolutions: {i +1}, id={solutionId}");
            }

            for (int j = 0; j < numNonFoundationSolutions; j++)
            {
                solutionId = $"{60000000 + j}";
                var solCaps = GetRandomCaps(capabilities, nonFoundationSolCaps);
                
                var solution = GenerateSolution.NewSolution(faker, solutionId);
                var solutionDetail = GenerateSolution.NewSolutionDetail(faker, solutionId);
                GenerateSolution.InsertSolution(connectionString, solution);
                GenerateSolution.InsetSolutionDetail(connectionString, solutionDetail);
                GenerateSolution.Update(connectionString, solutionId, solutionDetail.SolutionDetailId);

                GenerateCapabilities.InsertSolutionCapabilities(connectionString, solCaps, solutionId);
                GenerateCapabilities.InsertSolutionEpics(connectionString, solCaps, solutionId);

                var contact = GenerateContacts.NewContactDetail(solutionId);
                GenerateContacts.InsertContact(connectionString, contact);

                Console.WriteLine($"NonFoundationSolutions: {j + 1}, id={solutionId}");
            }
        }

        private static IEnumerable<Capability> GetRandomCaps(IEnumerable<Capability> capabilities, int CapsCount)
        {
            return capabilities.OrderBy(s => Guid.NewGuid()).Take(CapsCount);
        }
    }
}