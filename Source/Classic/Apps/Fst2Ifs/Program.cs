/* Program.cs
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

namespace Fst2Ifs
{
    /// <summary>
    /// 
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        public static int Main ( string[] args )
        {
            TextReader reader = Console.In;
            bool ownReader = false;
            TextWriter writer = Console.Out;
            bool ownWriter = false;
            Encoding encoding = Encoding.Default;

            if ( args.Length > 0 )
            {
                reader = new StreamReader ( args[0], encoding, false );
                ownReader = true;
            }
            if ( args.Length > 1 )
            {
                writer = new StreamWriter ( args[1], false, encoding );
                ownWriter = true;
            }

            using ( FstTransformer transformer 
                = new FstTransformer( reader, ownReader, writer, ownWriter ))
            {
                transformer.TransformFile ();
            }

            return 0;
        }
    }
}
