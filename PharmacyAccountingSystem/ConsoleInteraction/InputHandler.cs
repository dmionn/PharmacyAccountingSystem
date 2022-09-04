namespace PharmacyAccountingSystem
{
    internal static class InputHandler
    {
        private static readonly Dictionary<CommandType, string[]> _commandNamesMap =
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

                [CommandType.ShowProducts] = new[] { "показать товары", "пт" },

                [CommandType.Exit] = new[] { "выход", "в" },
            };

        private static readonly Dictionary<CommandType, string[]> _commandParameterNamesMap =
            new()
            {
                [CommandType.CreateProduct] = new[] { nameof(Product.Name) },
                [CommandType.DeleteProduct] = new[] { nameof(Product.Name) },

                [CommandType.CreatePharmacy] = new[] { nameof(Pharmacy.Name), nameof(Pharmacy.Address), nameof(Pharmacy.PhoneNumber) },
                [CommandType.DeletePharmacy] = new[] { nameof(Pharmacy.Name) },

                [CommandType.CreateWarehouse] = new[] { nameof(Warehouse.Name), nameof(Warehouse.PharmacyName) },
                [CommandType.DeleteWarehouse] = new[] { nameof(Warehouse.Name) },

                [CommandType.CreateBatch] = new[] { nameof(Batch.Name), nameof(Batch.WarehouseName), nameof(Batch.ProductName), nameof(Batch.Number) },
                [CommandType.DeleteBatch] = new[] { nameof(Batch.Name) },

                [CommandType.ShowProducts] = new[] { "PharmacyName" },
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
            commandInfo.Command = _commandNamesMap.FirstOrDefault(kvp => kvp.Value.Any(v => input.StartsWith(v))).Key;
        }

        private static void GetParameters(string input, CommandInfo commandInfo)
        {
            var allCommandFormats = _commandNamesMap.FirstOrDefault(kvp => kvp.Value.Any(v => input.StartsWith(v))).Value;
            var currentCommandFormat = allCommandFormats.First(cf => input.StartsWith(cf));

            var parametersString = input.Remove(0, currentCommandFormat.Length).Trim();
            ParseParameters(parametersString, commandInfo);
        }

        private static void ParseParameters(string parametersString, CommandInfo commandInfo)
        {
            var paramNamesList = _commandParameterNamesMap[commandInfo.Command];

            bool notEnoughParameters() => parametersString.Split(' ').Length < paramNamesList.Length;
            bool containsQuotes() => parametersString.Any(i => i == '"');
            bool wrongQuotesNumber() => containsQuotes() && (parametersString.Count(i => i == '"') % 2 != 0);

            if (string.IsNullOrEmpty(parametersString) || notEnoughParameters() || wrongQuotesNumber())
            {
                commandInfo.Command = CommandType.Unknown;
            }
            else
            {
                if (containsQuotes())
                {
                    ParseParametersWithQuotes(parametersString, commandInfo, paramNamesList);
                }
                else
                {
                    ParseParametersWithoutQuotes(parametersString, commandInfo, paramNamesList);
                }
            }
        }

        private static void ParseParametersWithQuotes(string parametersString, CommandInfo commandInfo, string[] paramNamesList)
        {
            var paramValuesList = new List<string>();
            var parameterValue = new List<char>();

            void addNewParameter()
            {
                paramValuesList.Add(new string(parameterValue.ToArray()).Trim());
                parameterValue.Clear();
            }

            bool isLastChar(int index) => ++index == parametersString.Length;
            bool nextCharInList(int index, char[] charList) => charList.Contains(parametersString[++index]);

            var parameterWithQuotes = false;

            for (int i = 0; i < parametersString.Length; i++)
            {
                if (parametersString[i] == '"')
                {
                    parameterWithQuotes = !parameterWithQuotes;
                    continue;
                }

                parameterValue.Add(parametersString[i]);
                if (parameterWithQuotes)
                {
                    if (isLastChar(i))
                    {
                        commandInfo.Command = CommandType.Unknown;
                        break;
                    }

                    if (nextCharInList(i, new[] { '"' }))
                    {
                        addNewParameter();
                    }
                }
                else if (isLastChar(i) || nextCharInList(i, new[] { ' ', '"' }))
                {
                    addNewParameter();
                }
            }

            if (paramValuesList.Count() != paramNamesList.Length)
            {
                commandInfo.Command = CommandType.Unknown;
            }
            else
            {
                FillParametersList(paramNamesList, paramValuesList.ToArray(), commandInfo);
            }
        }

        private static void ParseParametersWithoutQuotes(string parametersString, CommandInfo commandInfo, string[] paramNamesList)
        {
            var paramValuesList = parametersString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (paramValuesList.Length != paramNamesList.Length)
            {
                commandInfo.Command = CommandType.Unknown;
            }
            else
            {
                FillParametersList(paramNamesList, paramValuesList, commandInfo);
            }
        }

        private static void FillParametersList(string[] paramNamesList, string[] paramValuesList, CommandInfo commandInfo)
        {
            paramNamesList.Zip(paramValuesList).ToList().ForEach(i => commandInfo.Parameters.Add(i.First, i.Second));
        }
    }
}
