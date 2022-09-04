namespace PharmacyAccountingSystem
{
    class Program
    {
        private static void Main(string[] args)
        {
            ShowDescription();

            var dbContext = new DatabaseContext();

            while (true)
            {
                Console.Write("Введите команду: ");
                var commandInfo = InputHandler.ParseInputString(Console.ReadLine() ?? string.Empty);

                if (commandInfo.Command == CommandType.Unknown)
                {
                    MessagesLogger.ErrorMessage("команда не определена");
                }
                else if(commandInfo.Command == CommandType.Exit)
                {
                    break;
                }
            }

            Environment.Exit(0);
        }

        private static void ShowDescription()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine(" 'создать товар' или 'ст' <наименование товара> - создание товара");
            Console.WriteLine(" 'удалить товар' или 'ут' <наименование товара> - удаление товара");
            Console.WriteLine(" 'создать аптеку' или 'са' <наименование аптеки> <адрес> <телефон> - создание аптеки");
            Console.WriteLine(" 'удалить аптеку' или 'уа' <наименование аптеки> - удаление аптеки");
            Console.WriteLine(" 'создать склад' или 'сс' <наименование склада> <наименование аптеки> - создание склада");
            Console.WriteLine(" 'удалить склад' или 'ус' <наименование склада> - удаление склада");
            Console.WriteLine(" 'создать партию' или 'сп' <наименование партии> <наименование склада> <наименование товара> <количество товара> - создание партии");
            Console.WriteLine(" 'удалить партию' или 'уп' <наименование партии> - удаление партии");
            Console.WriteLine(" 'показать товары' или 'пт' <наименование аптеки> - отображение списка товаров и его количество в выбранной аптеке");
            Console.WriteLine(" 'выход' или 'в' - выход из приложения");

            Console.WriteLine();
            Console.WriteLine("Если параметр содержит пробелы, необходимо обернуть его в кавычки");
            Console.WriteLine("Пример команды - создать партию \"партия для склада 1\" склад1 товар1 100");

            Console.WriteLine();
        }
    }
}