namespace PharmacyAccountingSystem
{
    internal class Warehouse
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public string? Name { get; set; }
        public string? WarehouseUserId { get; set; }
        public string? PharmacyUserId { get; set; }
    }
}
