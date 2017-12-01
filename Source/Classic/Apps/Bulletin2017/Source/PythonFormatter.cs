// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PythonFormatter.cs -- 
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
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using IronPython.Hosting;

using ManagedIrbis;

using Microsoft.Scripting.Hosting;

using Newtonsoft.Json;

#endregion

namespace Bulletin2017
{
    public sealed class PythonFormatter
    {
        #region Properties

        [NotNull]
        public ScriptEngine Interpreter { get; private set; }

        [NotNull]
        public Dictionary<string, object> Locals { get; private set; }

        [CanBeNull]
        public TextWriter Output { get; private set; }

        [CanBeNull]
        public MarcRecord Record { get; set; }

        #endregion

        #region Construction

        public PythonFormatter()
        {
            Interpreter = Python.CreateEngine();
            Locals = new CaseInsensitiveDictionary<object>();

            //_engine.Sys.setdefaultencodingImpl((object)Encoding.Default.HeaderName);
            //Locals["a"] = (object)new PythonFormatter._Void<object>(this._A);
            //Locals["apf"] = (object)new PythonFormatter._Void<object, MarcField, string>(this._APF);
            //Locals["aps"] = (object)new PythonFormatter._Void<object, object, object>(this._APS);
            //Locals["apsf"] = (object)new PythonFormatter._Void<object, MarcField, string, object>(this._APSF);
            //Locals["asf"] = (object)new PythonFormatter._Void<MarcField, string>(this._ASF);
            //Locals["ca"] = (object)new PythonFormatter._Void<bool, object, object>(this._CA);
            //Locals["fm"] = (object)new Func<string, string>(this._FM);
            //Locals["fma"] = (object)new Func<string, string[]>(this._FMA);
            //Locals["iif"] = (object)new Func<object, object, object>(this._IIF);
            //Locals["gf"] = (object)new Func<string, MarcField>(this._GF);
            //Locals["gfa"] = (object)new Func<string, MarcField[]>(this._GFA);
            //Locals["nl"] = (object)new PythonFormatter._Void(this._NL);
            //Locals["p"] = (object)new Func<string, bool>(this._P);
            //Locals["pf"] = (object)new Func<string, MarcField, string, string>(this._PF);
            //Locals["ps"] = (object)new Func<MarcField, string, bool>(this._PS);
            //Locals["sf"] = (object)new Func<MarcField, string, string>(this._SF);
            //Locals["v"] = (object)new PythonFormatter._Void<string>(this._V);
            //Locals["va"] = (object)new PythonFormatter._Void<string, string>(this._VA);
            //Locals["vl"] = (object)new PythonFormatter._Void<string>(this._VL);
            //Locals["vla"] = (object)new PythonFormatter._Void<string, string>(this._VLA);
        }

        #endregion

        #region Private members

        //private void _A(object obj)
        //{
        //    if (obj == null)
        //        return;
        //    this._destination.Write(obj);
        //}

        //private void _APF(object prefix, MarcField field, string code)
        //{
        //    if (field == null)
        //        return;
        //    this._APS(prefix, (object)field.FM(code[0]), (object)null);
        //}

        //private void _APS(object prefix, object obj, object suffix)
        //{
        //    if (obj == null)
        //        return;
        //    this._A(prefix);
        //    this._A(obj);
        //    this._A(suffix);
        //}

        //private void _APSF(object prefix, MarcField field, string code, object suffix)
        //{
        //    if (field == null)
        //        return;
        //    this._APS(prefix, (object)field.FM(code[0]), suffix);
        //}

        //private void _ASF(MarcField field, string code)
        //{
        //    this._A((object)this._SF(field, code));
        //}

        //private void _CA(bool condition, object onTrue, object onFalse)
        //{
        //    this._A(condition ? onTrue : onFalse);
        //}

        //private object _IIF(object first, object second)
        //{
        //    return first ?? second;
        //}

        //private string _FM(string label)
        //{
        //    return this.Record.FM(label);
        //}

        //private string[] _FMA(string label)
        //{
        //    return this.Record.FMA(label);
        //}

        //private MarcField[] _GFA(string tag)
        //{
        //    return this.Record.Fields.GetAll(tag);
        //}

        //private MarcField _GF(string tag)
        //{
        //    return this.Record.Fields.GetFirst(tag);
        //}

        //private void _NL()
        //{
        //    this._destination.WriteLine();
        //}

        //private bool _P(string label)
        //{
        //    MarcLabel marcLabel = MarcLabel.Parse(label);
        //    MarcField first = this.Record.Fields.GetFirst(marcLabel.Tag);
        //    if (first == null)
        //        return false;
        //    return marcLabel.IsField || first.SubFields.GetFirst(marcLabel.Code) != null;
        //}

        //private string _PF(string prefix, MarcField field, string code)
        //{
        //    if (field != null)
        //    {
        //        string str = field.FM(code[0]);
        //        if (!string.IsNullOrEmpty(str))
        //            return prefix + str;
        //    }
        //    return (string)null;
        //}

        //private bool _PS(MarcField field, string code)
        //{
        //    return field.SubFields.GetFirst(code[0]) != null;
        //}

        //private string _SF(MarcField field, string code)
        //{
        //    if (field == null)
        //        return (string)null;
        //    return field.FM(code[0]);
        //}

        //private void _V(string label)
        //{
        //    string str = this.Record.FM(label);
        //    if (string.IsNullOrEmpty(str))
        //        return;
        //    this._destination.Write(str);
        //}

        //private void _VA(string label, string separator)
        //{
        //    string[] strArray = this.Record.FMA(label);
        //    if (ArrayUtility.IsNullOrEmpty((Array)strArray))
        //        return;
        //    this._destination.Write(string.Join(separator, strArray));
        //}

        //private void _VL(string label)
        //{
        //    this._V(label);
        //    this._NL();
        //}

        //private void _VLA(string label, string separator)
        //{
        //    this._VA(label, separator);
        //    this._NL();
        //}

        ///// <summary>Clears the cache.</summary>
        //public virtual void ClearCache()
        //{
        //    this._codeCache.Clear();
        //}

        #endregion

        #region Public methods

        public void FormatRecord
            (
                [NotNull] TextWriter output,
                [NotNull] MarcRecord record,
                [NotNull] string format
            )
        {
            Code.NotNull(output, "output");
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(format, "format");

            Output = output;
            Record = record;
            Locals["Record"] = record;
            ScriptIO io = Interpreter.Runtime.IO;
            InterceptorStream interceptor = new InterceptorStream
                (
                    Output,
                    io.OutputEncoding
                );
            io.SetOutput(interceptor, io.OutputEncoding);
        }

        #endregion
    }
}
