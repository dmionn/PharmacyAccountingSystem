using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAccountingSystem
{
    internal class PharmacyOperationsProvider : ModelOperationsProvider<Pharmacy>
    {
        private const string NON_UNIQUE_PHARMACY_NAME = "constraint failed\r\nUNIQUE constraint failed: Pharmacies.Name";

        public PharmacyOperationsProvider(SQLiteConnection connection) : base(connection)
        {
        }

        public bool AddPharmacy(Pharmacy pharmacy)
        {
            using var command = new SQLiteCommand($"INSERT INTO Pharmacies (Name, Address, PhoneNumber)" +
                $" VALUES ('{pharmacy.Name}', '{pharmacy.Address}', '{pharmacy.PhoneNumber}')");

            return ExecuteCommand(command);
        }

        public bool DeletePharmacy(Pharmacy pharmacy)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}DELETE FROM Pharmacies WHERE Name='{pharmacy.Name}';");
            return ExecuteCommand(command);
        }

        protected override void HandleFailedCommand(Exception ex)
        {
            if (ex.Message == NON_UNIQUE_PHARMACY_NAME)
            {
                MessagesLogger.ErrorMessage("Аптека с таким наименованием уже существует");
            }
        }
    }
}
