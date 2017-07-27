// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldDictionary.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class FieldDictionary
    {
        #region Properties

        [NotNull]
        public Dictionary<string,FieldInfo> Forward { get; private set; }

        [NotNull]
        public Dictionary<int,FieldInfo> Backward { get; private set; }

        public int LastId { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldDictionary()
        {
            Forward = new Dictionary<string, FieldInfo>();
            Backward = new Dictionary<int, FieldInfo>();
            LastId = 0;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public void Add
            (
                [NotNull] FieldInfo info
            )
        {
            Code.NotNull(info, "info");

            Forward.Add(info.Text, info);
            Backward.Add(info.Id, info);
        }

        [NotNull]
        public FieldInfo Create
            (
                [NotNull] PftField field
            )
        {
            Code.NotNull(field, "field");

            FieldInfo result = new FieldInfo(field, ++LastId);
            Add(result);

            return result;
        }

        [CanBeNull]
        public FieldInfo Get
            (
                [NotNull] PftField field
            )
        {
            Code.NotNull(field, "field");

            string text = field.ToString();
            FieldInfo result;
            Forward.TryGetValue(text, out result);

            return result;
        }

        [NotNull]
        public FieldInfo Get
            (
                int id
            )
        {
            FieldInfo result;
            if (!Backward.TryGetValue(id, out result))
            {
                throw new PftCompilerException();
            }

            return result;
        }

        [NotNull]
        public FieldInfo GetOrCreate
            (
                [NotNull] PftField field
            )
        {
            Code.NotNull(field, "field");

            FieldInfo result = Get(field) ?? Create(field);

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
