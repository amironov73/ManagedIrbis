// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EndNoteTag.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Metadata.EndNote
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class EndNoteTag
    {
        #region Constants

        //%A	Author
        //%B	Secondary title	of a book or conference name
        //%C	Place published
        //%D	Year
        //%E	Editor/Secondary author
        //%F	Label
        //%G	Language
        //%H	Translated author
        //%I	Publisher
        //%J	Journal name
        //%K	Keywords
        //%L	Call number
        //%M	Accession number
        //%N	Number	or issue
        //%O	Alternate title
        //%P	Pages
        //%Q	Translated title
        //%R	DOI	digital object identifier
        //%S	Tertiary title
        //%T	Title
        //%U	URL
        //%V	Volume
        //%W	Database provider
        //%X	Abstract
        //%Y	Tertiary author/Translator
        //%Z	Notes
        //%0	Reference type	see right table
        //%1	Custom 1
        //%2	Custom 2
        //%3	Custom 3
        //%4	Custom 4
        //%6	Number of volumes
        //%7	Edition
        //%8	Date
        //%9	Type of work
        //%?	Subsidiary author
        //%@	ISBN/ISSN	ISBN or ISSN number
        //%!	Short title
        //%#	Custom 5
        //%$	Custom 6
        //%]	Custom 7
        //%&	Section
        //%(	Original publication
        //%)	Reprint edition
        //%*	Reviewed item
        //%+	Author address
        //%^	Caption
        //%>	File attachments
        //%<	Research notes
        //%[	Access date
        //%=	Custom 8
        //%~	Name of database

        #endregion
    }
}
