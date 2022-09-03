namespace PharmacyAccountingSystem
{
    internal class CommandInfo
    {
        public CommandType Command { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
