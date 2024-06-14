namespace BlazorAuthentication.Client.Model
{
    public class Page<T>
    {
        public List<T> Content { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public int Number { get; set; }
        public int Size { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
    }
}
