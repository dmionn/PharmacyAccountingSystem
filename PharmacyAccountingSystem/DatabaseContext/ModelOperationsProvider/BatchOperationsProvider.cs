using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class BatchOperationsProvider : ModelOperationsProvider<Batch>
    {
        private const string NON_UNIQUE_BATCH_ID = "constraint failed\r\nUNIQUE constraint failed: Batches.BatchUserId";

        public BatchOperationsProvider(SQLiteConnection connection) : base(connection)
        {
        }

        protected override Batch PopulateRecord(SQLiteDataReader reader)
        {
            return new Batch
            {
                Id = reader.GetInt32(0),
                ProductId = reader.GetInt32(1),
                WarehouseId = reader.GetInt32(2),
                Number = reader.GetInt32(3),
                BatchUserId = reader.GetString(4),
                ProductUserId = reader.GetString(5),
                WarehouseUserId = reader.GetString(6),
            };
        }

        public bool AddBatch(Batch batch)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Batches (ProductId, WarehouseId, Number, BatchUserId, ProductUserId, WarehouseUserId)" +
                $" VALUES ({batch.ProductId}, {batch.WarehouseId}, {batch.Number}, '{batch.BatchUserId}', '{batch.ProductUserId}', '{batch.WarehouseUserId}');");

            return ExecuteCommand(command);
        }

        public bool DeleteBatch(Batch batch)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Batches WHERE BatchUserId='{batch.BatchUserId}';");
            return ExecuteCommand(command);
        }

        public Batch? GetBatchById(string id)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Batches WHERE BatchUserId='{id}';");
            return GetRecord(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == NON_UNIQUE_BATCH_ID)
            {
                MessagesLogger.ErrorMessage("Партия с таким id уже существует");
            }
        }
    }
}
