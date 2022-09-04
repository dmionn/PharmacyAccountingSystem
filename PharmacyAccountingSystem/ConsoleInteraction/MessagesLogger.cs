namespace PharmacyAccountingSystem
{
    internal static class MessagesLogger
    {
        public static void ErrorMessage(string msg) => ShowIndentedMessage($"Ошибка: {msg}");
        public static void CustomMessage(string msg) => ShowIndentedMessage(msg);
        public static void CommandСompletedMessage() => ShowIndentedMessage("Операция выполнена");

        private static void ShowIndentedMessage(string msg) => Console.WriteLine($"{msg}{Environment.NewLine}");
    }
}
