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

        public void AddBatch(Batch batch)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Batches (ProductId, WarehouseId, Number, Name)" +
                $" VALUES ({batch.ProductId}, {batch.WarehouseId}, {batch.Number}, '{batch.Name}');");

            ExecuteCommand(command);
        }

        public void DeleteBatch(Batch batch)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Batches WHERE Name='{batch.Name}';");
            ExecuteCommand(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == "")
            {
                MessagesLogger.ErrorMessage("Партия с таким наименованием уже существует");
            }
        }
    }
}
