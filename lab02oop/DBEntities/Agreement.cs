using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace lab02oop.DBEntities {
    public class Agreement {
        public ObjectId Id { get; set; }
        public double LateAlarmDisableFine { get; set; }
        public double PropertyNotSavedFine { get; set; }
        public int ApartmentNumber { get; set; }
        public List<GuardCall> GuardCalls { get; set; }

        public Agreement() {
            LateAlarmDisableFine = Program.Random.NextDouble();
            PropertyNotSavedFine = Program.Random.NextDouble();
            ApartmentNumber = Program.Random.Next(1, 100);
            GuardCalls = new List<GuardCall>();
        }

        public static async Task<BsonDocument> SearchWithTopFine() {
            var totalFineProject = new BsonDocument {
                        {"LateAlarmDisableFine", 1},
                        {"PropertyNotSavedFine", 1},
                        {"totalFine", new BsonDocument {{"$add", new BsonArray(new[] {"$LateAlarmDisableFine", "$PropertyNotSavedFine"})}}}
                    };
            var descendingSort = new BsonDocument { { "totalFine", -1 } };
            return await Database.Agreements.Aggregate().Project(totalFineProject).Sort(descendingSort).FirstOrDefaultAsync();
        }

        public static async void SearchByApartment(int apartmentNumber) {
            var searchResults = await Database.Agreements.Find(Builders<BsonDocument>.Filter.Eq("ApartmentNumber", apartmentNumber)).ToListAsync();
            searchResults.ForEach(PrintEntry);
        }

        public static async void SearchByName(string name) {
            var searchResults = await Database.Customers.Find(Builders<BsonDocument>.Filter.Eq("Name", name)).ToListAsync();
            var customerAgreements = new List<string>();
            foreach (var searchResult in searchResults) {
                customerAgreements.AddRange(searchResult["Agreements"].AsBsonArray.Select(currentCustomerAgrement => currentCustomerAgrement.AsString));
            }
            foreach (var customerAgreementId in customerAgreements) {
                var agreement = (await Database.Agreements.Find(Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(customerAgreementId))).ToListAsync()).ElementAt(0);
                PrintEntry(agreement);
            }
        }

        public static void PrintEntry(BsonDocument agreementDocument) {
            Console.WriteLine("Договiр №" + agreementDocument["ApartmentNumber"] +
                " - " + agreementDocument["GuardCalls"].AsBsonArray.Count + " викликiв - " +
                "Сума штрафів: " + (double.Parse(agreementDocument["LateAlarmDisableFine"].AsDouble.ToString()) +
                double.Parse(agreementDocument["PropertyNotSavedFine"].AsDouble.ToString())));
        }
    }
}