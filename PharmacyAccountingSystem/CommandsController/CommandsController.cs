using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private bool CreateProduct(Dictionary<string, string> parameters) => _databaseContext.AddProduct(new Product { Name = parameters[nameof(Product.Name)] });
        private bool DeleteProduct(Dictionary<string, string> parameters) => _databaseContext.DeleteProduct(new Product { Name = parameters[nameof(Product.Name)] });

        private bool CreatePharmacy(Dictionary<string, string> parameters)
        {
            return _databaseContext.AddPharmacy(
                new Pharmacy
                {
                    Name = parameters[nameof(Pharmacy.Name)],
                    Address = parameters[nameof(Pharmacy.Address)],
                    PhoneNumber = parameters[nameof(Pharmacy.PhoneNumber)],
                });
        }

        private bool DeletePharmacy(Dictionary<string, string> parameters)
        {
            return _databaseContext.AddPharmacy(
                new Pharmacy
                {
                    Name = parameters[nameof(Pharmacy.Name)],
                });
        }
        private bool CreateWarehouse(Dictionary<string, string> parameters)
        {
            return _databaseContext.AddWarehouse(
                new Warehouse
                {
                    Name = parameters[nameof(Warehouse.Name)],
                    PharmacyName = parameters[nameof(Warehouse.PharmacyName)]
                });
        }

        private bool DeleteWarehouse(Dictionary<string, string> parameters)
        {
            return _databaseContext.DeleteWarehouse(
                new Warehouse
                {
                    Name = parameters[nameof(Warehouse.Name)],
                });
        }

        private bool CreateBatch(Dictionary<string, string> parameters)
        {
            if (!int.TryParse(parameters[nameof(Batch.Number)], out int number) || number <= 0)
            {
                MessagesLogger.ErrorMessage("Значение параметра <количество товара> не является положительным числом");
                return false;
            }

            return _databaseContext.AddBatch(
                new Batch
                {
                    Name = parameters[nameof(Batch.Name)],
                    WarehouseName = parameters[nameof(Batch.WarehouseName)],
                    ProductName = parameters[nameof(Batch.ProductName)],
                    Number = number
                });
        }

        private bool DeleteBatch(Dictionary<string, string> parameters)
        {
            return _databaseContext.DeleteBatch(
                new Batch
                {
                    Name = parameters[nameof(Batch.Name)],
                });
        }

        private bool ShowProducts(Dictionary<string, string> parameters)
        {


            return true;
        }
    }
}
