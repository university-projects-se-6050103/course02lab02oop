using System;
using lab02oop.DBEntities;

namespace lab02oop {
    public class Search {
        private readonly string _query;

        public Search(string query) {
            _query = query;
        }

        public void PerformSearch() {
            if (_query.Contains("громадянин")) {
                if (_query.Contains("найбільший штраф")) {
                    Customer.SearchByTopFine();
                } else {
                    var customerName = ExtractSearchPhrase("громадянин", _query);
                    Customer.SearchByName(customerName);
                    return;
                }
            }
            if (_query.Contains("договір")) {
                if (_query.Contains("на квартиру")) {
                    var apartmentNumber = int.Parse(ExtractSearchPhrase("договір на квартиру", _query));
                    Agreement.SearchByApartment(apartmentNumber);
                    return;
                }
                if (_query.Contains("на імя")) {
                    var customerName = ExtractSearchPhrase("договір на імя", _query);
                    Agreement.SearchByName(customerName);
                    return;
                }
            }
            if (_query.Contains("виклик")) {
                var callDate = ExtractSearchPhrase("виклик", _query);
                GuardCall.SearchByDate(callDate);
                return;
            }
            if (_query.Contains("довідка")) {
                Console.WriteLine("Приклади запитів:");
                Console.WriteLine("громадянин <імя>");
                Console.WriteLine("громадянин найбільший штраф");
                Console.WriteLine("договір на квартиру <номер_квартири>");
                Console.WriteLine("договір на імя <імя_громадянина>");
                Console.WriteLine("виклик <дата> (напр. 2015-09-04)");
                Console.WriteLine("вийти");
                return;
            }
            if (_query.Contains("вийти")) {
                Environment.Exit(1);
            }
        }

        private static string ExtractSearchPhrase(string keyword, string searchQuery) {
            return searchQuery.Split(new[] { keyword }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
        }
    }
}