using System.Text;

namespace PharmacyAccountingSystem
{
    internal class CommandsController
    {
        private readonly DatabaseContext _databaseContext;
        private readonly Dictionary<CommandType, Func<Dictionary<string, string>, bool>> _commandMethods = new();

        public CommandsController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;

            _commandMethods.Add(CommandType.CreateProduct, CreateProduct);
            _commandMethods.Add(CommandType.DeleteProduct, DeleteProduct);
            _commandMethods.Add(CommandType.CreatePharmacy, CreatePharmacy);
            _commandMethods.Add(CommandType.DeletePharmacy, DeletePharmacy);
            _commandMethods.Add(CommandType.CreateWarehouse, CreateWarehouse);
            _commandMethods.Add(CommandType.DeleteWarehouse, DeleteWarehouse);
            _commandMethods.Add(CommandType.CreateBatch, CreateBatch);
            _commandMethods.Add(CommandType.DeleteBatch, DeleteBatch);
            _commandMethods.Add(CommandType.ShowProducts, ShowProducts);
        }

        public bool RunCommand(CommandInfo commandInfo) => _commandMethods[commandInfo.Command](commandInfo.Parameters);

        private bool CreateProduct(Dictionary<string, string> parameters) 
        { 
            return _databaseContext.AddProduct(
                new Product 
                { 
                    ProductUserId = parameters[nameof(Product.ProductUserId)],
                    Name = parameters[nameof(Product.Name)] 
                }); 
        }
        private bool DeleteProduct(Dictionary<string, string> parameters)
        {
            if (_databaseContext.GetProductById(parameters[nameof(Product.ProductUserId)]) == null)
            {
                MessagesLogger.ErrorMessage("Товара с данным id не существует");
                return false;
            }
            
            return _databaseContext.DeleteProduct(new Product { ProductUserId = parameters[nameof(Product.ProductUserId)] });
        }

        private bool CreatePharmacy(Dictionary<string, string> parameters)
        {
            return _databaseContext.AddPharmacy(
                new Pharmacy
                {
                    Name = parameters[nameof(Pharmacy.Name)],
                    Address = parameters[nameof(Pharmacy.Address)],
                    PhoneNumber = parameters[nameof(Pharmacy.PhoneNumber)],
                    PharmacyUserId = parameters[nameof(Pharmacy.PharmacyUserId)],
                });
        }

        private bool DeletePharmacy(Dictionary<string, string> parameters)
        {
            if (_databaseContext.GetPharmacyById(parameters[nameof(Pharmacy.PharmacyUserId)]) == null)
            {
                MessagesLogger.ErrorMessage("Аптеки с данным id не существует");
                return false;
            }

            return _databaseContext.DeletePharmacy(
                new Pharmacy
                {
                    PharmacyUserId = parameters[nameof(Pharmacy.PharmacyUserId)],
                });
        }

        private bool CreateWarehouse(Dictionary<string, string> parameters)
        {
            var pharmacy = _databaseContext.GetPharmacyById(parameters[nameof(Warehouse.PharmacyUserId)]);
            if (pharmacy == null)
            {
                MessagesLogger.ErrorMessage("Аптеки с данным id не существует");
                return false;
            }

            return _databaseContext.AddWarehouse(
                new Warehouse
                {
                    Name = parameters[nameof(Warehouse.Name)],
                    WarehouseUserId = parameters[nameof(Warehouse.WarehouseUserId)],
                    PharmacyId = pharmacy.Id,
                    PharmacyUserId = pharmacy.PharmacyUserId
                });
        }

        private bool DeleteWarehouse(Dictionary<string, string> parameters)
        {
            if (_databaseContext.GetWarehouseById(parameters[nameof(Warehouse.WarehouseUserId)]) == null)
            {
                MessagesLogger.ErrorMessage("Склада с данным id не существует");
                return false;
            }

            return _databaseContext.DeleteWarehouse(
                new Warehouse
                {
                    WarehouseUserId = parameters[nameof(Warehouse.WarehouseUserId)],
                });
        }

