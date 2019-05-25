using System;
using System.Linq;

namespace NConsole.Options
{
    using static Domain;
    using static String;

    internal static class PrototypeExtensionMethods
    {
        public static string AppendRequiredOrOptional(this string prototype, char requiredOrOptional)
            => !IsNullOrEmpty(prototype) && $"{Colon}{Equal}".Contains(prototype.Last())
                ? prototype
                : $"{prototype}{requiredOrOptional}";
    }
}
