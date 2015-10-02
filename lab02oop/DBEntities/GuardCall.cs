using System;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Driver;

namespace lab02oop.DBEntities {
    public class GuardCall {
        public ObjectId Id { get; set; }
        public bool IsPropertyWasSaved { get; set; }
        public bool IsAlarmDisabledInTime { get; set; }
        public DateTime CallDate { get; set; }

        public GuardCall() {
            IsPropertyWasSaved = Program.Random.Next(0, 100) > 50;
            IsAlarmDisabledInTime = Program.Random.Next(0, 100) > 50;
            CallDate = GetRandomDate(new DateTime(2015, 08, 01), new DateTime(2015, 10, 01));
        }

        private static DateTime GetRandomDate(DateTime startDate, DateTime endDate) {
            var timeSpan = endDate - startDate;
            var newSpan = new TimeSpan(0, Program.Random.Next(0, (int)timeSpan.TotalMinutes), 0);
            return startDate + newSpan;
        }

        public static async void SearchByDate(string date) {
            var callDateNext = DateTime.ParseExact(date, "yyyy-MM-dd", new DateTimeFormatInfo()) +
                               new TimeSpan(1, 0, 0, 0);
            var searchResults = await Database.GuardCalls.Find(Builders<BsonDocument>.Filter.Eq("CallDate", new BsonDocument {
                    { "$gte", BsonDateTime.Create(date) }, { "$lt", BsonDateTime.Create(callDateNext) }
                })).ToListAsync();
            searchResults.ForEach(PrintEntry);
        }

        public static void PrintEntry(BsonValue guardCallDocument) {
            Console.WriteLine(guardCallDocument["CallDate"] + " - " + (guardCallDocument["IsPropertyWasSaved"].AsBoolean ? "врятовано" : "втрачено") + ", " +
                (guardCallDocument["IsAlarmDisabledInTime"].AsBoolean ? "сигн. вимкн." : "невчасно"));
        }
    }
}