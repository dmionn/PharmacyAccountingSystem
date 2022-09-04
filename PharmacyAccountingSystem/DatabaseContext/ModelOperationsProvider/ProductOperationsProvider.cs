using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAccountingSystem
{
    internal class ProductOperationsProvider : ModelOperationsProvider<Product>
    {
        private const string NON_UNIQUE_PRODUCT = "constraint failed\r\nUNIQUE constraint failed: Products.Name";

        public ProductOperationsProvider(SQLiteConnection connection) : base(connection)
        {
        }

        public bool AddProduct(Product product)
        {
            using var command = new SQLiteCommand($"INSERT INTO Products (Name) VALUES ('{product.Name}')");
            return ExecuteCommand(command);
        }

        public bool DeleteProduct(Product product)
        {
            using var command = new SQLiteCommand($"DELETE FROM Products WHERE Name='{product.Name}'");
            return ExecuteCommand(command);
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
