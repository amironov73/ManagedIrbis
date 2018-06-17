// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Gtin.cs -- GTIN
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Identifiers
{
    //
    // https://ru.wikipedia.org/wiki/GTIN
    //
    // GTIN (англ. Global Trade Item Number) - международный код маркировки
    // и учёта логистических единиц, разработанный и поддерживаемый GS1.
    // Предложен для замены американского UPC и европейского EAN.
    // GTIN-код имеет длину 8, 12, 13 или 14 цифр, каждая из схем построена
    // по аналогу с предыдущими стандартами и включает в себя префикс
    // компании, код товара и контрольную цифру. GTIN-8 кодируются
    // в штрихкод EAN-8; GTIN-12 могут использовать штрихкод форматов
    // UPC-A, ITF-14 или GS1-128. GTIN-13 кодируются как EAN-13, ITF-14
    // или GS1-128; а GTIN-14 кодируются в ITF-14 или GS1-128,
    // в зависимости от назначения.
    //

    //
    // https://en.wikipedia.org/wiki/Global_Trade_Item_Number
    //
    // Global Trade Item Number (GTIN) is an identifier for trade items,
    // developed by GS1. Such identifiers are used to look up product
    // information in a database (often by entering the number through
    // a barcode scanner pointed at an actual product) which may belong
    // to a retailer, manufacturer, collector, researcher, or other entity.
    // The uniqueness and universality of the identifier is useful
    // in establishing which product in one database corresponds
    // to which product in another database, especially across
    // organizational boundaries.
    //
    // The GTIN standard has incorporated the International Standard
    // Book Number (ISBN), International Standard Serial Number (ISSN),
    // International Standard Music Number (ISMN), International Article
    // Number (which includes the European Article Number and Japanese
    // Article Number) and some Universal Product Codes (UPCs), into
    // a universal number space.
    //
    // GTINs may be 8, 12, 13 or 14 digits long, and each of these four
    // numbering structures are constructed in a similar fashion,
    // combining Company Prefix, Item Reference and a calculated
    // Check Digit (GTIN-14 adds another component- the Indicator Digit,
    // which can be 1-8). GTIN-8s will be encoded in an EAN-8 barcode.
    // GTIN-12s may be shown in UPC-A, ITF-14, or GS1-128 barcodes.
    // GTIN-13s may be encoded in EAN-13, ITF-14 or GS1-128 barcodes,
    // and GTIN-14s may be encoded in ITF-14 or GS1-128 barcodes.
    // The choice of barcode will depend on the application; for example,
    // items to be sold at a retail establishment could be marked with
    // EAN-8, EAN-13, UPC-A or UPC-E barcodes.
    //
    // The EAN-8 code is an eight-digit barcode used usually for very
    // small articles, such as chewing gum, where fitting a larger code
    // onto the item would be difficult. Note: the equivalent UPC small
    // format barcode, UPC-E, encodes a GTIN-12 with a special Company
    // Prefix that allows for "zero suppression" of four zeros in the
    // GTIN-12. The GS1 encoding and decoding rules state that the
    // entire GTIN-12 is used for encoding and that the entire GTIN-12
    // is to be delivered when scanned.
    //

    /// <summary>
    /// Global Trade Item Number.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Gtin
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier { get; set; }

        #endregion
    }
}
