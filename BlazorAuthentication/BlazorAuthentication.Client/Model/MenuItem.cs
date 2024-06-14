namespace BlazorAuthentication.Client.Model
{
    public class MenuItem
    {
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Text { get; set; }
        public List<MenuItem> SubMenu { get; set; } = new List<MenuItem>();
    }
}
