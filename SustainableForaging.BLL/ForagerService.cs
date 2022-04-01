using SustainableForaging.Core.Models;
using SustainableForaging.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SustainableForaging.BLL
{
    public class ForagerService
    {
        private readonly IForagerRepository repository;

        public ForagerService(IForagerRepository repository)
        {
            this.repository = repository;
        }

        //REPORTS IN BLL

        public bool Add(Forager forager)
        {
            if (String.IsNullOrEmpty(forager.ToString()))
            {
                Console.WriteLine("Cannot add an empty forager");
                return false;
            }

            List<Forager> foragers = repository.FindAll();
            if (foragers.Any(f => f.FirstName == forager.FirstName && f.LastName == forager.LastName && f.State == forager.State))
            {
                Console.WriteLine("Cannot add a duplicate forager");
                return false;
            }

            return repository.Add(forager);
        }
        
        public List<Forager> FindByState(string stateAbbr)
        {
            return repository.FindByState(stateAbbr);
        }

        public List<Forager> FindByLastName(string prefix)
        {
            return repository.FindAll()
                    .Where(i => i.LastName.StartsWith(prefix))
                    .ToList();
        }
    }
}
