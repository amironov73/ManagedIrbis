// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TimeML.cs --
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

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Metadata
{
    //
    // https://en.wikipedia.org/wiki/TimeML
    //
    // TimeML is a set of rules for encoding documents electronically.
    // It is defined in the TimeML Specification version 1.2.1 developed
    // by several efforts, led in large part by the Laboratory for
    // Linguistics and Computation at Brandeis University.
    //
    // The TimeML project's goal is to create a standard markup language
    // for temporal events in a document. TimeML addresses four problems
    // regarding event markup, including time stamping (with which
    // an event is anchored to a time), ordering events with respect
    // to one another, reasoning with contextually underspecified temporal
    // expressions, and reasoning about the length of events and their outcomes.
    //

    class TimeML
    {
    }
}
