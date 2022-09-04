using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAccountingSystem
{
    internal class WarehouseOperationsProvider : ModelOperationsProvider<Warehouse>
    {
        public WarehouseOperationsProvider(SQLiteConnection connection) : base(connection)
        {
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

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == "")
            {
                MessagesLogger.ErrorMessage("Склад с таким наименованием уже существует");
            }
        }
    }
}
