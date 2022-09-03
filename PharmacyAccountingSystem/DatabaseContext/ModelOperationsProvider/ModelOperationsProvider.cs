using System.Data.SQLite;

namespace PharmacyAccountingSystem
{
    internal class ModelOperationsProvider<T> where T : class
    {
        protected const string ENABLE_FOREIGN_KEYS = "PRAGMA foreign_keys = ON;";

        private readonly SQLiteConnection _connection;

        public ModelOperationsProvider(SQLiteConnection connection)
        {
            _connection = connection;
        }

        protected virtual T PopulateRecord(SQLiteDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected virtual void HandleFailedCommand(Exception ex)
        {
        }

        protected IEnumerable<T> GetRecords(SQLiteCommand command)
        {
            var list = new List<T>();
            command.Connection = _connection;
            _connection.Open();

            try
            {
                var reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                        list.Add(PopulateRecord(reader));
                }
                catch (Exception ex)
                {
                    HandleFailedCommand(ex);
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }

            return list;
        }

        protected T? GetRecord(SQLiteCommand command)
        {
            T? record = null;
            command.Connection = _connection;
            _connection.Open();

            try
            {
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        record = PopulateRecord(reader);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    HandleFailedCommand(ex);
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                _connection.Close();
            }

            return record;
        }

        protected void ExecuteCommand(SQLiteCommand command)
        {
            command.Connection = _connection;
            _connection.Open();

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                HandleFailedCommand(ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
