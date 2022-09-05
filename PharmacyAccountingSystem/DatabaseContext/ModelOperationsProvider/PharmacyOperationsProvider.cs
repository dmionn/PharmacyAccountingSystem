﻿using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class PharmacyOperationsProvider : ModelOperationsProvider<Pharmacy>
    {
        private const string NON_UNIQUE_PHARMACY_NAME = "constraint failed\r\nUNIQUE constraint failed: Pharmacies.Name";

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
            };
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

        public Pharmacy? GetPharmacyByName(string name)
        {
            using var command = new SQLiteCommand($"{ENABLE_FOREIGN_KEYS}SELECT * FROM Pharmacies WHERE Name='{name}';");
            return GetRecord(command);
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
