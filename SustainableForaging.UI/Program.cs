using SustainableForaging.BLL;
using SustainableForaging.DAL;
using System;
using System.IO;

namespace SustainableForaging.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleIO io = new ConsoleIO();
            View view = new View(io);

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string forageFileDirectory = Path.Combine(projectDirectory, "data", "forage_data");
            string foragerFilePath = Path.Combine(projectDirectory, "data", "foragers.csv");
            string itemFilePath = Path.Combine(projectDirectory, "data", "items.txt");

            ForageFileRepository forageFileRepository = new ForageFileRepository(forageFileDirectory);
            ForagerFileRepository foragerFileRepository = new ForagerFileRepository(foragerFilePath);
            ItemFileRepository itemFileRepository = new ItemFileRepository(itemFilePath);

            ForagerService foragerService = new ForagerService(foragerFileRepository);
            ForageService forageService = new ForageService(forageFileRepository, foragerFileRepository, itemFileRepository);
            ItemService itemService = new ItemService(itemFileRepository);

            Controller controller = new Controller(foragerService, forageService, itemService, view);
            controller.Run();
        }
    }
}
