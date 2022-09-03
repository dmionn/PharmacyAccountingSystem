using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAccountingSystem
{
    internal class DatabaseContext
    {
        private readonly SQLiteConnection _connection;

        private readonly ProductOperationsProvider _productOperationsProvider;
        private readonly PharmacyOperationsProvider _pharmacyOperationsProvider;
        private readonly WarehouseOperationsProvider _warehouseOperationsProvider;
        private readonly BatchOperationsProvider _batchOperationsProvider;

        public DatabaseContext()
        {
            _connection = new SQLiteConnection(LoadConnectionString());

            _productOperationsProvider = new ProductOperationsProvider(_connection);
            _pharmacyOperationsProvider = new PharmacyOperationsProvider(_connection);
            _warehouseOperationsProvider = new WarehouseOperationsProvider(_connection);
            _batchOperationsProvider = new BatchOperationsProvider(_connection);

            dosmth();
        }

        private void dosmth()
        {
            //_batchOperationsProvider.AddBatch(new Batch { Name = "партия 1", Number = 100, ProductId = 4, WarehouseId = 3 });
            //_batchOperationsProvider.DeleteBatch(new Batch { Name = "партия 1", Number = 100, ProductId = 4, WarehouseId = 3 });
        }

        private string LoadConnectionString(string id = "Default")
            => ConfigurationManager.ConnectionStrings[id].ConnectionString;

    }
}
