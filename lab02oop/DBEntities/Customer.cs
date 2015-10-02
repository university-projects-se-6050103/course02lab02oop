using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace lab02oop.DBEntities {
    public class Customer {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<Agreement> Agreements { get; set; }

        public Customer() {
            Agreements = new List<Agreement>();
        }

        public static async void SearchByName(string name) {
            var searchResults = await Database.Customers.Find(Builders<BsonDocument>.Filter.Eq("Name", name)).ToListAsync();
            searchResults.ForEach(PrintEntry);
        }

        public static async void SearchByTopFine() {
            var topFineAgreement = await Agreement.SearchWithTopFine();
            var topFineAgreementOwner = await Database.Customers.Find(new BsonDocument {
                            {"Agreements", new BsonDocument {{"$elemMatch", new BsonDocument {{"$eq", topFineAgreement["_id"].ToString()}}}}}
                        }).FirstOrDefaultAsync();

            Console.WriteLine("Найбільший штраф у " + topFineAgreementOwner["Name"] + ": " + topFineAgreement["totalFine"]);
        }

        public static void PrintEntry(BsonValue customerDocument) {
            Console.WriteLine(customerDocument["Name"] + " - " + customerDocument["Agreements"].AsBsonArray.Count + " договори");
        }
    }
}