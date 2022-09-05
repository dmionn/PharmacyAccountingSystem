using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class PharmacyOperationsProvider : ModelOperationsProvider<Pharmacy>
    {
        private const string NON_UNIQUE_PHARMACY_ID = "constraint failed\r\nUNIQUE constraint failed: Pharmacies.PharmacyUserId";

        public PharmacyOperationsProvider(SQLiteConnection connection) : base(connection)
        {
        }

        protected override Pharmacy PopulateRecord(SQLiteDataReader reader)
        {
            return new Pharmacy
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Address = reader.GetString(2),
                PhoneNumber = reader.GetString(3),
                PharmacyUserId = reader.GetString(4)
            };
        }

        public bool AddPharmacy(Pharmacy pharmacy)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}INSERT INTO Pharmacies (Name, Address, PhoneNumber, PharmacyUserId)" +
                $" VALUES ('{pharmacy.Name}', '{pharmacy.Address}', '{pharmacy.PhoneNumber}', '{pharmacy.PharmacyUserId}');");

            return ExecuteCommand(command);
        }

        public bool DeletePharmacy(Pharmacy pharmacy)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Pharmacies WHERE PharmacyUserId='{pharmacy.PharmacyUserId}';");
            return ExecuteCommand(command);
        }

        public Pharmacy? GetPharmacyById(string id)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Pharmacies WHERE PharmacyUserId='{id}';");
            return GetRecord(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == NON_UNIQUE_PHARMACY_ID)
            {
                MessagesLogger.ErrorMessage("Аптека с таким id уже существует");
            }
        }
    }
}
