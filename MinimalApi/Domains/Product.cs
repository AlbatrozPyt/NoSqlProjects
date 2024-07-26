using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MinimalApi.Domains
{
    public class Product
    {

        public Product()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }

        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        public Dictionary<string, string> AdditionalAttributes { get; set; }

    }
}
