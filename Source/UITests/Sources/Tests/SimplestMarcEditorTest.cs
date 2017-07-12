/* SimplestMarcEditorTest.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using CodeJam;

using IrbisUI;
using IrbisUI.Editors;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace UITests
{
    public sealed class SimplestMarcEditorTest
        : IUITest
    {
        #region IUITest members

        public void Run
            (
                IWin32Window ownerWindow
            )
        {
            using (Form form = new Form())
            {
                form.Size = new Size(800, 600);

                MarcRecord record = new MarcRecord();
                RecordField field = new RecordField("700");
                field.AddSubField('a', "Иванов");
                field.AddSubField('b', "И. И.");
                record.Fields.Add(field);

                field = new RecordField("701");
                field.AddSubField('a', "Петров");
                field.AddSubField('b', "П. П.");
                record.Fields.Add(field);

                field = new RecordField("200");
                field.AddSubField('a', "Заглавие");
                field.AddSubField('e', "подзаголовочное");
                field.AddSubField('f', "И. И. Иванов, П. П. Петров");
                record.Fields.Add(field);

                field = new RecordField("300", "Первое примечание");
                record.Fields.Add(field);
                field = new RecordField("300", "Второе примечание");
                record.Fields.Add(field);
                field = new RecordField("300", "Третье примечание");
                record.Fields.Add(field);

                SimplestMarcEditor editor = new SimplestMarcEditor
                {
                    Dock = DockStyle.Fill
                };
                form.Controls.Add(editor);
                editor.SetFields(record.Fields);

                form.ShowDialog(ownerWindow);

                editor.GetFields(record.Fields);
                string text = record.ToPlainText();
                PlainTextForm.ShowDialog(form, text);
            }
        }

        #endregion
    }
}
