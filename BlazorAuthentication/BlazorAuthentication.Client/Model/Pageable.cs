namespace BlazorAuthentication.Client.Model
{
    public class Pageable
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public int? Direction { get; set; }
    }
}
