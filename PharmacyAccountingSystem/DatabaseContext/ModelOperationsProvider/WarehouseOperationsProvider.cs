using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class WarehouseOperationsProvider : ModelOperationsProvider<Warehouse>
    {
        private const string NON_UNIQUE_WAREHOUSE_ID = "constraint failed\r\nUNIQUE constraint failed: Warehouses.WarehouseUserId";

        public WarehouseOperationsProvider(SQLiteConnection connection) : base(connection)
        {
        }

        protected override Warehouse PopulateRecord(SQLiteDataReader reader)
        {
            return new Warehouse
            {
                Id = reader.GetInt32(0),
                PharmacyId = reader.GetInt32(1),
                Name = reader.GetString(2),
                WarehouseUserId = reader.GetString(3),
                PharmacyUserId = reader.GetString(4),
            };
        }

        public bool AddWarehouse(Warehouse warehouse)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Warehouses (PharmacyId, Name, WarehouseUserId, PharmacyUserId)" +
                $" VALUES({warehouse.PharmacyId}, '{warehouse.Name}', '{warehouse.WarehouseUserId}', '{warehouse.PharmacyUserId}');");

            return ExecuteCommand(command);
        }

        public bool DeleteWarehouse(Warehouse warehouse)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Warehouses WHERE WarehouseUserId='{warehouse.WarehouseUserId}';");
            return ExecuteCommand(command);
        }

        public Warehouse? GetWarehouseById(string id)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Warehouses WHERE WarehouseUserId='{id}';");
            return GetRecord(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == NON_UNIQUE_WAREHOUSE_ID)
            {
                MessagesLogger.ErrorMessage("Склад с таким id уже существует");
            }
        }
    }
}