        private bool CreateBatch(Dictionary<string, string> parameters)
        {
            if (!int.TryParse(parameters[nameof(Batch.Number)], out int number) || number <= 0)
            {
                MessagesLogger.ErrorMessage("Значение параметра <количество товара> не является положительным числом");
                return false;
            }

            var product = _databaseContext.GetProductById(parameters[nameof(Batch.ProductUserId)]);
            if (product == null)
            {
                MessagesLogger.ErrorMessage("Товара с данным id не существует");
                return false;
            }

            var warehouse = _databaseContext.GetWarehouseById(parameters[nameof(Batch.WarehouseUserId)]);
            if (warehouse == null)
            {
                MessagesLogger.ErrorMessage("Склада с данным id не существует");
                return false;
            }

            return _databaseContext.AddBatch(
                new Batch
                {
                    ProductUserId = parameters[nameof(Batch.ProductUserId)],
                    WarehouseUserId = parameters[nameof(Batch.WarehouseUserId)],
                    BatchUserId = parameters[nameof(Batch.BatchUserId)],
                    Number = number,
                    WarehouseId = warehouse.Id,
                    ProductId = product.Id
                });
        }

        private bool DeleteBatch(Dictionary<string, string> parameters)
        {
            if (_databaseContext.GetBatchById(parameters[nameof(Batch.BatchUserId)]) == null)
            {
                MessagesLogger.ErrorMessage("Партии с данным id не существует");
                return false;
            }

            return _databaseContext.DeleteBatch(
                new Batch
                {
                    BatchUserId = parameters[nameof(Batch.BatchUserId)],
                });
        }

        private bool ShowProducts(Dictionary<string, string> parameters)
        {
            var id = parameters[nameof(Pharmacy.PharmacyUserId)];
            if (_databaseContext.GetPharmacyById(id) == null)
            {
                MessagesLogger.ErrorMessage("Аптеки с данным id не существует");
                return false;
            }

            var warehouses = _databaseContext.GetWarehousesByPharmacyId(id);
            if (!(warehouses?.Any() ?? false))
            {
                MessagesLogger.ErrorMessage("У данной аптеки нет складов");
                return false;
            }
            else
            {
                var batchesInWarehouses = new Dictionary<Warehouse, IEnumerable<Batch>>();
                foreach (var warehouse in warehouses)
                {
                    var batches = _databaseContext.GetBatchesByWarehouseId(warehouse.WarehouseUserId!);
                    batchesInWarehouses.Add(warehouse, batches);
                }

                var products = _databaseContext.GetProductsByIdList(batchesInWarehouses.SelectMany(b => b.Value).Select(b => b.ProductUserId!));
                ShowProductsList(batchesInWarehouses, products);
            }

            return true;
        }

        private static void ShowProductsList(Dictionary<Warehouse, IEnumerable<Batch>> batchesInWarehouses, IEnumerable<Product> products)
        {
            var productsTotal = new StringBuilder("Всего:");
            productsTotal.AppendLine();

            foreach (var product in products)
            {
                var p = $"{product.Name}: {batchesInWarehouses.Values.SelectMany(bl => bl).Where(b => b.ProductId == product.Id).Select(b => b.Number).Sum()}";
                productsTotal.AppendLine(p);
            }

            MessagesLogger.CustomMessage(productsTotal.ToString());

            var productsInWarehouse = new StringBuilder();
            foreach (var warehouse in batchesInWarehouses)
            {
                productsInWarehouse.AppendLine($"В складе '{warehouse.Key.Name}' ({warehouse.Key.WarehouseUserId}):");
                foreach (var product in products)
                {
                    var p = $"{product.Name} ({product.ProductUserId}): {warehouse.Value.Where(b => b.ProductId == product.Id).Select(b => b.Number).Sum()}";
                    productsInWarehouse.AppendLine(p);
                }

                MessagesLogger.CustomMessage(productsInWarehouse.ToString());
                productsInWarehouse.Clear();
            }
        }
    }
}
