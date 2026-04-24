namespace WebFrontEnd.Shared.Dtos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterableAttribute : Attribute
    {
        public string? LabelKey { get; set; }
    }
}
