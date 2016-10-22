/* TodayEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Suggestions
{
    /// <summary>
    /// Today editor.
    /// </summary>
    public sealed class TodayEditor
        : UITypeEditor
    {
        #region UITypeEditor members

        /// <inheritdoc />
        public override object EditValue
            (
                ITypeDescriptorContext context,
                IServiceProvider provider,
                object value
            )
        {
            if (ReferenceEquals(provider, null))
            {
                return value;
            }

            IWindowsFormsEditorService service
                = provider.GetService(typeof(IWindowsFormsEditorService))
                as IWindowsFormsEditorService;

            if (service != null)
            {
                DateTime? selected = null;
                if ((value != null) && (value is DateTime))
                {
                    selected = (DateTime)value;
                }
                if ((value != null) && (value is string))
                {
                    DateTime dummy;
                    if (DateTime.TryParse(value as string, out dummy))
                    {
                        selected = dummy;
                    }
                }

                TodayControl control = new TodayControl
                    (
                        service,
                        selected
                    );
                service.DropDownControl(control);
                if (control.Success)
                {
                    return control.Date.ToShortDateString();
                }
            }

            return value;
        }

        /// <inheritdoc />
        public override bool GetPaintValueSupported
            (
                ITypeDescriptorContext context
            )
        {
            return false;
        }

        /// <inheritdoc />
        public override UITypeEditorEditStyle GetEditStyle
            (
                ITypeDescriptorContext context
            )
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <inheritdoc />
        public override bool IsDropDownResizable
        {
            get { return false; }
        }

        #endregion
    }
}
