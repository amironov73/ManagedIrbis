#region Using directives

using System;

using RestfulIrbis.OsmiCards;

#endregion

// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

namespace Protector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: protector <text>");
                return;
            }

            Console.WriteLine
                (
                    args[0][0] == '!'
                        ? DicardsConfiguration.Unprotect(args[0])
                        : DicardsConfiguration.Protect(args[0])
                );
        }
    }
}
