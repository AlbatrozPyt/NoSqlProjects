using MongoDB.Bson.Serialization.Attributes;

namespace MinimalApi.Domains
{
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        public string? Nome { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        public User()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }

        public Dictionary<string, string> AdditionalAttributes { get; set; }
    }
}
