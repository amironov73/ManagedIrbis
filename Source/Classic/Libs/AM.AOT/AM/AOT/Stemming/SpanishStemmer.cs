// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SpanishStemmer.cs --
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.AOT.Stemming
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SpanishStemmer
        : StemmerOperations,
        IStemmer
    {
        private static readonly SpanishStemmer methodObject
            = new SpanishStemmer();


        private static readonly Among[] a_0 =
        {
            new Among ( "", -1, 6, null ),
            new Among ( "\u00E1", 0, 1, null ),
            new Among ( "\u00E9", 0, 2, null ),
            new Among ( "\u00ED", 0, 3, null ),
            new Among ( "\u00F3", 0, 4, null ),
            new Among ( "\u00FA", 0, 5, null )
        };


        private static readonly Among[] a_1 =
        {
            new Among ( "la", -1, -1, null ),
            new Among ( "sela", 0, -1, null ),
            new Among ( "le", -1, -1, null ),
            new Among ( "me", -1, -1, null ),
            new Among ( "se", -1, -1, null ),
            new Among ( "lo", -1, -1, null ),
            new Among ( "selo", 5, -1, null ),
            new Among ( "las", -1, -1, null ),
            new Among ( "selas", 7, -1, null ),
            new Among ( "les", -1, -1, null ),
            new Among ( "los", -1, -1, null ),
            new Among ( "selos", 10, -1, null ),
            new Among ( "nos", -1, -1, null )
        };


        private static readonly Among[] a_2 =
        {
            new Among ( "ando", -1, 6, null ),
            new Among ( "iendo", -1, 6, null ),
            new Among ( "yendo", -1, 7, null ),
            new Among ( "\u00E1ndo", -1, 2, null ),
            new Among ( "i\u00E9ndo", -1, 1, null ),
            new Among ( "ar", -1, 6, null ),
            new Among ( "er", -1, 6, null ),
            new Among ( "ir", -1, 6, null ),
            new Among ( "\u00E1r", -1, 3, null ),
            new Among ( "\u00E9r", -1, 4, null ),
            new Among ( "\u00EDr", -1, 5, null )
        };


        private static readonly Among[] a_3 =
        {
            new Among ( "ic", -1, -1, null ),
            new Among ( "ad", -1, -1, null ),
            new Among ( "os", -1, -1, null ),
            new Among ( "iv", -1, 1, null )
        };


        private static readonly Among[] a_4 =
        {
            new Among ( "able", -1, 1, null ),
            new Among ( "ible", -1, 1, null ),
            new Among ( "ante", -1, 1, null )
        };


        private static readonly Among[] a_5 =
        {
            new Among ( "ic", -1, 1, null ),
            new Among ( "abil", -1, 1, null ),
            new Among ( "iv", -1, 1, null )
        };


        private static readonly Among[] a_6 =
        {
            new Among ( "ica", -1, 1, null ),
            new Among ( "ancia", -1, 2, null ),
            new Among ( "encia", -1, 5, null ),
            new Among ( "adora", -1, 2, null ),
            new Among ( "osa", -1, 1, null ),
            new Among ( "ista", -1, 1, null ),
            new Among ( "iva", -1, 9, null ),
            new Among ( "anza", -1, 1, null ),
            new Among ( "log\u00EDa", -1, 3, null ),
            new Among ( "idad", -1, 8, null ),
            new Among ( "able", -1, 1, null ),
            new Among ( "ible", -1, 1, null ),
            new Among ( "ante", -1, 2, null ),
            new Among ( "mente", -1, 7, null ),
            new Among ( "amente", 13, 6, null ),
            new Among ( "aci\u00F3n", -1, 2, null ),
            new Among ( "uci\u00F3n", -1, 4, null ),
            new Among ( "ico", -1, 1, null ),
            new Among ( "ismo", -1, 1, null ),
            new Among ( "oso", -1, 1, null ),
            new Among ( "amiento", -1, 1, null ),
            new Among ( "imiento", -1, 1, null ),
            new Among ( "ivo", -1, 9, null ),
            new Among ( "ador", -1, 2, null ),
            new Among ( "icas", -1, 1, null ),
            new Among ( "ancias", -1, 2, null ),
            new Among ( "encias", -1, 5, null ),
            new Among ( "adoras", -1, 2, null ),
            new Among ( "osas", -1, 1, null ),
            new Among ( "istas", -1, 1, null ),
            new Among ( "ivas", -1, 9, null ),
            new Among ( "anzas", -1, 1, null ),
            new Among ( "log\u00EDas", -1, 3, null ),
            new Among ( "idades", -1, 8, null ),
            new Among ( "ables", -1, 1, null ),
            new Among ( "ibles", -1, 1, null ),
            new Among ( "aciones", -1, 2, null ),
            new Among ( "uciones", -1, 4, null ),
            new Among ( "adores", -1, 2, null ),
            new Among ( "antes", -1, 2, null ),
            new Among ( "icos", -1, 1, null ),
            new Among ( "ismos", -1, 1, null ),
            new Among ( "osos", -1, 1, null ),
            new Among ( "amientos", -1, 1, null ),
            new Among ( "imientos", -1, 1, null ),
            new Among ( "ivos", -1, 9, null )
        };



        private static readonly Among[] a_7 =
        {
            new Among ( "ya", -1, 1, null ),
            new Among ( "ye", -1, 1, null ),
            new Among ( "yan", -1, 1, null ),
            new Among ( "yen", -1, 1, null ),
            new Among ( "yeron", -1, 1, null ),
            new Among ( "yendo", -1, 1, null ),
            new Among ( "yo", -1, 1, null ),
            new Among ( "yas", -1, 1, null ),
            new Among ( "yes", -1, 1, null ),
            new Among ( "yais", -1, 1, null ),
            new Among ( "yamos", -1, 1, null ),
            new Among ( "y\u00F3", -1, 1, null )
        };



        private static readonly Among[] a_8 =
        {
            new Among ( "aba", -1, 2, null ),
            new Among ( "ada", -1, 2, null ),
            new Among ( "ida", -1, 2, null ),
            new Among ( "ara", -1, 2, null ),
            new Among ( "iera", -1, 2, null ),
            new Among ( "\u00EDa", -1, 2, null ),
            new Among ( "ar\u00EDa", 5, 2, null ),
            new Among ( "er\u00EDa", 5, 2, null ),
            new Among ( "ir\u00EDa", 5, 2, null ),
            new Among ( "ad", -1, 2, null ),
            new Among ( "ed", -1, 2, null ),
            new Among ( "id", -1, 2, null ),
            new Among ( "ase", -1, 2, null ),
            new Among ( "iese", -1, 2, null ),
            new Among ( "aste", -1, 2, null ),
            new Among ( "iste", -1, 2, null ),
            new Among ( "an", -1, 2, null ),
            new Among ( "aban", 16, 2, null ),
            new Among ( "aran", 16, 2, null ),
            new Among ( "ieran", 16, 2, null ),
            new Among ( "\u00EDan", 16, 2, null ),
            new Among ( "ar\u00EDan", 20, 2, null ),
            new Among ( "er\u00EDan", 20, 2, null ),
            new Among ( "ir\u00EDan", 20, 2, null ),
            new Among ( "en", -1, 1, null ),
            new Among ( "asen", 24, 2, null ),
            new Among ( "iesen", 24, 2, null ),
            new Among ( "aron", -1, 2, null ),
            new Among ( "ieron", -1, 2, null ),
            new Among ( "ar\u00E1n", -1, 2, null ),
            new Among ( "er\u00E1n", -1, 2, null ),
            new Among ( "ir\u00E1n", -1, 2, null ),
            new Among ( "ado", -1, 2, null ),
            new Among ( "ido", -1, 2, null ),
            new Among ( "ando", -1, 2, null ),
            new Among ( "iendo", -1, 2, null ),
            new Among ( "ar", -1, 2, null ),
            new Among ( "er", -1, 2, null ),
            new Among ( "ir", -1, 2, null ),
            new Among ( "as", -1, 2, null ),
            new Among ( "abas", 39, 2, null ),
            new Among ( "adas", 39, 2, null ),
            new Among ( "idas", 39, 2, null ),
            new Among ( "aras", 39, 2, null ),
            new Among ( "ieras", 39, 2, null ),
            new Among ( "\u00EDas", 39, 2, null ),
            new Among ( "ar\u00EDas", 45, 2, null ),
            new Among ( "er\u00EDas", 45, 2, null ),
            new Among ( "ir\u00EDas", 45, 2, null ),
            new Among ( "es", -1, 1, null ),
            new Among ( "ases", 49, 2, null ),
            new Among ( "ieses", 49, 2, null ),
            new Among ( "abais", -1, 2, null ),
            new Among ( "arais", -1, 2, null ),
            new Among ( "ierais", -1, 2, null ),
            new Among ( "\u00EDais", -1, 2, null ),
            new Among ( "ar\u00EDais", 55, 2, null ),
            new Among ( "er\u00EDais", 55, 2, null ),
            new Among ( "ir\u00EDais", 55, 2, null ),
            new Among ( "aseis", -1, 2, null ),
            new Among ( "ieseis", -1, 2, null ),
            new Among ( "asteis", -1, 2, null ),
            new Among ( "isteis", -1, 2, null ),
            new Among ( "\u00E1is", -1, 2, null ),
            new Among ( "\u00E9is", -1, 1, null ),
            new Among ( "ar\u00E9is", 64, 2, null ),
            new Among ( "er\u00E9is", 64, 2, null ),
            new Among ( "ir\u00E9is", 64, 2, null ),
            new Among ( "ados", -1, 2, null ),
            new Among ( "idos", -1, 2, null ),
            new Among ( "amos", -1, 2, null ),
            new Among ( "\u00E1bamos", 70, 2, null ),
            new Among ( "\u00E1ramos", 70, 2, null ),
            new Among ( "i\u00E9ramos", 70, 2, null ),
            new Among ( "\u00EDamos", 70, 2, null ),
            new Among ( "ar\u00EDamos", 74, 2, null ),
            new Among ( "er\u00EDamos", 74, 2, null ),
            new Among ( "ir\u00EDamos", 74, 2, null ),
            new Among ( "emos", -1, 1, null ),
            new Among ( "aremos", 78, 2, null ),
            new Among ( "eremos", 78, 2, null ),
            new Among ( "iremos", 78, 2, null ),
            new Among ( "\u00E1semos", 78, 2, null ),
            new Among ( "i\u00E9semos", 78, 2, null ),
            new Among ( "imos", -1, 2, null ),
            new Among ( "ar\u00E1s", -1, 2, null ),
            new Among ( "er\u00E1s", -1, 2, null ),
            new Among ( "ir\u00E1s", -1, 2, null ),
            new Among ( "\u00EDs", -1, 2, null ),
            new Among ( "ar\u00E1", -1, 2, null ),
            new Among ( "er\u00E1", -1, 2, null ),
            new Among ( "ir\u00E1", -1, 2, null ),
            new Among ( "ar\u00E9", -1, 2, null ),
            new Among ( "er\u00E9", -1, 2, null ),
            new Among ( "ir\u00E9", -1, 2, null ),
            new Among ( "i\u00F3", -1, 2, null )
        };



        private static readonly Among[] a_9 =
        {
            new Among ( "a", -1, 1, null ),
            new Among ( "e", -1, 2, null ),
            new Among ( "o", -1, 1, null ),
            new Among ( "os", -1, 1, null ),
            new Among ( "\u00E1", -1, 1, null ),
            new Among ( "\u00E9", -1, 2, null ),
            new Among ( "\u00ED", -1, 1, null ),
            new Among ( "\u00F3", -1, 1, null )
        };


        private static readonly char[] g_v = { (char)17, (char)65, (char)16, (char)0, (char)0, (char)0, (char)0,
                                                 (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
                                                 (char)0,(char)0, (char)1, (char)17, (char)4, (char)10 };

        private int I_p2;
        private int I_p1;
        private int I_pV;


        private void copy_from(SpanishStemmer other)
        {
            I_p2 = other.I_p2;
            I_p1 = other.I_p1;
            I_pV = other.I_pV;
            base.CopyFrom(other);
        }


        private bool r_mark_regions()
        {

            bool subroot = false;
            int v_1;
            int v_2;
            int v_3;
            int v_6;
            int v_8;
            // (, line 31
            I_pV = limit;
            I_p1 = limit;
            I_p2 = limit;
            // do, line 37
            v_1 = cursor;
            do
            {
                // (, line 37
                // or, line 39
                do
                {
                    v_2 = cursor;
                    do
                    {
                        // (, line 38
                        if (!(InGrouping(g_v, 97, 252)))
                        {
                            break;
                        }
                        // or, line 38
                        do
                        {
                            v_3 = cursor;
                            do
                            {
                                // (, line 38
                                if (!(OutGrouping(g_v, 97, 252)))
                                {
                                    break;
                                }
                                // gopast, line 38
                                while (true)
                                {
                                    do
                                    {
                                        if (!(InGrouping(g_v, 97, 252)))
                                        {
                                            break;
                                        }
                                        subroot = true;
                                        if (subroot) break;
                                    } while (false);
                                    if (subroot) { subroot = false; break; }
                                    if (cursor >= limit)
                                    {
                                        subroot = true;
                                        break;
                                    }
                                    cursor++;
                                }
                                if (subroot) { subroot = false; break; }
                                subroot = true;
                                if (subroot) break;
                            } while (false);
                            if (subroot) { subroot = false; break; }
                            cursor = v_3;
                            // (, line 38
                            if (!(InGrouping(g_v, 97, 252)))
                            {
                                subroot = true;
                                goto breaklab2;
                            }
                            // gopast, line 38
                            while (true)
                            {
                                do
                                {
                                    if (!(OutGrouping(g_v, 97, 252)))
                                    {
                                        break;
                                    }
                                    subroot = true;
                                    if (subroot) break;
                                } while (false);
                                if (subroot) { subroot = false; break; }
                                if (cursor >= limit)
                                {
                                    subroot = true;
                                    goto breaklab2;
                                }
                                cursor++;
                            }
                        } while (false);
                        breaklab2: if (subroot) { subroot = false; break; }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = v_2;
                    // (, line 40
                    if (!(OutGrouping(g_v, 97, 252)))
                    {
                        subroot = true;
                        goto breaklab0;
                    }
                    // or, line 40
                    do
                    {
                        v_6 = cursor;
                        do
                        {
                            // (, line 40
                            if (!(OutGrouping(g_v, 97, 252)))
                            {
                                break;
                            }
                            // gopast, line 40
                            while (true)
                            {
                                do
                                {
                                    if (!(InGrouping(g_v, 97, 252)))
                                    {
                                        break;
                                    }
                                    subroot = true;
                                    if (subroot) break;
                                } while (false);
                                if (subroot) { subroot = false; break; }
                                if (cursor >= limit)
                                {
                                    subroot = true;
                                    break;
                                }
                                cursor++;
                            }
                            if (subroot) { subroot = false; break; }
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        cursor = v_6;
                        // (, line 40
                        if (!(InGrouping(g_v, 97, 252)))
                        {
                            subroot = true;
                            goto breaklab0;
                        }
                        // next, line 40
                        if (cursor >= limit)
                        {
                            subroot = true;
                            goto breaklab0;
                        }
                        cursor++;
                    } while (false);
                } while (false);
                breaklab0: if (subroot) { subroot = false; break; }
                // setmark pV, line 41
                I_pV = cursor;
            } while (false);
            cursor = v_1;
            // do, line 43
            v_8 = cursor;
            do
            {
                // (, line 43
                // gopast, line 44
                while (true)
                {
                    do
                    {
                        if (!(InGrouping(g_v, 97, 252)))
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    if (cursor >= limit)
                    {
                        subroot = true;
                        break;
                    }
                    cursor++;
                }
                if (subroot) { subroot = false; break; }
                // gopast, line 44
                while (true)
                {
                    do
                    {
                        if (!(OutGrouping(g_v, 97, 252)))
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    if (cursor >= limit)
                    {
                        subroot = true;
                        break;
                    }
                    cursor++;
                }
                if (subroot) { subroot = false; break; }
                // setmark p1, line 44
                I_p1 = cursor;
                // gopast, line 45
                while (true)
                {
                    do
                    {
                        if (!(InGrouping(g_v, 97, 252)))
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    if (cursor >= limit)
                    {
                        subroot = true;
                        break;
                    }
                    cursor++;
                }
                if (subroot) { subroot = false; break; }
                // gopast, line 45
                while (true)
                {
                    do
                    {
                        if (!(OutGrouping(g_v, 97, 252)))
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    if (cursor >= limit)
                    {
                        subroot = true;
                        break;
                    }
                    cursor++;
                }
                if (subroot) { subroot = false; break; }
                // setmark p2, line 45
                I_p2 = cursor;
            } while (false);
            cursor = v_8;
            return true;
        }


        private bool r_postlude()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            // repeat, line 49
            replab0: while (true)
            {
                v_1 = cursor;
                do
                {
                    // (, line 49
                    // [, line 50
                    bra = cursor;
                    // substring, line 50
                    among_var = find_among(a_0, 6);
                    if (among_var == 0)
                    {
                        break;
                    }
                    // ], line 50
                    ket = cursor;
                    switch (among_var)
                    {
                        case 0:
                            subroot = true;
                            break;
                        case 1:
                            // (, line 51
                            // <-, line 51
                            SliceFrom("a");
                            break;
                        case 2:
                            // (, line 52
                            // <-, line 52
                            SliceFrom("e");
                            break;
                        case 3:
                            // (, line 53
                            // <-, line 53
                            SliceFrom("i");
                            break;
                        case 4:
                            // (, line 54
                            // <-, line 54
                            SliceFrom("o");
                            break;
                        case 5:
                            // (, line 55
                            // <-, line 55
                            SliceFrom("u");
                            break;
                        case 6:
                            // (, line 57
                            // next, line 57
                            if (cursor >= limit)
                            {
                                subroot = true;
                                break;
                            }
                            cursor++;
                            break;
                    }
                    if (subroot) { subroot = false; break; }
                    else if (!subroot)
                    {
                        goto replab0;
                    }
                } while (false);
                cursor = v_1;
                break;
            }
            return true;
        }


        private bool r_RV()
        {
            if (!(I_pV <= cursor))
            {
                return false;
            }
            return true;
        }


        private bool r_R1()
        {
            if (!(I_p1 <= cursor))
            {
                return false;
            }
            return true;
        }


        private bool r_R2()
        {
            if (!(I_p2 <= cursor))
            {
                return false;
            }
            return true;
        }


        private bool r_attached_pronoun()
        {

            int among_var;
            // (, line 67
            // [, line 68
            ket = cursor;
            // substring, line 68
            if (find_among_b(a_1, 13) == 0)
            {
                return false;
            }
            // ], line 68
            bra = cursor;
            // substring, line 72
            among_var = find_among_b(a_2, 11);
            if (among_var == 0)
            {
                return false;
            }
            // call RV, line 72
            if (!r_RV())
            {
                return false;
            }
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 73
                    // ], line 73
                    bra = cursor;
                    // <-, line 73
                    SliceFrom("iendo");
                    break;
                case 2:
                    // (, line 74
                    // ], line 74
                    bra = cursor;
                    // <-, line 74
                    SliceFrom("ando");
                    break;
                case 3:
                    // (, line 75
                    // ], line 75
                    bra = cursor;
                    // <-, line 75
                    SliceFrom("ar");
                    break;
                case 4:
                    // (, line 76
                    // ], line 76
                    bra = cursor;
                    // <-, line 76
                    SliceFrom("er");
                    break;
                case 5:
                    // (, line 77
                    // ], line 77
                    bra = cursor;
                    // <-, line 77
                    SliceFrom("ir");
                    break;
                case 6:
                    // (, line 81
                    // delete, line 81
                    slice_del();
                    break;
                case 7:
                    // (, line 82
                    // literal, line 82
                    if (!(eq_s_b(1, "u")))
                    {
                        return false;
                    }
                    // delete, line 82
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_standard_suffix()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            // (, line 86
            // [, line 87
            ket = cursor;
            // substring, line 87
            among_var = find_among_b(a_6, 46);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 87
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 98
                    // call R2, line 99
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 99
                    slice_del();
                    break;
                case 2:
                    // (, line 104
                    // call R2, line 105
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 105
                    slice_del();
                    // try, line 106
                    v_1 = limit - cursor;
                    do
                    {
                        // (, line 106
                        // [, line 106
                        ket = cursor;
                        // literal, line 106
                        if (!(eq_s_b(2, "ic")))
                        {
                            cursor = limit - v_1;
                            break;
                        }
                        // ], line 106
                        bra = cursor;
                        // call R2, line 106
                        if (!r_R2())
                        {
                            cursor = limit - v_1;
                            break;
                        }
                        // delete, line 106
                        slice_del();
                    } while (false);
                    break;
                case 3:
                    // (, line 110
                    // call R2, line 111
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 111
                    SliceFrom("log");
                    break;
                case 4:
                    // (, line 114
                    // call R2, line 115
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 115
                    SliceFrom("u");
                    break;
                case 5:
                    // (, line 118
                    // call R2, line 119
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 119
                    SliceFrom("ente");
                    break;
                case 6:
                    // (, line 122
                    // call R1, line 123
                    if (!r_R1())
                    {
                        return false;
                    }
                    // delete, line 123
                    slice_del();
                    // try, line 124
                    v_2 = limit - cursor;
                    do
                    {
                        // (, line 124
                        // [, line 125
                        ket = cursor;
                        // substring, line 125
                        among_var = find_among_b(a_3, 4);
                        if (among_var == 0)
                        {
                            cursor = limit - v_2;
                            break;
                        }
                        // ], line 125
                        bra = cursor;
                        // call R2, line 125
                        if (!r_R2())
                        {
                            cursor = limit - v_2;
                            break;
                        }
                        // delete, line 125
                        slice_del();
                        switch (among_var)
                        {
                            case 0:
                                cursor = limit - v_2;
                                subroot = true;
                                break;
                            case 1:
                                // (, line 126
                                // [, line 126
                                ket = cursor;
                                // literal, line 126
                                if (!(eq_s_b(2, "at")))
                                {
                                    cursor = limit - v_2;
                                    subroot = true;
                                    break;
                                }
                                // ], line 126
                                bra = cursor;
                                // call R2, line 126
                                if (!r_R2())
                                {
                                    cursor = limit - v_2;
                                    subroot = true;
                                    break;
                                }
                                // delete, line 126
                                slice_del();
                                break;
                        }
                        if (subroot) { subroot = false; break; }
                    } while (false);
                    break;
                case 7:
                    // (, line 134
                    // call R2, line 135
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 135
                    slice_del();
                    // try, line 136
                    v_3 = limit - cursor;
                    do
                    {
                        // (, line 136
                        // [, line 137
                        ket = cursor;
                        // substring, line 137
                        among_var = find_among_b(a_4, 3);
                        if (among_var == 0)
                        {
                            cursor = limit - v_3;
                            break;
                        }
                        // ], line 137
                        bra = cursor;
                        switch (among_var)
                        {
                            case 0:
                                cursor = limit - v_3;
                                subroot = true;
                                break;
                            case 1:
                                // (, line 140
                                // call R2, line 140
                                if (!r_R2())
                                {
                                    cursor = limit - v_3;
                                    subroot = true;
                                    break;
                                }
                                // delete, line 140
                                slice_del();
                                break;
                        }
                        if (subroot) { subroot = false; break; }
                    } while (false);
                    break;
                case 8:
                    // (, line 146
                    // call R2, line 147
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 147
                    slice_del();
                    // try, line 148
                    v_4 = limit - cursor;
                    do
                    {
                        // (, line 148
                        // [, line 149
                        ket = cursor;
                        // substring, line 149
                        among_var = find_among_b(a_5, 3);
                        if (among_var == 0)
                        {
                            cursor = limit - v_4;
                            break;
                        }
                        // ], line 149
                        bra = cursor;
                        switch (among_var)
                        {
                            case 0:
                                cursor = limit - v_4;
                                subroot = true;
                                break;
                            case 1:
                                // (, line 152
                                // call R2, line 152
                                if (!r_R2())
                                {
                                    cursor = limit - v_4;
                                    subroot = true;
                                    break;
                                }
                                // delete, line 152
                                slice_del();
                                break;
                        }
                        if (subroot) { subroot = false; break; }
                    } while (false);
                    break;
                case 9:
                    // (, line 158
                    // call R2, line 159
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 159
                    slice_del();
                    // try, line 160
                    v_5 = limit - cursor;
                    do
                    {
                        // (, line 160
                        // [, line 161
                        ket = cursor;
                        // literal, line 161
                        if (!(eq_s_b(2, "at")))
                        {
                            cursor = limit - v_5;
                            break;
                        }
                        // ], line 161
                        bra = cursor;
                        // call R2, line 161
                        if (!r_R2())
                        {
                            cursor = limit - v_5;
                            break;
                        }
                        // delete, line 161
                        slice_del();
                    } while (false);
                    break;
            }
            return true;
        }


        private bool r_y_verb_suffix()
        {
            int among_var;
            int v_1;
            int v_2;
            // (, line 167
            // setlimit, line 168
            v_1 = limit - cursor;
            // tomark, line 168
            if (cursor < I_pV)
            {
                return false;
            }
            cursor = I_pV;
            v_2 = limit_backward;
            limit_backward = cursor;
            cursor = limit - v_1;
            // (, line 168
            // [, line 168
            ket = cursor;
            // substring, line 168
            among_var = find_among_b(a_7, 12);
            if (among_var == 0)
            {
                limit_backward = v_2;
                return false;
            }
            // ], line 168
            bra = cursor;
            limit_backward = v_2;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 171
                    // literal, line 171
                    if (!(eq_s_b(1, "u")))
                    {
                        return false;
                    }
                    // delete, line 171
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_verb_suffix()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            // (, line 175
            // setlimit, line 176
            v_1 = limit - cursor;
            // tomark, line 176
            if (cursor < I_pV)
            {
                return false;
            }
            cursor = I_pV;
            v_2 = limit_backward;
            limit_backward = cursor;
            cursor = limit - v_1;
            // (, line 176
            // [, line 176
            ket = cursor;
            // substring, line 176
            among_var = find_among_b(a_8, 96);
            if (among_var == 0)
            {
                limit_backward = v_2;
                return false;
            }
            // ], line 176
            bra = cursor;
            limit_backward = v_2;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 179
                    // try, line 179
                    v_3 = limit - cursor;
                    do
                    {
                        // (, line 179
                        // literal, line 179
                        if (!(eq_s_b(1, "u")))
                        {
                            cursor = limit - v_3;
                            break;
                        }
                        // test, line 179
                        v_4 = limit - cursor;
                        // literal, line 179
                        if (!(eq_s_b(1, "g")))
                        {
                            cursor = limit - v_3;
                            break;
                        }
                        cursor = limit - v_4;
                    } while (false);
                    // ], line 179
                    bra = cursor;
                    // delete, line 179
                    slice_del();
                    break;
                case 2:
                    // (, line 200
                    // delete, line 200
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_residual_suffix()
        {
            int among_var;
            int v_1;
            int v_2;
            // (, line 204
            // [, line 205
            ket = cursor;
            // substring, line 205
            among_var = find_among_b(a_9, 8);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 205
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 208
                    // call RV, line 208
                    if (!r_RV())
                    {
                        return false;
                    }
                    // delete, line 208
                    slice_del();
                    break;
                case 2:
                    // (, line 210
                    // call RV, line 210
                    if (!r_RV())
                    {
                        return false;
                    }
                    // delete, line 210
                    slice_del();
                    // try, line 210
                    v_1 = limit - cursor;
                    do
                    {
                        // (, line 210
                        // [, line 210
                        ket = cursor;
                        // literal, line 210
                        if (!(eq_s_b(1, "u")))
                        {
                            cursor = limit - v_1;
                            break;
                        }
                        // ], line 210
                        bra = cursor;
                        // test, line 210
                        v_2 = limit - cursor;
                        // literal, line 210
                        if (!(eq_s_b(1, "g")))
                        {
                            cursor = limit - v_1;
                            break;
                        }
                        cursor = limit - v_2;
                        // call RV, line 210
                        if (!r_RV())
                        {
                            cursor = limit - v_1;
                            break;
                        }
                        // delete, line 210
                        slice_del();
                    } while (false);
                    break;
            }
            return true;
        }


        private bool CanStem()
        {
            bool subroot = false;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            int v_6;
            // (, line 215
            // do, line 216
            v_1 = cursor;
            do
            {
                // call mark_regions, line 216
                if (!r_mark_regions())
                {
                    break;
                }
            } while (false);
            cursor = v_1;
            // backwards, line 217
            limit_backward = cursor; cursor = limit;
            // (, line 217
            // do, line 218
            v_2 = limit - cursor;
            do
            {
                // call attached_pronoun, line 218
                if (!r_attached_pronoun())
                {
                    break;
                }
            } while (false);
            cursor = limit - v_2;
            // do, line 219
            v_3 = limit - cursor;
            do
            {
                // (, line 219
                // or, line 219
                do
                {
                    v_4 = limit - cursor;
                    do
                    {
                        // call standard_suffix, line 219
                        if (!r_standard_suffix())
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = limit - v_4;
                    do
                    {
                        // call y_verb_suffix, line 220
                        if (!r_y_verb_suffix())
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = limit - v_4;
                    // call verb_suffix, line 221
                    if (!r_verb_suffix())
                    {
                        subroot = true;
                        break;
                    }
                } while (false);
                if (subroot) { subroot = false; break; }
            } while (false);
            cursor = limit - v_3;
            // do, line 223
            v_5 = limit - cursor;
            do
            {
                // call residual_suffix, line 223
                if (!r_residual_suffix())
                {
                    break;
                }
            } while (false);
            cursor = limit - v_5;
            cursor = limit_backward;                    // do, line 225
            v_6 = cursor;
            do
            {
                // call postlude, line 225
                if (!r_postlude())
                {
                    break;
                }
            } while (false);
            cursor = v_6;
            return true;
        }

        /// <inheritdoc cref="IStemmer.Stem"/>
        public string Stem
            (
                string s
            )
        {
            SetCurrent(s.ToLowerInvariant());
            CanStem();
            return GetCurrent();
        }


    }
}
