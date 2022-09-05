using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class ProductOperationsProvider : ModelOperationsProvider<Product>
    {
        private const string NON_UNIQUE_PRODUCT = "constraint failed\r\nUNIQUE constraint failed: Products.Name";

        public ProductOperationsProvider(SQLiteConnection connection) : base(connection)
        {
        }

        protected override Product PopulateRecord(SQLiteDataReader reader)
        {
            return new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }

        public bool AddProduct(Product product)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Products (Name) VALUES ('{product.Name}');");
            return ExecuteCommand(command);
        }

        public bool DeleteProduct(Product product)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Products WHERE Name='{product.Name}';");
            return ExecuteCommand(command);
        }

        public Product? GetProductByName(string name)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Products WHERE Name='{name}';");
            return GetRecord(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == NON_UNIQUE_PRODUCT)
            {
                MessagesLogger.ErrorMessage("Товар с таким наименованием уже существует");
            }
        }
    }
}
