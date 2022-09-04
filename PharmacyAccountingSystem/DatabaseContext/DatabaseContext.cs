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
        }

        public bool AddProduct(Product product) => _productOperationsProvider.AddProduct(product);
        public bool DeleteProduct(Product product) => _productOperationsProvider.DeleteProduct(product);

        public bool AddPharmacy(Pharmacy pharmacy) => _pharmacyOperationsProvider.AddPharmacy(pharmacy);
        public bool DeletePharmacy(Pharmacy pharmacy) => _pharmacyOperationsProvider.DeletePharmacy(pharmacy);

        public bool AddWarehouse(Warehouse warehouse) => _warehouseOperationsProvider.AddWarehouse(warehouse);
        public bool DeleteWarehouse(Warehouse warehouse) => _warehouseOperationsProvider.DeleteWarehouse(warehouse);

        public bool AddBatch(Batch batch) => _batchOperationsProvider.AddBatch(batch);
        public bool DeleteBatch(Batch batch) => _batchOperationsProvider.DeleteBatch(batch);

        private string LoadConnectionString(string id = "Default")
            => ConfigurationManager.ConnectionStrings[id].ConnectionString;

    }
}
