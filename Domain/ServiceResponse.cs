namespace Domain
{
    public class ServiceResponse<T>
    {
        public bool Flag { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
