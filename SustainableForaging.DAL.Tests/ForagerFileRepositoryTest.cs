using NUnit.Framework;
using SustainableForaging.Core.Models;
using System.Collections.Generic;

namespace SustainableForaging.DAL.Tests
{
    public class ForagerFileRepositoryTest
    {
        ForagerFileRepository repo = new ForagerFileRepository(@"data\foragers.csv");

        [Test]
        public void ShouldFindAll()
        {
            List<Forager> all = repo.FindAll();
            Assert.AreEqual(1005, all.Count);
        }

        [Test]
        public void AddShouldReturnTrueWhenForagerNotEmpty()
        {
            Forager forager = new();
            forager.FirstName = "Bob";
            forager.LastName = "Jenkins";
            forager.State = "KY";
            
            Assert.IsTrue(repo.Add(forager));
        }
    }
}
