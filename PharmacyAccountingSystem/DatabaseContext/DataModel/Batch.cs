namespace PharmacyAccountingSystem
{
    internal class Batch
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Number { get; set; }
        public string? Name{ get; set; }
        public string? WarehouseName { get; set; }
        public string? ProductName { get; set; }
    }
}
