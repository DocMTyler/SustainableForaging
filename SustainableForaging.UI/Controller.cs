using SustainableForaging.BLL;
using SustainableForaging.Core.Exceptions;
using SustainableForaging.Core.Models;
using System;
using System.Collections.Generic;

namespace SustainableForaging.UI
{
    public class Controller
    {
        private readonly ForagerService foragerService;
        private readonly ForageService forageService;
        private readonly ItemService itemService;
        private readonly View view;

        public Controller(ForagerService foragerService, ForageService forageService, ItemService itemService, View view)
        {
            this.foragerService = foragerService;
            this.forageService = forageService;
            this.itemService = itemService;
            this.view = view;
        }

        public void Run()
        {
            view.DisplayHeader("Welcome to Sustainable Foraging");
            try
            {
                RunAppLoop();
            }
            catch(RepositoryException ex)
            {
                view.DisplayException(ex);
            }
            view.DisplayHeader("Goodbye.");
        }

        private void RunAppLoop()
        {
            MainMenuOption option;
            do
            {
                option = view.SelectMainMenuOption();
                switch(option)
                {
                    case MainMenuOption.ViewForagesByDate:
                        ViewByDate();
                        break;
                    case MainMenuOption.ViewItems:
                        ViewItems();
                        break;
                    case MainMenuOption.AddForage:
                        AddForage();
                        break;
                    case MainMenuOption.AddForager:
                        AddForager();
                        break;
                    case MainMenuOption.ViewForagers:
                        ViewForagers();
                        view.EnterToContinue();
                        break;
                    case MainMenuOption.AddItem:
                        AddItem();
                        break;
                    case MainMenuOption.ReportKgPerItem:
                        StatsKgPerItem();
                        break;
                    case MainMenuOption.ReportCategoryValue:
                        StatsValuePerCategory();
                        break;
                    case MainMenuOption.Generate:
                        Generate();
                        break;
                }
            } while(option != MainMenuOption.Exit);
        }

        // top level menu

        private void ViewByDate()
        {
            DateTime date = view.GetForageDate();
            List<Forage> forages = forageService.FindByDate(date);
            view.DisplayForages(forages);
            view.EnterToContinue();
        }

        private void ViewItems()
        {
            view.DisplayHeader(MainMenuOption.ViewItems.ToLabel());
            Category category = view.GetItemCategory();
            List<Item> items = itemService.FindByCategory(category);
            view.DisplayHeader("Items");
            view.DisplayItems(items);
            view.EnterToContinue();
        }

        public void ViewForagers()
        {
            Console.Clear();
            view.DisplayHeader(MainMenuOption.ViewForagers.ToLabel());
            string lastNamePrefix = view.GetForagerNamePrefix();
            List<Forager> foragers = foragerService.FindByLastName(lastNamePrefix);
            view.DisplayForagers(foragers);
        }

        private void AddForager()
        {
            view.DisplayHeader(MainMenuOption.AddForager.ToLabel());
            Forager forager = view.MakeForager();
            Console.WriteLine(foragerService.Add(forager) ? "Forager added" : "Forager not added");
        }

        private void AddForage()
        {
            view.DisplayHeader(MainMenuOption.AddForage.ToLabel());
            Forager forager = GetForager();
            if(forager == null)
            {
                return;
            }
            Item item = GetItem();
            if(item == null)
            {
                return;
            }
            Forage forage = view.MakeForage(forager, item);
            Result<Forage> result = forageService.Add(forage);
            if(!result.Success)
            {
                view.DisplayStatus(false, result.Messages);
            }
            else
            {
                string successMessage = $"Forage {result.Value.Id} created.";
                view.DisplayStatus(true, successMessage);
            }
        }

        private void AddItem()
        {
            Item item = view.MakeItem();
            Result<Item> result = itemService.Add(item);
            if(!result.Success)
            {
                view.DisplayStatus(false, result.Messages);
            }
            else
            {
                string successMessage = $"Item {result.Value.Id} created.";
                view.DisplayStatus(true, successMessage);
            }
        }

        private void StatsKgPerItem()
        {
            DateTime date = view.GetKgPerItem();
            List<Forage> forages = forageService.FindByDate(date);
            Dictionary<Item, decimal?> kgOfItems = forageService.KilogramPerItemStats(forages);
            view.DisplayStatsKgPerItem(kgOfItems);
            view.EnterToContinue();
        }

        private void StatsValuePerCategory()
        {
            DateTime date = view.GetValuePerCategory();
            List<Forage> forages = forageService.FindByDate(date);
            Dictionary<Category, decimal?> valuePerCategory = forageService.ValuePerCategory(forages);
            view.DisplayStatsValuePerCategory(valuePerCategory);
            view.EnterToContinue();
        }

        private void Generate()
        {
            ForageGenerationRequest request = view.GetForageGenerationRequest();
            if(request != null)
            {
                int count = forageService.Generate(request.Start, request.End, request.Count);
                view.DisplayStatus(true, $"{count} forages generated.");
            }
        }

        // support methods
        private Forager GetForager()
        {
            string lastNamePrefix = view.GetForagerNamePrefix();
            List<Forager> foragers = foragerService.FindByLastName(lastNamePrefix);
            return view.ChooseForager(foragers);
        }

        private Item GetItem()
        {
            Category category = view.GetItemCategory();
            List<Item> items = itemService.FindByCategory(category);
            return view.ChooseItem(items);
        }
    }
}
