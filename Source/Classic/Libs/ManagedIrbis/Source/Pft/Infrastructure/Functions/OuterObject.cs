// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OuterObject.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    abstract class OuterObject
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Usage counter.
        /// </summary>
        public int Counter { get; internal set; }

        /// <summary>
        /// Name of the object.
        /// </summary>
        [NotNull]
        public string Name { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        protected OuterObject
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            Name = name;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Call method of the object.
        /// </summary>
        [CanBeNull]
        public virtual object CallMethod
            (
                [NotNull] string methodName,
                [NotNull] object[] parameters
            )
        {
            Code.NotNullNorEmpty(methodName, "methodName");
            Code.NotNull(parameters, "parameters");

            // Nothing to do here

            return null;
        }

        /// <summary>
        /// Decrease counter.
        /// </summary>
        public int DecreaseCounter()
        {
            return --Counter;
        }

        /// <summary>
        /// Increase counter.
        /// </summary>
        public int IncreaseCounter()
        {
            return ++Counter;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc />
        public virtual void Dispose()
        {
            // Nothing to do here
        }

        #endregion
    }
}
