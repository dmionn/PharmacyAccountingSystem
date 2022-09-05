using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class BatchOperationsProvider : ModelOperationsProvider<Batch>
    {
        private const string NON_UNIQUE_BATCH_NAME = "constraint failed\r\nUNIQUE constraint failed: Batches.Name";

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
                Name = reader.GetString(4),
            };
        }

        public bool AddBatch(Batch batch)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Batches (ProductId, WarehouseId, Number, Name)" +
                $" VALUES ({batch.ProductId}, {batch.WarehouseId}, {batch.Number}, '{batch.Name}');");

            return ExecuteCommand(command);
        }

        public bool DeleteBatch(Batch batch)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Batches WHERE Name='{batch.Name}';");
            return ExecuteCommand(command);
        }

        public Batch? GetBatchByName(string name)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Batches WHERE Name='{name}';");
            return GetRecord(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == NON_UNIQUE_BATCH_NAME)
            {
                MessagesLogger.ErrorMessage("Партия с таким наименованием уже существует");
            }
        }
    }
}
