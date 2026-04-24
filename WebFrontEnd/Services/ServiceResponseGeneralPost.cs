namespace WebFrontEnd.Services
{
    public class ServiceResponseGeneralPost <T>
    {
        public bool Flag { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<T>? Datalist { get; set; }
    }
}
