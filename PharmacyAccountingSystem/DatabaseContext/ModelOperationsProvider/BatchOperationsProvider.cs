using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAccountingSystem
{
    internal class BatchOperationsProvider : ModelOperationsProvider<Batch>
    {
        public BatchOperationsProvider(SQLiteConnection connection) : base(connection)
        {
        }

        public void AddWarehouse(Batch batch)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Batches (ProductId, WarehouseId, Number)" +
                $" VALUES ({batch.ProductId}, {batch.WarehouseId}, {batch.Number});");

            ExecuteCommand(command);
        }

        public void DeleteWarehouse(Batch batch)
        {
            //using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Batches WHERE Name='{batch.}';");
            //ExecuteCommand(command);
        }
    }
}
