using SustainableForaging.Core.Exceptions;
using SustainableForaging.Core.Models;
using SustainableForaging.Core.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SustainableForaging.DAL
{
    public class ForagerFileRepository : IForagerRepository
    {
        private const string HEADER = "id,first_name,last_name,state";
        private readonly string filePath;

        public ForagerFileRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public bool Add(Forager forager)
        {
            try
            {
                List<Forager> foragers = FindAll();
                forager.Id = Guid.NewGuid().ToString();
                int beforeAdd = foragers.Count;
                foragers.Add(forager);
                int afterAdd = foragers.Count;
                Write(foragers);
                return beforeAdd != afterAdd;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<Forager> FindAll()
        {
            var foragers = new List<Forager>();
            if(!File.Exists(filePath))
            {
                return foragers;
            }

            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch(IOException ex)
            {
                throw new RepositoryException("could not read foragers", ex);
            }

            for(int i = 1; i < lines.Length; i++) // skip the header
            {
                string[] fields = lines[i].Split(",", StringSplitOptions.TrimEntries);
                Forager forager = Deserialize(fields);
                if(forager != null)
                {
                    foragers.Add(forager);
                }
            }
            return foragers;
        }

        public Forager FindById(string id)
        {
            return FindAll().FirstOrDefault(i => i.Id == id);
        }

        public List<Forager> FindByState(string stateAbbr)
        {
            return FindAll()
                .Where(i => i.State == stateAbbr)
                .ToList();
        }

        private string Serialize(Forager forager)
        {
            return string.Format("{0},{1},{2},{3}",
                    forager.Id,
                    forager.FirstName,
                    forager.LastName,
                    forager.State);
        }

        private Forager Deserialize(string[] fields)
        {
            if(fields.Length != 4)
            {
                return null;
            }

            Forager result = new Forager();
            result.Id = fields[0];
            result.FirstName = fields[1];
            result.LastName = fields[2];
            result.State = fields[3];
            return result;
        }

        private void Write(List<Forager> foragers)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(filePath, false);
                writer.WriteLine(HEADER);

                if (foragers.ToString() == null)
                {
                    return;
                }

                foreach (var forager in foragers)
                {
                    writer.WriteLine(Serialize(forager));
                }
            }
            catch (IOException ex)
            {
                throw new RepositoryException("could not write forages", ex);
            }
        }
    }
}
