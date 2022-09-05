namespace PharmacyAccountingSystem
{
    class Program
    {
        private static void Main(string[] args)
        {
            ShowDescription();

            var dbContext = new DatabaseContext();
            var commandsController = new CommandsController(dbContext);

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
                else if (commandsController.RunCommand(commandInfo))
                {
                    MessagesLogger.CommandСompletedMessage();
                }
            }

            Environment.Exit(0);
        }

        private static void ShowDescription()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine(" 'создать товар' или 'ст' <id товара> <наименование товара> - создание товара");
            Console.WriteLine(" 'удалить товар' или 'ут' <id товара> - удаление товара");
            Console.WriteLine(" 'создать аптеку' или 'са' <id аптеки> <наименование аптеки> <адрес> <телефон> - создание аптеки");
            Console.WriteLine(" 'удалить аптеку' или 'уа' <id аптеки> - удаление аптеки");
            Console.WriteLine(" 'создать склад' или 'сс' <id склада> <id аптеки> <наименование склада> - создание склада");
            Console.WriteLine(" 'удалить склад' или 'ус' <id склада> - удаление склада");
            Console.WriteLine(" 'создать партию' или 'сп' <id партии> <id товара> <id склада> <количество товара> - создание партии");
            Console.WriteLine(" 'удалить партию' или 'уп' <id партии> - удаление партии");
            Console.WriteLine(" 'показать товары' или 'пт' <id аптеки> - отображение списка товаров и его количество в выбранной аптеке");
            Console.WriteLine(" 'выход' или 'в' - выход из приложения");

            Console.WriteLine();
            Console.WriteLine("Если параметр содержит пробелы, необходимо обернуть его в кавычки");
            Console.WriteLine("Пример команды - создать склад с1 ап1 \"склад для первой аптеки\"");

            Console.WriteLine();
        }
    }
}