// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioProcessor.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class BiblioProcessor
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public AbstractOutput Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioProcessor()
        {
            Output = new NullOutput();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioProcessor
            (
                [NotNull] AbstractOutput output
            )
        {
            Code.NotNull(output, "output");

            Output = output;
        }

        #endregion

        #region Private members

        /// <summary>
        /// 
        /// </summary>
        private void BildDictionaries
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BildItems
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            BiblioDocument document = context.Document;
            document.BuildItems(context);
        }

        /// <summary>
        /// 
        /// </summary>
        private void FinalRender
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gather records.
        /// </summary>
        private void GatherRecords
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            BiblioDocument document = context.Document;
            document.GatherRecords(context);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RenderReport
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GatherTerms
            (
                BiblioContext context
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build document.
        /// </summary>
        public string BuildDocument
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            GatherRecords(context);
            BildItems(context);
            //GatherTerms(context);
            //BildDictionaries(context);
            //RenderReport(context);
            //FinalRender(context);

            return string.Empty;
        }

        /// <summary>
        /// Get text.
        /// </summary>
        [CanBeNull]
        public string GetText
            (
                [NotNull] BiblioContext context, 
                [NotNull] string path
            )
        {
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(path, "path");

            if (path.StartsWith("*"))
            {
                
            }
            if (path.StartsWith("@"))
            {
                
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin initialize the processor");
            context.Processor = this;
            BiblioDocument document = context.Document;
            document.Initialize(context);
            log.WriteLine("End initialize the processor");
        }

        #endregion

        #region Object members

        #endregion

    }
}