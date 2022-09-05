namespace PharmacyAccountingSystem
{
    internal class Batch
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Number { get; set; }
        public string? BatchUserId { get; set; }
        public string? ProductUserId { get; set; }
        public string? WarehouseUserId { get; set; }
    }
}
