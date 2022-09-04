namespace PharmacyAccountingSystem
{
    internal enum CommandType
    {
        Unknown,

        CreateProduct,
        DeleteProduct,

        CreatePharmacy,
        DeletePharmacy,

        CreateWarehouse,
        DeleteWarehouse,

        CreateBatch,
        DeleteBatch,

        ShowProducts,

        Exit,
    }
}
