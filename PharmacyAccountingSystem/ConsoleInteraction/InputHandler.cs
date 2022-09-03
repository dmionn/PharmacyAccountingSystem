using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAccountingSystem
{
    internal static class InputHandler
    {
        private static readonly Dictionary<CommandType, string[]> _commandNamesMapper =
            new()
            {
                [CommandType.CreateProduct] = new[] { "создать товар", "ст" },
                [CommandType.DeleteProduct] = new[] { "удалить товар", "ут" },

                [CommandType.CreatePharmacy] = new[] { "создать аптеку", "са" },
                [CommandType.DeletePharmacy] = new[] { "удалить аптеку", "уа" },

                [CommandType.CreateWarehouse] = new[] { "создать склад", "сс" },
                [CommandType.DeleteWarehouse] = new[] { "удалить склад", "ус" },

                [CommandType.CreateBatch] = new[] { "создать партию", "сп" },
                [CommandType.DeleteBatch] = new[] { "удалить партию", "уп" },

                [CommandType.Exit] = new[] { "выход", "в" },
            };

        public static CommandInfo ParseInputString(string input)
        {
            input = input.Trim();   

            var result = new CommandInfo();
            GetCommandType(input, result);

            if (result.Command is not (CommandType.Unknown or CommandType.Exit))
            {
                GetParameters(input, result);
            }

            return result;
        }

        private static void GetCommandType(string input, CommandInfo commandInfo)
        {
            commandInfo.Command = _commandNamesMapper.FirstOrDefault(kvp => kvp.Value.Any(v => input.StartsWith(v))).Key;
        }

        private static void GetParameters(string input, CommandInfo commandInfo)
        {
            var allCommandFormats = _commandNamesMapper.FirstOrDefault(kvp => kvp.Value.Any(v => input.StartsWith(v))).Value;
            var currentCommandFormat = allCommandFormats.First(cf => input.StartsWith(cf));

            var parametersString = input.Remove(0, currentCommandFormat.Length).Trim();
            commandInfo.Parameters = ParseParameters(parametersString);
        }

        private static Dictionary<string, string> ParseParameters(string parametersString)
        {
            var result = new Dictionary<string, string>();



            return result;
        }
    }
}
