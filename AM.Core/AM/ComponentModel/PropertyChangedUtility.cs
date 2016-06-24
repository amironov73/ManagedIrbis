/* NotifyPropertyChangedUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.ComponentModel
{
    public static class PropertyChangedUtility
    {
        #region Public methods

        /// <summary>
        /// Borrowed from ReactiveUI
        /// </summary>
        public static TRet RaiseAndSetIfChanged<TObj, TRet>
            (
                this TObj This,
                ref TRet backingField,
                TRet newValue,
                [NotNull] string propertyName
            )
            where TObj : INotifyPropertyChanged
        {
            Code.NotNull(propertyName, "propertyName");

            if (EqualityComparer<TRet>.Default.Equals
                (
                    backingField,
                    newValue
                ))
            {
                return newValue;
            }

            //This.raisePropertyChanging(propertyName);
            backingField = newValue;
            //This.raisePropertyChanged(propertyName);

            return newValue;
        }

        public static void NotifyPropertyChanged<T, TProperty>
            (
                this　T propertyChangedBase,
                Expression<Func<T, TProperty>> expression,
                object newValue = null
            )
            where T : NotifyProperty
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                if (newValue != null)
                {
                    typeof(T).GetProperty(propertyName)
                        .SetValue
                        (
                            propertyChangedBase,
                            newValue,
                            null
                        );
                }
                propertyChangedBase.NotifyPropertyChanged(propertyName);
            }

        }


        #endregion
    }
}
