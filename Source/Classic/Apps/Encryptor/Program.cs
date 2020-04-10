// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- simple text encryptor
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Security;

using Encryptor.Properties;

#endregion

namespace Encryptor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(Resources.Usage);
                return;
            }

            var secretText = args[0];
            var password = args[1];
            var encryptedText = SecurityUtility.Encrypt(secretText, password);
            Console.WriteLine(encryptedText);
        }
    }
}
