// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Foaf.cs --
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
    // https://ru.wikipedia.org/wiki/FOAF
    //
    // Друг друга, FOAF (англ. Friend of a Friend) - проект по созданию
    // модели машинно-читаемых домашних страниц и социальных сетей,
    // основанный Либби Миллером и Дэном Брикли. Сердцем проекта является
    // спецификация, которая определяет некоторые выражения, используемые
    // в высказываниях (англ. statements) о ком-либо, например: имя, пол
    // и другие характеристики. Чтобы сослаться на эти данные используется
    // идентификатор, включающий уникальные свойства друга (например,
    // SHA1 сумма от E-Mail адреса, Jabber ID, или URI домашней страницы,
    // веблога).
    //
    // Основанный на RDF, определённый с помощью Web Ontology Language
    // и разработанный для лёгкой расширяемости FOAF позволяет распределять
    // данные между различными компьютерными окружениями.
    //
    // Пример
    //
    //<rdf:RDF
    // xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
    // xmlns:foaf="http://xmlns.com/foaf/0.1/"
    // xmlns:rdfs="http://www.w3.org/2000/01/rdf-schema#">
    // <foaf:Person>
    //  <foaf:name>Jimmy Wales</foaf:name>
    //  <foaf:mbox rdf:resource="mailto:jwales@bomis.com" />
    //  <foaf:homepage rdf:resource="http://www.jimmywales.com/" />
    //  <foaf:nick>Jimbo</foaf:nick>
    //  <foaf:depiction rdf:resource="http://www.jimmywales.com/aus_img_small.jpg" />
    //  <foaf:interest>
    //   <rdf:Description rdf:about="http://www.wikimedia.org" rdfs:label="Wikipedia" />
    //  </foaf:interest>
    //  <foaf:knows>
    //   <foaf:Person>
    //    <foaf:name>Angela Beesley</foaf:name> <!-- Wikimedia Board of Trustees -->
    //   </foaf:Person>
    //  </foaf:knows>
    // </foaf:Person>
    //</rdf:RDF>
    //

    class Foaf
    {
    }
}
