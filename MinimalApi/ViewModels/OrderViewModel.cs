namespace MinimalApi.ViewModels
{
    public class OrderViewModel
    {
        public string? Id { get; set; }

        public DateOnly? Date { get; set; }

        public bool? Status { get; set; }

        public List<string>? ProductId { get; set; }

        public string? ClientId { get; set; }
    }
}
