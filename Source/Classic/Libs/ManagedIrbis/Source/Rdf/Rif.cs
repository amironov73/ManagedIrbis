// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Rif.cs --
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

namespace ManagedIrbis.Rdf
{
    //
    // https://ru.wikipedia.org/wiki/Rule_Interchange_Format
    //
    // RIF, Rule Interchange Format ("формат обмена правилами") - стандартный
    // формат Семантической паутины для обеспечения взаимодействия между
    // системами, основанными на правилах (rule-based system (англ.)).
    // Является рекомендацией W3C с 22 июня 2010 года.
    //
    // Первоначально предназначался в качестве слоя абстракции для работы
    // с правилами в семантической паутине, но в реальности спроектирован
    // для обеспечения взаимодействия между различными языками правил.
    //
    // RIF включает в себя три диалекта: диалект-центральное ядро (Core
    // dialect), которое можно расширить до диалекта базовой логики
    // (Basic Logic Dialect, BLD) и диалекта продукционных правил
    // (Production Rule Dialect, PRD):
    //
    // * Диалект RIF-Core соответствует Datalog, то есть логике Хорна
    // без функциональных символов с расширениями для поддержки объектов,
    // фреймов и F-логики.
    // * Диалект RIF-BLD в теоретическом плане является языком определённых
    // хорновских правил и стандартной семантикой логики первого порядка.
    // Соответствует системам на основе правил логического вывода.
    // * Диалект RIF-PRD призван охватить основные аспекты различных
    // продукционных систем. Продукционные правила состоят из условия
    // и действия: IF и THEN. Условия аналогичны используемым в RIF-Core
    // и RIF-BLD, а действия интерпретируются в соответствии с операционной
    // семантикой RDF-PRD и могут состоять в добавлении, удалении,
    // изменении фактов базы знаний и вызвать другие побочные эффекты.
    //

    class Rif
    {
    }
}
