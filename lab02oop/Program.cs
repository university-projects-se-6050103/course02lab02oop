using System;
using System.Text;

namespace lab02oop {
    class Program {
        public static readonly Random Random = new Random();

        private static void Main() {
            InitUkrainianSupport();
            //Database.PopulateWithRandomData();
            Database.PrintCrmStats();
            while (true) {
                StartSearch();
            }
        }

        private static void InitUkrainianSupport() {
            Console.OutputEncoding = Encoding.GetEncoding("Cyrillic");
            Console.InputEncoding = Encoding.GetEncoding("Cyrillic");
        }

        private static void StartSearch() {
            Console.WriteLine("\nВведiть пошуковий запит: ");
            var searchQuery = Console.ReadLine();
            new Search(searchQuery).PerformSearch();
        }
    }
}
