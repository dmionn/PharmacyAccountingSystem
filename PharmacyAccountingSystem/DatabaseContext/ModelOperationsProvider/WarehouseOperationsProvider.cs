using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class WarehouseOperationsProvider : ModelOperationsProvider<Warehouse>
    {
        private const string NON_UNIQUE_WAREHOUSE_NAME = "constraint failed\r\nUNIQUE constraint failed: Warehouses.Name";

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
            };
        }

        public bool AddWarehouse(Warehouse warehouse)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Warehouses (PharmacyId, Name) VALUES({warehouse.PharmacyId}, '{warehouse.Name}');");

            return ExecuteCommand(command);
        }

        public bool DeleteWarehouse(Warehouse warehouse)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Warehouses WHERE Name='{warehouse.Name}';");
            return ExecuteCommand(command);
        }

        public Warehouse? GetWarehouseByName(string name)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Warehouses WHERE Name='{name}';");
            return GetRecord(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == NON_UNIQUE_WAREHOUSE_NAME)
            {
                MessagesLogger.ErrorMessage("Склад с таким наименованием уже существует");
            }
        }
    }
}
