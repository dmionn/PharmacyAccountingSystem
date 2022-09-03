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

        public void AddWarehouse(Warehouse warehouse)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Warehouses (PharmacyId, Name) VALUES({warehouse.PharmacyId}, '{warehouse.Name}');");

            ExecuteCommand(command);
        }

        public void DeleteWarehouse(Warehouse warehouse)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Warehouses WHERE Name='{warehouse.Name}';");
            ExecuteCommand(command);
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
