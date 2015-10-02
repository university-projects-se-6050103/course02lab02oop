using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace lab02oop {
    public class Database {
        public static IMongoCollection<BsonDocument> GuardCalls;
        public static IMongoCollection<BsonDocument> Agreements;
        public static IMongoCollection<BsonDocument> Customers;

        static Database() {
            var client = new MongoClient(new MongoUrl("mongodb://vlad:vlad@ds055752.mongolab.com:55752/lab02oop"));
            var database = client.GetDatabase("lab02oop");

            GuardCalls = database.GetCollection<BsonDocument>("guard_calls");
            Agreements = database.GetCollection<BsonDocument>("agreements");
            Customers = database.GetCollection<BsonDocument>("customers");
        }

        public static async void PopulateWithRandomData() {
            var customerId = await GuardDepartment.AddCustomer(new[] {"Влад", "Ашот", "Русик"}[Program.Random.Next(3)]);
            RunRandomlyTimes(async i => {
                var agreementId = await GuardDepartment.SignAgreementWith(customerId);
                RunRandomlyTimes(async j => {
                    var guardCallId = await GuardDepartment.CallTheGuardBy(agreementId);
                });
            });
            Console.WriteLine("Finihed populationg \n\n\n");
        }

        private static void RunRandomlyTimes(Action<int> action) {
            Enumerable.Range(0, new Random(DateTime.Now.Millisecond).Next(1, 10)).ToList().ForEach(action);
        }

        public static void PrintCrmStats() {
            var collectionsStats = GetCollectionsStats();
            Console.WriteLine("Пошук серед {0} громадян, {1} договорів та {2} викликів служби охорони",
                collectionsStats["Customers"], collectionsStats["Agreements"], collectionsStats["GuardCalls"]);
        }

        private static IDictionary<string, int> GetCollectionsStats() {
            return new Dictionary<string, int> {
                {"Customers", (int) Customers.CountAsync(new BsonDocument()).Result},
                {"Agreements", (int) Agreements.CountAsync(new BsonDocument()).Result},
                {"GuardCalls", (int) GuardCalls.CountAsync(new BsonDocument()).Result}
            };
        }
    }
}