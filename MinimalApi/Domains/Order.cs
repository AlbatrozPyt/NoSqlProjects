using MongoDB.Bson.Serialization.Attributes;

namespace MinimalApi.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Date")]
        public DateOnly? Date { get; set; }

        [BsonElement("Status")]
        public bool? Status { get; set; }


        // referencia dos produtos

        [BsonElement("ProductId")]
        public List<string>? ProductId { get; set; }

        public List<Product>? Products { get; set; }


        // referencia do cliente

        [BsonElement("ClientId")]
        public string? ClientId { get; set; }

        public Client? Client { get; set; }




    }
}
