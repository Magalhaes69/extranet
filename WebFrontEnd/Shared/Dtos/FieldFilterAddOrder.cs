namespace WebFrontEnd.Shared.Dtos
{
    public sealed class FieldFilterAddOrder
    {
         [Filterable(LabelKey = "itemCode")]
        public string ItemCode { get; set; }
        [Filterable(LabelKey = "itemDescription")]
        public string ItemName { get; set; }
        [Filterable(LabelKey = "code")]
        public string Code { get; set; }
        [Filterable(LabelKey = "name")]
        public string Name { get; set; }

        [Filterable(LabelKey = "WarehouseName")]
        public string WarehouseName { get; set; }
        [Filterable(LabelKey = "WarehouseCode")]
        public string WarehouseCode { get; set; }
        [Filterable(LabelKey = "location")]
        public string Location { get; set; }
    }
}
