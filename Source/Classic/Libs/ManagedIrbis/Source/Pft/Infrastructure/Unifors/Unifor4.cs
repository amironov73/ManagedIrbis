// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor4.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // &UNIFOR('4N,Format')  - ФОРМАТИРОВАНИЕ ПРЕДЫДУЩЕЙ КОПИИ ТЕКУЩЕЙ ЗАПИСИ:
    //
    // где:
    //
    // N - номер копии (в обратном порядке, т.е. если N=1 - это один шаг назад,
    // N=2 - два шага назад и т.д.). Может принимать значение * - это указывает
    // на последнюю копию.
    // Если N - пустое значение, то в случае повторяющейся группы в качестве
    // значения N берется НОМЕР ТЕКУЩЕГО ПОВТОРЕНИЯ, в противном случае
    // берется первая копия;
    // Format - формат; может задаваться непосредственно или в виде @имя_формата.
    //
    // Если не задается ни N ни Format, т.е. &unifor('4'), то возвращается
    // количество предыдущих копий.
    // Если запись не имеет предыдущих копий, то &unifor('4') возвращает 0,
    // а все остальные конструкции &unifor('4...') возвращают пустоту.
    //
    // Примеры:
    //
    // &unifor('41,@brief')
    // (...&unifor('4,v200^a')...)
    // &unifor('4*,(v910/)')
    //

    static class Unifor4
    {
        #region Public methods

        /// <summary>
        /// Format previous version of current record.
        /// </summary>
        public static void FormatPreviousVersion
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {
            MarcRecord record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                if (string.IsNullOrEmpty(expression))
                {
                    // ibatrak
                    // пустое выражение означает
                    // "количество предыдущих версий"

                    string output = (record.Version - 1).ToInvariantString();
                    context.WriteAndSetFlag(node, output);

                    return;
                }


                int mfn = record.Mfn;
                if (mfn != 0 && record.Version > 1)
                {
                    int version = context.Index;

                    TextNavigator navigator = new TextNavigator(expression);
                    if (navigator.PeekChar() == '*')
                    {
                        version = 0;
                        navigator.ReadChar();
                    }
                    if (navigator.PeekChar() != ',')
                    {
                        string versionText = navigator.ReadInteger();
                        if (string.IsNullOrEmpty(versionText))
                        {
                            return;
                        }

                        version = -versionText.SafeToInt32();
                        navigator.ReadChar(); // eat the comma
                    }
                    else
                    {
                        navigator.ReadChar(); // eat the comma
                    }

                    string format = navigator.GetRemainingText();
                    if (string.IsNullOrEmpty(format))
                    {
                        return;
                    }

                    // ibatrak
                    // после вызова этого unifor
                    // в главном контексте сбрасываются флаги пост обработки
                    context.GetRootContext().PostProcessing = PftCleanup.None;

                    record = context.Provider.ReadRecordVersion(mfn, version);

                    using (PftContextGuard guard = new PftContextGuard(context))
                    {
                        PftContext nestedContext = guard.ChildContext;
                        nestedContext.Record = record;

                        // ibatrak
                        // формат вызывается в контексте без повторений
                        nestedContext.Reset();

                        // TODO some caching

                        PftProgram program = PftUtility.CompileProgram(format);
                        program.Execute(nestedContext);

                        string output = nestedContext.Text;
                        context.Write(node, output);
                    }
                }
            }
        }

        #endregion
    }
}
