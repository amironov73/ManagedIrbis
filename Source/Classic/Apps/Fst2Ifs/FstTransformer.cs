/* FstTransformer.cs - transforms FST to IFS file
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#endregion

namespace Fst2Ifs
{
    /// <summary>
    /// Transforms FST to IFS file
    /// </summary>
    public sealed class FstTransformer
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Input text stream.
        /// </summary>
        public TextReader In { get; set; }

        /// <summary>
        /// Output text stream.
        /// </summary>
        public TextWriter Out { get; set; }

        #endregion

        #region Construction

        public FstTransformer ()
        {
            In = Console.In;
            Out = Console.Out;
            _ownReader = false;
            _ownWriter = false;
        }

        public FstTransformer ( TextReader reader, TextWriter writer )
        {
            In = reader;
            Out = writer;
            _ownReader = true;
            _ownWriter = true;
        }

        public FstTransformer
            (
                TextReader reader,
                bool ownReader,
                TextWriter writer,
                bool ownWriter
            )
        {
            In = reader;
            _ownReader = ownReader;
            Out = writer;
            _ownWriter = ownWriter;
        }

        #endregion

        #region Private members

        private readonly bool _ownReader, _ownWriter;

        private readonly Regex _lineMatcher = new Regex ( @"^(\d+)\s+(\d+)" );
        private readonly Regex _itemFinder = new Regex(@"[dvn](\d+)");

        #endregion

        #region Public methods

        /// <summary>
        /// Finds 'd', 'n' and 'v' references in the line.
        /// </summary>
        public void TransformLine ( string line )
        {
            line = line.Trim ();
            if ( string.IsNullOrEmpty ( line ) )
            {
                return;
            }

            Match lineMatch = _lineMatcher.Match ( line );
            if ( !lineMatch.Success )
            {
                Out.WriteLine(line);
                return;
            }

            Out.Write ( lineMatch.Groups[1].Value );

            List <string> found = new List < string > ();
            MatchCollection itemMatches = _itemFinder.Matches ( line );
            foreach ( Match itemMatch in itemMatches )
            {
                string item = itemMatch.Groups [ 1 ].Value;
                if ( !found.Contains ( item ) )
                {
                    found.Add ( item );
                }
            }

            foreach ( string item in found )
            {
                Out.Write ( ",{0}", item );
            }

            Out.Write ( " {0} ", lineMatch.Groups[2].Value );

            Out.WriteLine ( line.Substring ( lineMatch.Length ).TrimStart () );
        }

        /// <summary>
        /// Do the job: reads input line by line and transforms it
        /// into the IFS.
        /// </summary>
        public void TransformFile ()
        {
            string line;

            while ( (line = In.ReadLine ()) != null )
            {
                TransformLine ( line );
            }
        }

        #endregion

        #region IDisposable members

        public void Dispose ()
        {
            if ( _ownReader )
            {
                In.Dispose ();
            }
            if ( _ownWriter )
            {
                Out.Dispose ();
            }
        }

        #endregion
    }
}
