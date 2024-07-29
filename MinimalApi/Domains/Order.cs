using MongoDB.Bson.Serialization.Attributes;

namespace MinimalApi.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Date")]
        public DateOnly? Date { get; set;}

        [BsonElement("Status")]
        public bool? Status { get; set;}

        [BsonElement("ProductId"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public List<string>? ProdcutId { get; set;}


        [BsonElement("ClientId"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? ClientId { get; set; }


    }
}
