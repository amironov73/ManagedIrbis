// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PortugalStemmer.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public sealed class PortugalStemmer
        : StemmerOperations,
        IStemmer
    {
        private static readonly PortugalStemmer methodObject 
            = new PortugalStemmer();

        private static readonly Among[] a_0 =
        {
            new Among ( "", -1, 3, null ),
            new Among ( "\u00E3", 0, 1, null ),
            new Among ( "\u00F5", 0, 2, null )
        };


        private static readonly Among[] a_1 =
        {
            new Among ( "", -1, 3, null ),
            new Among ( "a~", 0, 1, null ),
            new Among ( "o~", 0, 2, null )
        };


        private static readonly Among[] a_2 =
        {
            new Among ( "ic", -1, -1, null ),
            new Among ( "ad", -1, -1, null ),
            new Among ( "os", -1, -1, null ),
            new Among ( "iv", -1, 1, null )
        };


        private static readonly Among[] a_3 =
        {
            new Among ( "ante", -1, 1, null ),
            new Among ( "avel", -1, 1, null ),
            new Among ( "\u00EDvel", -1, 1, null )
        };


        private static readonly Among[] a_4 =
        {
            new Among ( "ic", -1, 1, null ),
            new Among ( "abil", -1, 1, null ),
            new Among ( "iv", -1, 1, null )
        };


        private static readonly Among[] a_5 =
        {
            new Among ( "ica", -1, 1, null ),
            new Among ( "\u00E2ncia", -1, 1, null ),
            new Among ( "\u00EAncia", -1, 4, null ),
            new Among ( "ira", -1, 9, null ),
            new Among ( "adora", -1, 1, null ),
            new Among ( "osa", -1, 1, null ),
            new Among ( "ista", -1, 1, null ),
            new Among ( "iva", -1, 8, null ),
            new Among ( "eza", -1, 1, null ),
            new Among ( "log\u00EDa", -1, 2, null ),
            new Among ( "idade", -1, 7, null ),
            new Among ( "ante", -1, 1, null ),
            new Among ( "mente", -1, 6, null ),
            new Among ( "amente", 12, 5, null ),
            new Among ( "\u00E1vel", -1, 1, null ),
            new Among ( "\u00EDvel", -1, 1, null ),
            new Among ( "uci\u00F3n", -1, 3, null ),
            new Among ( "ico", -1, 1, null ),
            new Among ( "ismo", -1, 1, null ),
            new Among ( "oso", -1, 1, null ),
            new Among ( "amento", -1, 1, null ),
            new Among ( "imento", -1, 1, null ),
            new Among ( "ivo", -1, 8, null ),
            new Among ( "a\u00E7a~o", -1, 1, null ),
            new Among ( "ador", -1, 1, null ),
            new Among ( "icas", -1, 1, null ),
            new Among ( "\u00EAncias", -1, 4, null ),
            new Among ( "iras", -1, 9, null ),
            new Among ( "adoras", -1, 1, null ),
            new Among ( "osas", -1, 1, null ),
            new Among ( "istas", -1, 1, null ),
            new Among ( "ivas", -1, 8, null ),
            new Among ( "ezas", -1, 1, null ),
            new Among ( "log\u00EDas", -1, 2, null ),
            new Among ( "idades", -1, 7, null ),
            new Among ( "uciones", -1, 3, null ),
            new Among ( "adores", -1, 1, null ),
            new Among ( "antes", -1, 1, null ),
            new Among ( "a\u00E7o~es", -1, 1, null ),
            new Among ( "icos", -1, 1, null ),
            new Among ( "ismos", -1, 1, null ),
            new Among ( "osos", -1, 1, null ),
            new Among ( "amentos", -1, 1, null ),
            new Among ( "imentos", -1, 1, null ),
            new Among ( "ivos", -1, 8, null )
        };



        private static readonly Among[] a_6 =
        {
            new Among ( "ada", -1, 1, null ),
            new Among ( "ida", -1, 1, null ),
            new Among ( "ia", -1, 1, null ),
            new Among ( "aria", 2, 1, null ),
            new Among ( "eria", 2, 1, null ),
            new Among ( "iria", 2, 1, null ),
            new Among ( "ara", -1, 1, null ),
            new Among ( "era", -1, 1, null ),
            new Among ( "ira", -1, 1, null ),
            new Among ( "ava", -1, 1, null ),
            new Among ( "asse", -1, 1, null ),
            new Among ( "esse", -1, 1, null ),
            new Among ( "isse", -1, 1, null ),
            new Among ( "aste", -1, 1, null ),
            new Among ( "este", -1, 1, null ),
            new Among ( "iste", -1, 1, null ),
            new Among ( "ei", -1, 1, null ),
            new Among ( "arei", 16, 1, null ),
            new Among ( "erei", 16, 1, null ),
            new Among ( "irei", 16, 1, null ),
            new Among ( "am", -1, 1, null ),
            new Among ( "iam", 20, 1, null ),
            new Among ( "ariam", 21, 1, null ),
            new Among ( "eriam", 21, 1, null ),
            new Among ( "iriam", 21, 1, null ),
            new Among ( "aram", 20, 1, null ),
            new Among ( "eram", 20, 1, null ),
            new Among ( "iram", 20, 1, null ),
            new Among ( "avam", 20, 1, null ),
            new Among ( "em", -1, 1, null ),
            new Among ( "arem", 29, 1, null ),
            new Among ( "erem", 29, 1, null ),
            new Among ( "irem", 29, 1, null ),
            new Among ( "assem", 29, 1, null ),
            new Among ( "essem", 29, 1, null ),
            new Among ( "issem", 29, 1, null ),
            new Among ( "ado", -1, 1, null ),
            new Among ( "ido", -1, 1, null ),
            new Among ( "ando", -1, 1, null ),
            new Among ( "endo", -1, 1, null ),
            new Among ( "indo", -1, 1, null ),
            new Among ( "ara~o", -1, 1, null ),
            new Among ( "era~o", -1, 1, null ),
            new Among ( "ira~o", -1, 1, null ),
            new Among ( "ar", -1, 1, null ),
            new Among ( "er", -1, 1, null ),
            new Among ( "ir", -1, 1, null ),
            new Among ( "as", -1, 1, null ),
            new Among ( "adas", 47, 1, null ),
            new Among ( "idas", 47, 1, null ),
            new Among ( "ias", 47, 1, null ),
            new Among ( "arias", 50, 1, null ),
            new Among ( "erias", 50, 1, null ),
            new Among ( "irias", 50, 1, null ),
            new Among ( "aras", 47, 1, null ),
            new Among ( "eras", 47, 1, null ),
            new Among ( "iras", 47, 1, null ),
            new Among ( "avas", 47, 1, null ),
            new Among ( "es", -1, 1, null ),
            new Among ( "ardes", 58, 1, null ),
            new Among ( "erdes", 58, 1, null ),
            new Among ( "irdes", 58, 1, null ),
            new Among ( "ares", 58, 1, null ),
            new Among ( "eres", 58, 1, null ),
            new Among ( "ires", 58, 1, null ),
            new Among ( "asses", 58, 1, null ),
            new Among ( "esses", 58, 1, null ),
            new Among ( "isses", 58, 1, null ),
            new Among ( "astes", 58, 1, null ),
            new Among ( "estes", 58, 1, null ),
            new Among ( "istes", 58, 1, null ),
            new Among ( "is", -1, 1, null ),
            new Among ( "ais", 71, 1, null ),
            new Among ( "eis", 71, 1, null ),
            new Among ( "areis", 73, 1, null ),
            new Among ( "ereis", 73, 1, null ),
            new Among ( "ireis", 73, 1, null ),
            new Among ( "\u00E1reis", 73, 1, null ),
            new Among ( "\u00E9reis", 73, 1, null ),
            new Among ( "\u00EDreis", 73, 1, null ),
            new Among ( "\u00E1sseis", 73, 1, null ),
            new Among ( "\u00E9sseis", 73, 1, null ),
            new Among ( "\u00EDsseis", 73, 1, null ),
            new Among ( "\u00E1veis", 73, 1, null ),
            new Among ( "\u00EDeis", 73, 1, null ),
            new Among ( "ar\u00EDeis", 84, 1, null ),
            new Among ( "er\u00EDeis", 84, 1, null ),
            new Among ( "ir\u00EDeis", 84, 1, null ),
            new Among ( "ados", -1, 1, null ),
            new Among ( "idos", -1, 1, null ),
            new Among ( "amos", -1, 1, null ),
            new Among ( "\u00E1ramos", 90, 1, null ),
            new Among ( "\u00E9ramos", 90, 1, null ),
            new Among ( "\u00EDramos", 90, 1, null ),
            new Among ( "\u00E1vamos", 90, 1, null ),
            new Among ( "\u00EDamos", 90, 1, null ),
            new Among ( "ar\u00EDamos", 95, 1, null ),
            new Among ( "er\u00EDamos", 95, 1, null ),
            new Among ( "ir\u00EDamos", 95, 1, null ),
            new Among ( "emos", -1, 1, null ),
            new Among ( "aremos", 99, 1, null ),
            new Among ( "eremos", 99, 1, null ),
            new Among ( "iremos", 99, 1, null ),
            new Among ( "\u00E1ssemos", 99, 1, null ),
            new Among ( "\u00EAssemos", 99, 1, null ),
            new Among ( "\u00EDssemos", 99, 1, null ),
            new Among ( "imos", -1, 1, null ),
            new Among ( "armos", -1, 1, null ),
            new Among ( "ermos", -1, 1, null ),
            new Among ( "irmos", -1, 1, null ),
            new Among ( "\u00E1mos", -1, 1, null ),
            new Among ( "ar\u00E1s", -1, 1, null ),
            new Among ( "er\u00E1s", -1, 1, null ),
            new Among ( "ir\u00E1s", -1, 1, null ),
            new Among ( "eu", -1, 1, null ),
            new Among ( "iu", -1, 1, null ),
            new Among ( "ou", -1, 1, null ),
            new Among ( "ar\u00E1", -1, 1, null ),
            new Among ( "er\u00E1", -1, 1, null ),
            new Among ( "ir\u00E1", -1, 1, null )
        };



        private static readonly Among[] a_7 =
        {
            new Among ( "a", -1, 1, null ),
            new Among ( "i", -1, 1, null ),
            new Among ( "o", -1, 1, null ),
            new Among ( "os", -1, 1, null ),
            new Among ( "\u00E1", -1, 1, null ),
            new Among ( "\u00ED", -1, 1, null ),
            new Among ( "\u00F3", -1, 1, null )
        };


        private static readonly Among[] a_8 =
        {
            new Among ( "e", -1, 1, null ),
            new Among ( "\u00E7", -1, 2, null ),
            new Among ( "\u00E9", -1, 1, null ),
            new Among ( "\u00EA", -1, 1, null )
        };


        private static readonly char[] g_v = {(char)17, (char)65, (char)16, (char)0, (char)0, (char)0, (char)0,
                                                 (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
                                                 (char)0, (char)0, (char)3, (char)19, (char)12, (char)2 };

        private int I_p2;
        private int I_p1;
        private int I_pV;


        private void copy_from(PortugalStemmer other)
        {
            I_p2 = other.I_p2;
            I_p1 = other.I_p1;
            I_pV = other.I_pV;
            CopyFrom(other);
        }



        private bool r_prelude()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            // repeat, line 36
            replab0: while (true)
            {
                v_1 = cursor;
                do
                {
                    // (, line 36
                    // [, line 37
                    bra = cursor;
                    // substring, line 37
                    among_var = find_among(a_0, 3);
                    if (among_var == 0)
                    {
                        break;
                    }
                    // ], line 37
                    ket = cursor;
                    switch (among_var)
                    {
                        case 0:
                            subroot = true;
                            break;
                        case 1:
                            // (, line 38
                            // <-, line 38
                            SliceFrom("a~");
                            break;
                        case 2:
                            // (, line 39
                            // <-, line 39
                            SliceFrom("o~");
                            break;
                        case 3:
                            // (, line 40
                            // next, line 40
                            if (cursor >= limit)
                            {
                                subroot = true;
                                break;
                            }
                            cursor++;
                            break;
                    }
                    if (subroot) { subroot = false; break; }
                    if (!subroot)
                    {
                        goto replab0;
                    }
                } while (false);
                cursor = v_1;
                break;
            }
            return true;
        }


        private bool r_mark_regions()
        {
            bool subroot = false;
            int v_1;
            int v_2;
            int v_3;
            int v_6;
            int v_8;
            // (, line 44
            I_pV = limit;
            I_p1 = limit;
            I_p2 = limit;
            // do, line 50
            v_1 = cursor;
            do
            {
                // (, line 50
                // or, line 52
                do
                {
                    v_2 = cursor;
                    do
                    {
                        // (, line 51
                        if (!(InGrouping(g_v, 97, 250)))
                        {
                            break;
                        }
                        // or, line 51
                        do
                        {
                            v_3 = cursor;
                            do
                            {
                                // (, line 51
                                if (!(OutGrouping(g_v, 97, 250)))
                                {
                                    break;
                                }
                                // gopast, line 51
                                //        golab5: 
                                while (true)
                                {
                                    do
                                    {
                                        if (!(InGrouping(g_v, 97, 250)))
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
                            // (, line 51
                            if (!(InGrouping(g_v, 97, 250)))
                            {
                                subroot = true;
                                break;
                            }
                            // gopast, line 51
                            while (true)
                            {
                                do
                                {
                                    if (!(OutGrouping(g_v, 97, 250)))
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
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = v_2;
                    // (, line 53
                    if (!(OutGrouping(g_v, 97, 250)))
                    {
                        subroot = true;
                        break;
                    }
                    // or, line 53
                    do
                    {
                        v_6 = cursor;
                        do
                        {
                            // (, line 53
                            if (!(OutGrouping(g_v, 97, 250)))
                            {
                                break;
                            }
                            // gopast, line 53
                            while (true)
                            {
                                do
                                {
                                    if (!(InGrouping(g_v, 97, 250)))
                                    {
                                        break;
                                    }
                                    subroot = true;
                                    if (subroot) break;
                                } while (false);
                                if (subroot) { subroot = false; break; }
                                if (cursor >= limit)
                                {
                                    subroot = false;
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
                        // (, line 53
                        if (!(InGrouping(g_v, 97, 250)))
                        {
                            subroot = true;
                            break;
                        }
                        // next, line 53
                        if (cursor >= limit)
                        {
                            subroot = true;
                            break;
                        }
                        cursor++;
                    } while (false);
                } while (false);
                if (subroot) { subroot = false; break; } //***lab0
                // setmark pV, line 54
                I_pV = cursor;
            } while (false);
            cursor = v_1;
            // do, line 56
            v_8 = cursor;
            do
            {
                // (, line 56
                // gopast, line 57
                while (true)
                {
                    do
                    {
                        if (!(InGrouping(g_v, 97, 250)))
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
                // gopast, line 57
                while (true)
                {
                    do
                    {
                        if (!(OutGrouping(g_v, 97, 250)))
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
                // setmark p1, line 57
                I_p1 = cursor;
                // gopast, line 58
                while (true)
                {
                    do
                    {
                        if (!(InGrouping(g_v, 97, 250)))
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
                // gopast, line 58
                while (true)
                {
                    do
                    {
                        if (!(OutGrouping(g_v, 97, 250)))
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
                // setmark p2, line 58
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
            // repeat, line 62
            replab0: while (true)
            {
                v_1 = cursor;
                //lab1: 
                do
                {
                    // (, line 62
                    // [, line 63
                    bra = cursor;
                    // substring, line 63
                    among_var = find_among(a_1, 3);
                    if (among_var == 0)
                    {
                        break;
                    }
                    // ], line 63
                    ket = cursor;
                    switch (among_var)
                    {
                        case 0:
                            subroot = true;
                            break;
                        case 1:
                            // (, line 64
                            // <-, line 64
                            SliceFrom("\u00E3");
                            break;
                        case 2:
                            // (, line 65
                            // <-, line 65
                            SliceFrom("\u00F5");
                            break;
                        case 3:
                            // (, line 66
                            // next, line 66
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


        private bool r_standard_suffix()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            // (, line 76
            // [, line 77
            ket = cursor;
            // substring, line 77
            among_var = find_among_b(a_5, 45);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 77
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 92
                    // call R2, line 93
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 93
                    slice_del();
                    break;
                case 2:
                    // (, line 97
                    // call R2, line 98
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 98
                    SliceFrom("log");
                    break;
                case 3:
                    // (, line 101
                    // call R2, line 102
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 102
                    SliceFrom("u");
                    break;
                case 4:
                    // (, line 105
                    // call R2, line 106
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 106
                    SliceFrom("ente");
                    break;
                case 5:
                    // (, line 109
                    // call R1, line 110
                    if (!r_R1())
                    {
                        return false;
                    }
                    // delete, line 110
                    slice_del();
                    // try, line 111
                    v_1 = limit - cursor;
                    do
                    {
                        // (, line 111
                        // [, line 112
                        ket = cursor;
                        // substring, line 112
                        among_var = find_among_b(a_2, 4);
                        if (among_var == 0)
                        {
                            cursor = limit - v_1;
                            break;
                        }
                        // ], line 112
                        bra = cursor;
                        // call R2, line 112
                        if (!r_R2())
                        {
                            cursor = limit - v_1;
                            break;
                        }
                        // delete, line 112
                        slice_del();
                        switch (among_var)
                        {
                            case 0:
                                cursor = limit - v_1;
                                subroot = true;
                                break;
                            case 1:
                                // (, line 113
                                // [, line 113
                                ket = cursor;
                                // literal, line 113
                                if (!(eq_s_b(2, "at")))
                                {
                                    cursor = limit - v_1;
                                    subroot = true;
                                    break;
                                }
                                // ], line 113
                                bra = cursor;
                                // call R2, line 113
                                if (!r_R2())
                                {
                                    cursor = limit - v_1;
                                    subroot = true;
                                    break;
                                }
                                // delete, line 113
                                slice_del();
                                break;
                        }
                        if (subroot) { subroot = false; break; }
                    } while (false);
                    break;
                case 6:
                    // (, line 121
                    // call R2, line 122
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 122
                    slice_del();
                    // try, line 123
                    v_2 = limit - cursor;
                    do
                    {
                        // (, line 123
                        // [, line 124
                        ket = cursor;
                        // substring, line 124
                        among_var = find_among_b(a_3, 3);
                        if (among_var == 0)
                        {
                            cursor = limit - v_2;
                            break;
                        }
                        // ], line 124
                        bra = cursor;
                        switch (among_var)
                        {
                            case 0:
                                cursor = limit - v_2;
                                subroot = true;
                                break;
                            case 1:
                                // (, line 127
                                // call R2, line 127
                                if (!r_R2())
                                {
                                    cursor = limit - v_2;
                                    subroot = true;
                                    break;
                                }
                                // delete, line 127
                                slice_del();
                                break;
                        }
                        if (subroot) { subroot = false; break; }
                    } while (false);
                    break;
                case 7:
                    // (, line 133
                    // call R2, line 134
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 134
                    slice_del();
                    // try, line 135
                    v_3 = limit - cursor;
                    do
                    {
                        // (, line 135
                        // [, line 136
                        ket = cursor;
                        // substring, line 136
                        among_var = find_among_b(a_4, 3);
                        if (among_var == 0)
                        {
                            cursor = limit - v_3;
                            break;
                        }
                        // ], line 136
                        bra = cursor;
                        switch (among_var)
                        {
                            case 0:
                                cursor = limit - v_3;
                                subroot = true;
                                break;
                            case 1:
                                // (, line 139
                                // call R2, line 139
                                if (!r_R2())
                                {
                                    cursor = limit - v_3;
                                    subroot = true;
                                    break;
                                }
                                // delete, line 139
                                slice_del();
                                break;
                        }
                        if (subroot) { subroot = false; break; }
                    } while (false);
                    break;
                case 8:
                    // (, line 145
                    // call R2, line 146
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 146
                    slice_del();
                    // try, line 147
                    v_4 = limit - cursor;
                    do
                    {
                        // (, line 147
                        // [, line 148
                        ket = cursor;
                        // literal, line 148
                        if (!(eq_s_b(2, "at")))
                        {
                            cursor = limit - v_4;
                            break;
                        }
                        // ], line 148
                        bra = cursor;
                        // call R2, line 148
                        if (!r_R2())
                        {
                            cursor = limit - v_4;
                            break;
                        }
                        // delete, line 148
                        slice_del();
                    } while (false);
                    break;
                case 9:
                    // (, line 152
                    // call RV, line 153
                    if (!r_RV())
                    {
                        return false;
                    }
                    // literal, line 153
                    if (!(eq_s_b(1, "e")))
                    {
                        return false;
                    }
                    // <-, line 154
                    SliceFrom("ir");
                    break;
            }
            return true;
        }


        private bool r_verb_suffix()
        {
            int among_var;
            int v_1;
            int v_2;
            // setlimit, line 159
            v_1 = limit - cursor;
            // tomark, line 159
            if (cursor < I_pV)
            {
                return false;
            }
            cursor = I_pV;
            v_2 = limit_backward;
            limit_backward = cursor;
            cursor = limit - v_1;
            // (, line 159
            // [, line 160
            ket = cursor;
            // substring, line 160
            among_var = find_among_b(a_6, 120);
            if (among_var == 0)
            {
                limit_backward = v_2;
                return false;
            }
            // ], line 160
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    limit_backward = v_2;
                    return false;
                case 1:
                    // (, line 179
                    // delete, line 179
                    slice_del();
                    break;
            }
            limit_backward = v_2;
            return true;
        }


        private bool r_residual_suffix()
        {
            int among_var;
            // (, line 183
            // [, line 184
            ket = cursor;
            // substring, line 184
            among_var = find_among_b(a_7, 7);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 184
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 187
                    // call RV, line 187
                    if (!r_RV())
                    {
                        return false;
                    }
                    // delete, line 187
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_residual_form()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            // (, line 191
            // [, line 192
            ket = cursor;
            // substring, line 192
            among_var = find_among_b(a_8, 4);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 192
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 194
                    // call RV, line 194
                    if (!r_RV())
                    {
                        return false;
                    }
                    // delete, line 194
                    slice_del();
                    // [, line 194
                    ket = cursor;
                    // or, line 194
                    do
                    {
                        v_1 = limit - cursor;
                        do
                        {
                            // (, line 194
                            // literal, line 194
                            if (!(eq_s_b(1, "u")))
                            {
                                break;
                            }
                            // ], line 194
                            bra = cursor;
                            // test, line 194
                            v_2 = limit - cursor;
                            // literal, line 194
                            if (!(eq_s_b(1, "g")))
                            {
                                break;
                            }
                            cursor = limit - v_2;
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        cursor = limit - v_1;
                        // (, line 195
                        // literal, line 195
                        if (!(eq_s_b(1, "i")))
                        {
                            return false;
                        }
                        // ], line 195
                        bra = cursor;
                        // test, line 195
                        v_3 = limit - cursor;
                        // literal, line 195
                        if (!(eq_s_b(1, "c")))
                        {
                            return false;
                        }
                        cursor = limit - v_3;
                    } while (false);
                    // call RV, line 195
                    if (!r_RV())
                    {
                        return false;
                    }
                    // delete, line 195
                    slice_del();
                    break;
                case 2:
                    // (, line 196
                    // <-, line 196
                    SliceFrom("c");
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
            int v_7;
            int v_8;
            int v_9;
            int v_10;
            // (, line 201
            // do, line 202
            v_1 = cursor;
            do
            {
                // call prelude, line 202
                if (!r_prelude())
                {
                    break;
                }
            } while (false);
            cursor = v_1;
            // do, line 203
            v_2 = cursor;
            do
            {
                // call mark_regions, line 203
                if (!r_mark_regions())
                {
                    break;
                }
            } while (false);
            cursor = v_2;
            // backwards, line 204
            limit_backward = cursor; cursor = limit;
            // (, line 204
            // do, line 205
            v_3 = limit - cursor;
            do
            {
                // (, line 205
                // or, line 209
                do
                {
                    v_4 = limit - cursor;
                    do
                    {
                        // (, line 206
                        // and, line 207
                        v_5 = limit - cursor;
                        // (, line 206
                        // or, line 206
                        do
                        {
                            v_6 = limit - cursor;
                            do
                            {
                                // call standard_suffix, line 206
                                if (!r_standard_suffix())
                                {
                                    break;
                                }
                                subroot = true;
                                if (subroot) break;
                            } while (false);
                            if (subroot) { subroot = false; break; }
                            cursor = limit - v_6;
                            // call verb_suffix, line 206
                            if (!r_verb_suffix())
                            {
                                subroot = true;
                                break;
                            }
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        cursor = limit - v_5;
                        // do, line 207
                        v_7 = limit - cursor;
                        do
                        {
                            // (, line 207
                            // [, line 207
                            ket = cursor;
                            // literal, line 207
                            if (!(eq_s_b(1, "i")))
                            {
                                break;
                            }
                            // ], line 207
                            bra = cursor;
                            // test, line 207
                            v_8 = limit - cursor;
                            // literal, line 207
                            if (!(eq_s_b(1, "c")))
                            {
                                break;
                            }
                            cursor = limit - v_8;
                            // call RV, line 207
                            if (!r_RV())
                            {
                                break;
                            }
                            // delete, line 207
                            slice_del();
                        } while (false);
                        cursor = limit - v_7;
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = limit - v_4;
                    // call residual_suffix, line 209
                    if (!r_residual_suffix())
                    {
                        subroot = true;
                        break;
                    }
                } while (false);
                if (subroot) { subroot = false; break; }
            } while (false);
            cursor = limit - v_3;
            // do, line 211
            v_9 = limit - cursor;
            do
            {
                // call residual_form, line 211
                if (!r_residual_form())
                {
                    break;
                }
            } while (false);
            cursor = limit - v_9;
            cursor = limit_backward;                    // do, line 213
            v_10 = cursor;
            do
            {
                // call postlude, line 213
                if (!r_postlude())
                {
                    break;
                }
            } while (false);
            cursor = v_10;
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
