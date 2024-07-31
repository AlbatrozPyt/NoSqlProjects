using MinimalApi.Domains;

namespace MinimalApi.ViewModels
{
    public class GetOrderViewModel
    {
        public string? Id { get; set; }

        public DateOnly? Date { get; set; }

        public string? Status { get; set; }

        public Client? Client { get; set; }

        public List<Product>? Products { get; set; }
    }
}
