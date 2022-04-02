using SustainableForaging.DAL;
using SustainableForaging.Core.Models;
using SustainableForaging.BLL.Tests.TestDoubles;
using NUnit.Framework;

namespace SustainableForaging.BLL.Tests
{
    public class ForagerServiceTest
    {
        ForagerService foragerService = new ForagerService(new ForagerFileRepository(@"data\foragers.csv"));

        [Test]
        public void AddShouldReturnTrueWhenForagerNotEmpty()
        {
            Forager forager = new();
            forager.FirstName = "Bobby";
            forager.LastName = "James";
            forager.State = "KY";

            Assert.IsTrue(foragerService.Add(forager));
        }

        [Test]
        public void AddShouldReturnFalseWhenDuplicateForager()
        {
            Forager forager = new();
            forager.FirstName = "Bob";
            forager.LastName = "Jenkins";
            forager.State = "KY";

            Assert.IsFalse(foragerService.Add(forager));
        }

        [Test]
        public void AddShouldReturnFalseWhenEmpty()
        {
            Forager forager = new();
            forager.FirstName = "";
            forager.LastName = "";
            forager.State = "";

            Assert.IsFalse(foragerService.Add(forager));
        }
    }
}
