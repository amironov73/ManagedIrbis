// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforA.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать заданное повторение поля – &uf('A
    // Вид функции: A.
    // Назначение: Выдать заданное повторение поля.
    // Формат (передаваемая строка):
    // AV<tag>^<delim>*<offset>.<length>#<occur>
    // где:
    // <tag> – метка поля;
    // <delim> – разделитель подполя;
    // <offset> – смещение;
    // <length> – длина;
    // <occur> – номер повторения.
    //
    // Примеры:
    //
    // &unifor('Av200#2')
    // &unifor('Av910^a#5')
    // &unifor('Av10^b*2.10#2')
    //

    static class UniforA
    {
        #region Public methods

        /// <summary>
        /// Get field repeat.
        /// </summary>
        public static void GetFieldRepeat
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            try
            {
                MarcRecord record = context.Record;
                if (!ReferenceEquals(record, null))
                {
                    FieldSpecification specification = new FieldSpecification();
                    if (specification.ParseUnifor(expression)
                        && specification.FieldRepeat.Kind != IndexKind.None)
                    {
                        FieldReference reference = new FieldReference();
                        reference.Apply(specification);

                        string result = reference.Format(record);
                        if (!String.IsNullOrEmpty(result))
                        {
                            context.Write(node, result);
                            context.OutputFlag = true;
                            context.VMonitor = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.TraceException
                    (
                        "UniforA::GetFieldRepeat",
                        exception
                    );
            }
        }

        #endregion
    }
}
