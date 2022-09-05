using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class ProductOperationsProvider : ModelOperationsProvider<Product>
    {
        private const string NON_UNIQUE_PRODUCT_ID = "constraint failed\r\nUNIQUE constraint failed: Products.ProductUserId";

        public ProductOperationsProvider(SQLiteConnection connection) : base(connection)
        {
        }

        protected override Product PopulateRecord(SQLiteDataReader reader)
        {
            return new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                ProductUserId = reader.GetString(2)
            };
        }

        public bool AddProduct(Product product)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Products (ProductUserId, Name)" +
                $" VALUES ('{product.ProductUserId}', '{product.Name}');");
            return ExecuteCommand(command);
        }

        public bool DeleteProduct(Product product)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Products WHERE ProductUserId='{product.ProductUserId}';");
            return ExecuteCommand(command);
        }

        public Product? GetProductById(string id)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Products WHERE ProductUserId='{id}';");
            return GetRecord(command);
        }

        public IEnumerable<Product> GetProductsByIdList(IEnumerable<string> idList)
        {
            var formattedIdList = string.Join(", ", idList.Select(i => $"'{i}'"));
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Products WHERE ProductUserId in ({formattedIdList});");
            return GetRecords(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == NON_UNIQUE_PRODUCT_ID)
            {
                MessagesLogger.ErrorMessage("Товар с таким id уже существует");
            }
        }
    }
}
