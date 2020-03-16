using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using CreateSolutions.Models;
using CreateSolutions.Utils;

namespace CreateSolutions
{
    internal static class GenerateContacts
    {
        internal static MarketingContacts NewContactDetail(string solutionId)
        {
            MarketingContacts contact = new Faker<MarketingContacts>("en_GB")
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
                .RuleFor(c => c.Department, f => f.Name.JobTitle())
                .Generate();

            contact.SolutionId = solutionId;
            return contact;
        }

        internal static void InsertContact(string connectionString, MarketingContacts contact)
        {
            var query = @"INSERT INTO [MarketingContact] (
                            SolutionId, 
                            FirstName, 
                            LastName, 
                            Email, 
                            PhoneNumber, 
                            Department, 
                            LastUpdated, 
                            LastUpdatedBy) 
                        VALUES(
                            @solutionId, 
                            @firstName, 
                            @lastName, 
                            @email, 
                            @phoneNumber, 
                            @department, 
                            GETUTCDATE(), 
                            '00000000-0000-0000-0000-000000000000')";
            SqlExecutor.Execute<MarketingContacts>(connectionString, query, contact);
        }
    }
}
