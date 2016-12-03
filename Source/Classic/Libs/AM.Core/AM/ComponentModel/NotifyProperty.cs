// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NotifyProperty.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;

using JetBrains.Annotations;

#endregion

namespace AM.ComponentModel
{
    /// <summary>
    /// Naive INotifyProperty implementation.
    /// </summary>
    [PublicAPI]
    public class NotifyProperty
        : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies when the property changed.
        /// </summary>
        public virtual void NotifyPropertyChanged
            (
                string str
            )
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (!ReferenceEquals(handler, null))
            {
                handler
                    (
                        this,
                        new PropertyChangedEventArgs(str)
                    );
            }
        }
    }
}
