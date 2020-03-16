using System;
using Bogus;
using CreateSolutions.ClientApplication;
using CreateSolutions.Models;
using CreateSolutions.Utils;

namespace CreateSolutions
{
    internal static class GenerateSolution
    {
        internal const string CompleteHostingTypes =
            "{\"PublicCloud\":{\"Summary\":\"dasfdasf\",\"Link\":\"http://www.bbc.co.uk\",\"RequiresHSCN\":\"This Solution requires a HSCN/N3 connection\"},\"PrivateCloud\":{\"Summary\":\"asdfasdfdasf\",\"Link\":\"http://www.bbc.co.uk\",\"HostingModel\":\"adfadfasd\",\"RequiresHSCN\":\"This Solution requires a HSCN/N3 connection\"},\"HybridHostingType\":{\"Summary\":\"asdfasdfasdf\",\"Link\":\"https://www.bbc.co.uk\",\"HostingModel\":\"asdfasdfasdf\",\"RequiresHSCN\":\"This Solution requires a HSCN/N3 connection\"},\"OnPremise\":{\"Summary\":\"sdfasdfasdfasdf\",\"Link\":\"https://www.bbc.co.uk\",\"HostingModel\":\"sdfasdfasdf\",\"RequiresHSCN\":\"This Solution requires a HSCN/N3 connection\"}}";

        internal static void InsertSolution(string connectionString, Solution solution)
        {
            var query = @"INSERT INTO Solution (
                            Id, 
                            SupplierId, 
                            Name, 
                            Version, 
                            PublishedStatusId, 
                            AuthorityStatusId, 
                            SupplierStatusId, 
                            OnCatalogueVersion, 
                            LastUpdatedBy, 
                            LastUpdated) 
                        VALUES (
                            @SolutionId, 
                            @SupplierId, 
                            @SolutionName, 
                            @SolutionVersion, 
                            @PublishedStatusId,
                            @AuthorityStatusId,
                            @SupplierStatusId, 
                            0, 
                            @LastUpdatedBy, 
                            @LastUpdated
                        )";
            SqlExecutor.Execute<Solution>(connectionString, query, solution);
        }

        internal static void Update(string connectionString, string SolutionId, Guid solutionDetailId)
        {
            var query = @"UPDATE Solution 
                        SET 
                            SolutionDetailId=@solutionDetailId                             
                        WHERE Id=@solutionId";
            SqlExecutor.Execute<Solution>(connectionString, query, new { SolutionId, solutionDetailId});
        }

        internal static void SetAsFoundation(string connectionString, string SolutionId)
        {
            var query =
                "INSERT INTO [dbo].[FrameworkSolutions] ([FrameworkId] ,[SolutionId] ,[IsFoundation], [LastUpdated] ,[LastUpdatedBy]) VALUES ('NHSDGP001', @SolutionId ,1 ,GetUtcDate(), '00000000-0000-0000-0000-000000000000')";
            SqlExecutor.Execute<Solution>(connectionString, query, new{SolutionId});
        }

        internal static Solution NewSolution(Faker faker, string Id)
        {
            return new Solution
            {
                Id = Id,
                Name = faker.Name.JobTitle(),
                Version = faker.System.Semver(),
                LastUpdated = DateTime.Now,
                LastUpdatedBy = Guid.Empty
            };
        }

        internal static SolutionDetail NewSolutionDetail(Faker faker, string solutionId)
        {
            var sd = new SolutionDetail
            {
                SolutionDetailId = Guid.NewGuid(),
                SolutionId = solutionId,
                AboutUrl = faker.Internet.Url(),
                Features = GenerateFeatures(5, faker),
                ClientApplication = ClientApplicationStringBuilder.GetClientAppString(clientApplicationTypes: "Browser-based, Native mobile or tablet, Native desktop"),
                Summary = faker.Lorem.Paragraphs(1),
                FullDescription = faker.Lorem.Paragraphs(7),
                RoadMap = faker.Rant.Review(),
                Hosting = CompleteHostingTypes,
                IntegrationsUrl = faker.Internet.Url(),
                ImplementationDetail = faker.Lorem.Sentences(2)
            };

            if (sd.Summary.Length > 300)
            {
                sd.Summary = sd.Summary.Remove(299);
            }

            if (sd.FullDescription.Length > 1000)
            {
                sd.FullDescription = sd.FullDescription.Remove(999);
            }

            return sd;
        }

        internal static void InsetSolutionDetail(string connectionString, SolutionDetail solutionDetail)
        {
            var query = @"INSERT INTO SolutionDetail (
                            Id, 
                            LastUpdatedBy, 
                            LastUpdated, 
                            SolutionId,
                            Features,
                            ClientApplication, 
                            AboutUrl, 
                            Summary, 
                            FullDescription, 
                            RoadMap, 
                            Hosting, 
                            IntegrationsUrl, 
                            ImplementationDetail ) 
                        VALUES (
                            @SolutionDetailId, 
                            @LastUpdatedBy, 
                            @LastUpdated, 
                            @SolutionId,
                            @Features,
                            @ClientApplication,
                            @AboutUrl,
                            @Summary,
                            @FullDescription,
                            @RoadMap,
                            @Hosting,
                            @IntegrationsUrl,
                            @ImplementationDetail)";

            SqlExecutor.Execute<SolutionDetail>(connectionString, query, solutionDetail);
        }

        private static string GenerateFeatures(int numFeatures, Faker faker)
        {
            if (numFeatures <= 0)
                return string.Empty;

            var featuresArray = new string[numFeatures];

            if (numFeatures > 0)
                for (var i = 0; i < numFeatures; i++)
                    featuresArray[i] = $"\"{faker.Commerce.ProductAdjective()}\"";

            return $"[{string.Join(",", featuresArray)}]";
        }
    }
}
