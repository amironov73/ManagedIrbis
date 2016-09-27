﻿/* QAstLevel8.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Search.Infrastructure
{
    /// <summary>
    /// Level 8.
    /// </summary>
    public sealed class QAstLevel8
    {
        #region Properties

        /// <summary>
        /// Is complex expression?
        /// </summary>
        public bool IsComplex
        {
            get
            {
                return (Right != null && Right.Length != 0)
                    || Left.IsComplex;
            }
        }

        /// <summary>
        /// Left part.
        /// </summary>
        public QAstLevel7 Left { get; set; }

        /// <summary>
        /// Right part.
        /// </summary>
        public QAstLevel7[] Right { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(Left);
            if (!ReferenceEquals(Right, null))
            {
                foreach (QAstLevel7 right in Right)
                {
                    result.Append(" * ");
                    result.Append(right);
                }
            }

            return result.ToString();
        }

        #endregion
    }
}