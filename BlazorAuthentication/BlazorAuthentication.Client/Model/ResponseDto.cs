namespace BlazorAuthentication.Client.Model
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
}
