namespace BlazorAuthentication.Client.Model
{
    public class ListFilialResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Cnpj { get; set; } = null!;
        public string Address { get; set; } = null!;
        public Guid IdCompany { get; set; }
        public string Description { get; set; } = null!;
    }
}
