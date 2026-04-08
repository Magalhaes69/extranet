namespace WebFrontEnd.Services
{
    public class SelectItem<T>
    {
        public T Value { get; set; } = default!;
        public string Text { get; set; } = "";
    }
}
