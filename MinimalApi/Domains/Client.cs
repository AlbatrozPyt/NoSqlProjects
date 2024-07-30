using MongoDB.Bson.Serialization.Attributes;

namespace MinimalApi.Domains
{
    public class Client
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("User_id")]
        public string? UserId { get; set; }


        [BsonElement("Cpf")]
        public string? Cpf { get; set; }

        [BsonElement("Phone")]
        public string? Phone { get; set; }

        [BsonElement("Address")]
        public string? Address { get; set; }

        public Client()
        {
            AdditionalAtributes = new Dictionary<string, string>();
        }

        public Dictionary<string, string> AdditionalAtributes { get; set; }
    }
}
