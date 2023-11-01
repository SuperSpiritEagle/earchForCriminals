using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchForCriminals
{
    class Program
    {
        static void Main(string[] args)
        {
            const string BeginSearchingCommand = "1";
            const string ShowAllCriminalsCommand = "2";
            const string ExitCommand = "3";

            CriminalBase criminalBase = new CriminalBase();
            bool isWorking = true;

            while (isWorking)
            {
                string userInput = EditTools.GetInput($"{BeginSearchingCommand} - Начать поиск\n" +
                                                      $"{ShowAllCriminalsCommand} - Показать всех преступников\n" +
                                                      $"{ExitCommand} - Выход\n" +
                                                      "\nВведите номер команды: ");

                switch (userInput)
                {
                    case BeginSearchingCommand:
                        criminalBase.SearchCriminal();
                        break;

                    case ShowAllCriminalsCommand:
                        criminalBase.ShowAllCriminals();
                        break;

                    case ExitCommand:
                        isWorking = false;
                        break;

                    default:
                        EditTools.ShowMessage("\nВведено некорректное значение!");
                        break;
                }
            }
        }
    }

    class CriminalBase
    {
        private List<Criminal> _criminals;

        public CriminalBase()
        {
            _criminals = new List<Criminal>
            {
                new Criminal("Иванов Иван Иванович", "Русский"), new Criminal("Мишустин Алексей Петрович", "Русский"),
                new Criminal("Воронов Алексей Алексеевич", "Русский"), new Criminal("Пригожин Евгений Боеприпасович", "Русский"),
                new Criminal("Григорян Галуст Арменович", "Армянин"), new Criminal("Азарян Армен Левонович", "Армянин"),
                new Criminal("Ниношвили Давид", "Грузин"), new Criminal("Давиташвили Омар", "Грузин")
            };
        }

        public void SearchCriminal()
        {
            string userHeight = EditTools.GetInput("\nВведите рост: ");
            string userWeight = EditTools.GetInput("Введите вес: ");
            string userNationality = EditTools.GetInput("Введите национальность: ");
            bool isArrested = IsArrested();

            IEnumerable<Criminal> selectedCriminals = GetSelectedCriminals(userHeight, userWeight, userNationality, isArrested);

            if (selectedCriminals.ToList().Count == 0)
            {
                EditTools.ShowMessage("\nНе найдено!");
                return;
            }

            ShowCriminals(selectedCriminals.ToList());
        }

        public void ShowAllCriminals() => ShowCriminals(_criminals);

        private void ShowCriminals(List<Criminal> criminals)
        {
            Console.Clear();

            foreach (Criminal criminal in criminals)
                criminal.ShowProperties();

            EditTools.WaitAndClear();
        }

        private bool IsArrested()
        {
            const string YesCommand = "1";
            const string NoCommand = "2";

            bool isWorking = true;
            bool isArrested = false;

            while (isWorking)
            {
                Console.Clear();
                string userInput = EditTools.GetInput($"{YesCommand} - Арестованный\n" +
                                                      $"{NoCommand} - Освобождённый\n" +
                                                      "\nВведите номер, чтобы выбрать нужную характеристику для поиска: ");

                switch (userInput)
                {
                    case YesCommand:
                        isArrested = true;
                        isWorking = false;
                        break;

                    case NoCommand:
                        isArrested = false;
                        isWorking = false;
                        break;

                    default:
                        EditTools.ShowMessage("\nВведено некорректное значение!");
                        break;
                }
            }

            return isArrested;
        }

        private IEnumerable<Criminal> GetSelectedCriminals(string height, string weight, string nationality, bool isArrested) => _criminals.Where(criminal => Convert.ToString(criminal.Height) == height &&
                                                                                                                                 Convert.ToString(criminal.Weight) == weight &&
                                                                                                                                 criminal.Nationality.ToLower() == nationality.ToLower() &&
                                                                                                                            criminal.IsUnderArrest == isArrested);
    }

    class Criminal
    {
        private string _fullName;

        public Criminal(string fullName, string nationality)
        {
            int minHeight = 160;
            int maxHeight = 201;

            int minWeight = 40;
            int maxWeight = 201;

            _fullName = (fullName.Trim() == "") ? "Неизвестно" : fullName.Trim();
            Nationality = (nationality.Trim() == "") ? "Неизвестно" : nationality.Trim();
            Height = EditTools.GetRandomNumber(minHeight, maxHeight);
            Weight = EditTools.GetRandomNumber(minWeight, maxWeight);
            IsUnderArrest = EditTools.FlipCoin();
        }

        public string Nationality { get; private set; }
        public int Height { get; private set; }
        public int Weight { get; private set; }
        public bool IsUnderArrest { get; private set; }

        public void ShowProperties() => Console.WriteLine($"Имя: {_fullName}\n" +
                                                          $"Национальность: {Nationality}\n" +
                                                          $"Рост: {Height}\n" +
                                                          $"Вес: {Weight}\n");
    }

    static class EditTools
    {
        private static readonly Random _random = new Random();

        public static void ShowMessage(string message)
        {
            Console.Write(message);
            WaitAndClear();
        }

        public static void WaitAndClear()
        {
            Console.ReadKey();
            Console.Clear();
        }

        public static string GetInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine().Trim();
        }

        public static bool FlipCoin()
        {
            int tails = 2;

            return GetRandomNumber(tails) > 0;
        }

        public static int GetRandomNumber(int maxRandomValue) => _random.Next(maxRandomValue);
        public static int GetRandomNumber(int minRandomValue, int maxRandomValue) => _random.Next(minRandomValue, maxRandomValue);
    }
}
