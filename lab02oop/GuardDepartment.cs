using System.Threading.Tasks;
using lab02oop.DBEntities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace lab02oop {
    public class GuardDepartment {
        public static async Task<string> AddCustomer(string name) {
            var customer = new Customer { Name = name };
            var customerBson = customer.ToBsonDocument();
            await Database.Customers.InsertOneAsync(customerBson);
            return customerBson["_id"].ToString();
        }

        public static async Task<string> SignAgreementWith(string customerId) {
            var agreement = new Agreement();
            var agreementBson = agreement.ToBsonDocument();
            await Database.Agreements.InsertOneAsync(agreementBson);
            var agreementId = agreementBson["_id"].ToString();

            var customerFilter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(customerId));
            var customerUpdate = Builders<BsonDocument>.Update.AddToSet("Agreements", agreementId);
            await Database.Customers.UpdateOneAsync(customerFilter, customerUpdate);

            return agreementId;
        }

        public static async Task<string> CallTheGuardBy(string agreementId) {
            var guardCall = new GuardCall();
            var guardCallBson = guardCall.ToBsonDocument();
            await Database.GuardCalls.InsertOneAsync(guardCallBson);
            var guardCallId = guardCallBson["_id"].ToString();

            var agreementFilter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(agreementId));
            var agreementUpdate = Builders<BsonDocument>.Update.AddToSet("GuardCalls", guardCallId);
            await Database.Agreements.UpdateOneAsync(agreementFilter, agreementUpdate);

            return guardCallId;
        }
    }
}