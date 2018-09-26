// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FrenchStemmer.cs --
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
    public sealed class FrenchStemmer
        : StemmerOperations,
        IStemmer
    {
        private static readonly FrenchStemmer methodObject = new FrenchStemmer();


        private static readonly Among[] a_0 =
        {
            new Among ( "col", -1, -1, null ),
            new Among ( "par", -1, -1, null ),
            new Among ( "tap", -1, -1, null )
        };


        private static readonly Among[] a_1 =
        {
            new Among ( "", -1, 4, null ),
            new Among ( "I", 0, 1, null ),
            new Among ( "U", 0, 2, null ),
            new Among ( "Y", 0, 3, null )
        };


        private static readonly Among[] a_2 =
        {
            new Among ( "iqU", -1, 3, null ),
            new Among ( "abl", -1, 3, null ),
            new Among ( "I\u00E8r", -1, 4, null ),
            new Among ( "i\u00E8r", -1, 4, null ),
            new Among ( "eus", -1, 2, null ),
            new Among ( "iv", -1, 1, null )
        };


        private static readonly Among[] a_3 =
        {
            new Among ( "ic", -1, 2, null ),
            new Among ( "abil", -1, 1, null ),
            new Among ( "iv", -1, 3, null )
        };


        private static readonly Among[] a_4 =
        {
            new Among ( "iqUe", -1, 1, null ),
            new Among ( "atrice", -1, 2, null ),
            new Among ( "ance", -1, 1, null ),
            new Among ( "ence", -1, 5, null ),
            new Among ( "logie", -1, 3, null ),
            new Among ( "able", -1, 1, null ),
            new Among ( "isme", -1, 1, null ),
            new Among ( "euse", -1, 11, null ),
            new Among ( "iste", -1, 1, null ),
            new Among ( "ive", -1, 8, null ),
            new Among ( "if", -1, 8, null ),
            new Among ( "usion", -1, 4, null ),
            new Among ( "ation", -1, 2, null ),
            new Among ( "ution", -1, 4, null ),
            new Among ( "ateur", -1, 2, null ),
            new Among ( "iqUes", -1, 1, null ),
            new Among ( "atrices", -1, 2, null ),
            new Among ( "ances", -1, 1, null ),
            new Among ( "ences", -1, 5, null ),
            new Among ( "logies", -1, 3, null ),
            new Among ( "ables", -1, 1, null ),
            new Among ( "ismes", -1, 1, null ),
            new Among ( "euses", -1, 11, null ),
            new Among ( "istes", -1, 1, null ),
            new Among ( "ives", -1, 8, null ),
            new Among ( "ifs", -1, 8, null ),
            new Among ( "usions", -1, 4, null ),
            new Among ( "ations", -1, 2, null ),
            new Among ( "utions", -1, 4, null ),
            new Among ( "ateurs", -1, 2, null ),
            new Among ( "ments", -1, 15, null ),
            new Among ( "ements", 30, 6, null ),
            new Among ( "issements", 31, 12, null ),
            new Among ( "it\u00E9s", -1, 7, null ),
            new Among ( "ment", -1, 15, null ),
            new Among ( "ement", 34, 6, null ),
            new Among ( "issement", 35, 12, null ),
            new Among ( "amment", 34, 13, null ),
            new Among ( "emment", 34, 14, null ),
            new Among ( "aux", -1, 10, null ),
            new Among ( "eaux", 39, 9, null ),
            new Among ( "eux", -1, 1, null ),
            new Among ( "it\u00E9", -1, 7, null )
        };


        private static readonly Among[] a_5 =
        {
            new Among ( "ira", -1, 1, null ),
            new Among ( "ie", -1, 1, null ),
            new Among ( "isse", -1, 1, null ),
            new Among ( "issante", -1, 1, null ),
            new Among ( "i", -1, 1, null ),
            new Among ( "irai", 4, 1, null ),
            new Among ( "ir", -1, 1, null ),
            new Among ( "iras", -1, 1, null ),
            new Among ( "ies", -1, 1, null ),
            new Among ( "\u00EEmes", -1, 1, null ),
            new Among ( "isses", -1, 1, null ),
            new Among ( "issantes", -1, 1, null ),
            new Among ( "\u00EEtes", -1, 1, null ),
            new Among ( "is", -1, 1, null ),
            new Among ( "irais", 13, 1, null ),
            new Among ( "issais", 13, 1, null ),
            new Among ( "irions", -1, 1, null ),
            new Among ( "issions", -1, 1, null ),
            new Among ( "irons", -1, 1, null ),
            new Among ( "issons", -1, 1, null ),
            new Among ( "issants", -1, 1, null ),
            new Among ( "it", -1, 1, null ),
            new Among ( "irait", 21, 1, null ),
            new Among ( "issait", 21, 1, null ),
            new Among ( "issant", -1, 1, null ),
            new Among ( "iraIent", -1, 1, null ),
            new Among ( "issaIent", -1, 1, null ),
            new Among ( "irent", -1, 1, null ),
            new Among ( "issent", -1, 1, null ),
            new Among ( "iront", -1, 1, null ),
            new Among ( "\u00EEt", -1, 1, null ),
            new Among ( "iriez", -1, 1, null ),
            new Among ( "issiez", -1, 1, null ),
            new Among ( "irez", -1, 1, null ),
            new Among ( "issez", -1, 1, null )
        };


        private static readonly Among[] a_6 =
        {
            new Among ( "a", -1, 3, null ),
            new Among ( "era", 0, 2, null ),
            new Among ( "asse", -1, 3, null ),
            new Among ( "ante", -1, 3, null ),
            new Among ( "\u00E9e", -1, 2, null ),
            new Among ( "ai", -1, 3, null ),
            new Among ( "erai", 5, 2, null ),
            new Among ( "er", -1, 2, null ),
            new Among ( "as", -1, 3, null ),
            new Among ( "eras", 8, 2, null ),
            new Among ( "\u00E2mes", -1, 3, null ),
            new Among ( "asses", -1, 3, null ),
            new Among ( "antes", -1, 3, null ),
            new Among ( "\u00E2tes", -1, 3, null ),
            new Among ( "\u00E9es", -1, 2, null ),
            new Among ( "ais", -1, 3, null ),
            new Among ( "erais", 15, 2, null ),
            new Among ( "ions", -1, 1, null ),
            new Among ( "erions", 17, 2, null ),
            new Among ( "assions", 17, 3, null ),
            new Among ( "erons", -1, 2, null ),
            new Among ( "ants", -1, 3, null ),
            new Among ( "\u00E9s", -1, 2, null ),
            new Among ( "ait", -1, 3, null ),
            new Among ( "erait", 23, 2, null ),
            new Among ( "ant", -1, 3, null ),
            new Among ( "aIent", -1, 3, null ),
            new Among ( "eraIent", 26, 2, null ),
            new Among ( "\u00E8rent", -1, 2, null ),
            new Among ( "assent", -1, 3, null ),
            new Among ( "eront", -1, 2, null ),
            new Among ( "\u00E2t", -1, 3, null ),
            new Among ( "ez", -1, 2, null ),
            new Among ( "iez", 32, 2, null ),
            new Among ( "eriez", 33, 2, null ),
            new Among ( "assiez", 33, 3, null ),
            new Among ( "erez", 32, 2, null ),
            new Among ( "\u00E9", -1, 2, null )
        };


        private static readonly Among[] a_7 =
        {
            new Among ( "e", -1, 3, null ),
            new Among ( "I\u00E8re", 0, 2, null ),
            new Among ( "i\u00E8re", 0, 2, null ),
            new Among ( "ion", -1, 1, null ),
            new Among ( "Ier", -1, 2, null ),
            new Among ( "ier", -1, 2, null ),
            new Among ( "\u00EB", -1, 4, null )

        };


        private static readonly Among[] a_8 =
        {
            new Among ( "ell", -1, -1, null ),
            new Among ( "eill", -1, -1, null ),
            new Among ( "enn", -1, -1, null ),
            new Among ( "onn", -1, -1, null ),
            new Among ( "ett", -1, -1, null )
        };


        private static readonly char[] g_v = {(char)17, (char)65, (char)16, (char)1, (char)0,
                                                 (char)0,(char)0, (char)0, (char)0, (char)0, (char)0,
                                                 (char)0, (char)0, (char)0,(char)0, (char)128, (char)130,
                                                 (char)103, (char)8, (char)5 };
        private static readonly char[] g_keep_with_s = {(char)1, (char)65, (char)20, (char)0, (char)0, (char)0,
                                                        (char)0, (char)0, (char)0, (char)0, (char)0, (char)0,
                                                        (char)0, (char)0, (char)0, (char)0, (char)128 };

        private int I_p2;
        private int I_p1;
        private int I_pV;


        private void copy_from(FrenchStemmer other)
        {
            I_p2 = other.I_p2;
            I_p1 = other.I_p1;
            I_pV = other.I_pV;
            CopyFrom(other);
        }


        private bool r_prelude()
        {
            bool subroot = false;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            // repeat, line 38
            replab0: while (true)
            {
                v_1 = cursor;
                do
                {
                    // goto, line 38
                    while (true)
                    {
                        v_2 = cursor;
                        do
                        {
                            // (, line 38
                            // or, line 44
                            do
                            {
                                v_3 = cursor;
                                do
                                {
                                    // (, line 40
                                    if (!(InGrouping(g_v, 97, 251)))
                                    {
                                        break;
                                    }
                                    // [, line 40
                                    bra = cursor;
                                    // or, line 40
                                    do
                                    {
                                        v_4 = cursor;
                                        do
                                        {
                                            // (, line 40
                                            // literal, line 40
                                            if (!(eq_s(1, "u")))
                                            {
                                                break;
                                            }
                                            // ], line 40
                                            ket = cursor;
                                            if (!(InGrouping(g_v, 97, 251)))
                                            {
                                                break;
                                            }

                                            SliceFrom("U");
                                            subroot = true;
                                            if (subroot) break;
                                        } while (false);
                                        if (subroot) { subroot = false; break; }
                                        cursor = v_4;
                                        do
                                        {
                                            // (, line 41
                                            // literal, line 41
                                            if (!(eq_s(1, "i")))
                                            {
                                                break;
                                            }
                                            // ], line 41
                                            ket = cursor;
                                            if (!(InGrouping(g_v, 97, 251)))
                                            {
                                                break;
                                            }

                                            SliceFrom("I");
                                            subroot = true;
                                            if (subroot) break;
                                        } while (false);
                                        if (subroot) { subroot = false; break; }
                                        cursor = v_4;
                                        // (, line 42
                                        // literal, line 42
                                        if (!(eq_s(1, "y")))
                                        {
                                            subroot = true;
                                            break;
                                        }
                                        // ], line 42
                                        ket = cursor;
                                        SliceFrom("Y");
                                    } while (false);
                                    if (subroot) { subroot = false; break; }
                                    subroot = true;
                                    if (subroot) break;
                                } while (false);
                                if (subroot) { subroot = false; break; }
                                cursor = v_3;
                                do
                                {
                                    // (, line 45
                                    // [, line 45
                                    bra = cursor;
                                    // literal, line 45
                                    if (!(eq_s(1, "y")))
                                    {
                                        break;
                                    }
                                    // ], line 45
                                    ket = cursor;
                                    if (!(InGrouping(g_v, 97, 251)))
                                    {
                                        break;
                                    }

                                    SliceFrom("Y");
                                    subroot = true;
                                    if (subroot) break;
                                } while (false);
                                if (subroot) { subroot = false; break; }
                                cursor = v_3;
                                // (, line 47
                                // literal, line 47
                                if (!(eq_s(1, "q")))
                                {
                                    subroot = true;
                                    break;
                                }
                                // [, line 47
                                bra = cursor;
                                // literal, line 47
                                if (!(eq_s(1, "u")))
                                {
                                    subroot = true;
                                    break;
                                }
                                // ], line 47
                                ket = cursor;
                                SliceFrom("U");
                            } while (false);
                            if (subroot) { subroot = false; break; }
                            cursor = v_2;
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        cursor = v_2;
                        if (cursor >= limit)
                        {
                            subroot = true;
                            break;
                        }
                        cursor++;
                    }
                    if (subroot)
                    {
                        subroot = false;
                        break;
                    }
                    else if (!subroot)
                    { goto replab0; }
                } while (false);
                cursor = v_1;
                break;
            }
            return true;
        }


        private bool r_mark_regions()
        {

            bool subroot = false;
            bool root = false;
            int v_1;
            int v_2;
            int v_4;
            // (, line 50
            I_pV = limit;
            I_p1 = limit;
            I_p2 = limit;
            // do, line 56
            v_1 = cursor;
            do
            {
                // (, line 56
                // or, line 58
                do
                {
                    v_2 = cursor;
                    do
                    {
                        // (, line 57
                        if (!(InGrouping(g_v, 97, 251)))
                        {
                            break;
                        }
                        if (!(InGrouping(g_v, 97, 251)))
                        {
                            break;
                        }
                        // next, line 57
                        if (cursor >= limit)
                        {
                            break;
                        }
                        cursor++;
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = v_2;
                    do
                    {
                        // among, line 59
                        if (find_among(a_0, 3) == 0)
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = v_2;
                    // (, line 66
                    // next, line 66
                    if (cursor >= limit)
                    {
                        root = true;
                        break;
                    }
                    cursor++;
                    // gopast, line 66
                    while (true)
                    {
                        do
                        {
                            if (!(InGrouping(g_v, 97, 251)))
                            {
                                break;
                            }
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        if (cursor >= limit)
                        {
                            root = true;
                            break;
                        }
                        cursor++;
                    }
                } while (false);
                if (root) { root = false; break; }
                if (subroot) { subroot = false; break; }
                // setmark pV, line 67
                I_pV = cursor;
            } while (false);
            cursor = v_1;
            // do, line 69
            subroot = false;
            v_4 = cursor;
            do
            {
                // (, line 69
                // gopast, line 70
                while (true)
                {
                    do
                    {
                        if (!(InGrouping(g_v, 97, 251)))
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
                // gopast, line 70
                while (true)
                {
                    do
                    {
                        if (!(OutGrouping(g_v, 97, 251)))
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
                // setmark p1, line 70
                I_p1 = cursor;
                // gopast, line 71
                while (true)
                {
                    do
                    {
                        if (!(InGrouping(g_v, 97, 251)))
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
                // gopast, line 71
                while (true)
                {
                    do
                    {
                        if (!(OutGrouping(g_v, 97, 251)))
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
                // setmark p2, line 71
                I_p2 = cursor;
            } while (false);
            cursor = v_4;
            return true;
        }

        private bool r_postlude()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            // repeat, line 75
            replab0: while (true)
            {
                v_1 = cursor;
                do
                {
                    // (, line 75
                    // [, line 77
                    bra = cursor;
                    // substring, line 77
                    among_var = find_among(a_1, 4);
                    if (among_var == 0)
                    {
                        break;
                    }
                    // ], line 77
                    ket = cursor;
                    switch (among_var)
                    {
                        case 0:
                            subroot = true;
                            break;
                        case 1:
                            // (, line 78
                            // <-, line 78
                            SliceFrom("i");
                            break;
                        case 2:
                            // (, line 79
                            // <-, line 79
                            SliceFrom("u");
                            break;
                        case 3:
                            // (, line 80
                            // <-, line 80
                            SliceFrom("y");
                            break;
                        case 4:
                            // (, line 81
                            // next, line 81
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
            int v_5;
            int v_6;
            int v_7;
            int v_8;
            int v_9;
            int v_10;
            int v_11;
            // (, line 91
            // [, line 92
            ket = cursor;
            // substring, line 92
            among_var = find_among_b(a_4, 43);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 92
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 96
                    // call R2, line 96
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 96
                    slice_del();
                    break;
                case 2:
                    // (, line 99
                    // call R2, line 99
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 99
                    slice_del();
                    // try, line 100
                    v_1 = limit - cursor;
                    do
                    {
                        // (, line 100
                        // [, line 100
                        ket = cursor;
                        // literal, line 100
                        if (!(eq_s_b(2, "ic")))
                        {
                            cursor = limit - v_1;
                            break;
                        }
                        // ], line 100
                        bra = cursor;
                        // or, line 100
                        do
                        {
                            v_2 = limit - cursor;
                            do
                            {
                                // (, line 100
                                // call R2, line 100
                                if (!r_R2())
                                {
                                    break;
                                }
                                // delete, line 100
                                slice_del();
                                subroot = true;
                                if (subroot) break;
                            } while (false);

                            if (subroot) { subroot = false; break; }

                            cursor = limit - v_2;
                            // <-, line 100
                            SliceFrom("iqU");
                        } while (false);
                    } while (false);
                    break;
                case 3:
                    // (, line 104
                    // call R2, line 104
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 104
                    SliceFrom("log");
                    break;
                case 4:
                    // (, line 107
                    // call R2, line 107
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 107
                    SliceFrom("u");
                    break;
                case 5:
                    // (, line 110
                    // call R2, line 110
                    if (!r_R2())
                    {
                        return false;
                    }
                    // <-, line 110
                    SliceFrom("ent");
                    break;
                case 6:
                    // (, line 113
                    // call RV, line 114
                    if (!r_RV())
                    {
                        return false;
                    }
                    // delete, line 114
                    slice_del();
                    // try, line 115
                    v_3 = limit - cursor;
                    do
                    {
                        // (, line 115
                        // [, line 116
                        ket = cursor;
                        // substring, line 116
                        among_var = find_among_b(a_2, 6);
                        if (among_var == 0)
                        {
                            cursor = limit - v_3;
                            break;
                        }
                        // ], line 116
                        bra = cursor;
                        switch (among_var)
                        {
                            case 0:
                                cursor = limit - v_3;
                                subroot = true;
                                goto exitLab3;
                            case 1:
                                // (, line 117
                                // call R2, line 117
                                if (!r_R2())
                                {
                                    cursor = limit - v_3;
                                    subroot = true;
                                    goto exitLab3;
                                }
                                // delete, line 117
                                slice_del();
                                // [, line 117
                                ket = cursor;
                                // literal, line 117
                                if (!(eq_s_b(2, "at")))
                                {
                                    cursor = limit - v_3;
                                    subroot = true;
                                    goto exitLab3;
                                }
                                // ], line 117
                                bra = cursor;
                                // call R2, line 117
                                if (!r_R2())
                                {
                                    cursor = limit - v_3;
                                    subroot = true;
                                    goto exitLab3;
                                }
                                // delete, line 117
                                slice_del();
                                break;
                            case 2:
                                // (, line 118
                                // or, line 118
                                do
                                {
                                    v_4 = limit - cursor;
                                    do
                                    {
                                        // (, line 118
                                        // call R2, line 118
                                        if (!r_R2())
                                        {
                                            break;
                                        }
                                        // delete, line 118
                                        slice_del();
                                        subroot = true;
                                        if (subroot) break;
                                    } while (false);

                                    if (subroot) { subroot = false; break; }

                                    cursor = limit - v_4;
                                    // (, line 118
                                    // call R1, line 118
                                    if (!r_R1())
                                    {
                                        cursor = limit - v_3;
                                        subroot = true;
                                        goto exitLab3;
                                    }
                                    // <-, line 118
                                    SliceFrom("eux");
                                } while (false);
                                break;
                            case 3:
                                // (, line 120
                                // call R2, line 120
                                if (!r_R2())
                                {
                                    cursor = limit - v_3;
                                    subroot = true;
                                    goto exitLab3;
                                }
                                // delete, line 120
                                slice_del();
                                break;
                            case 4:
                                // (, line 122
                                // call RV, line 122
                                if (!r_RV())
                                {
                                    cursor = limit - v_3;
                                    subroot = true;
                                    goto exitLab3;
                                }
                                // <-, line 122
                                SliceFrom("i");
                                break;
                        }
                        exitLab3: if (subroot) { subroot = false; break; }
                    } while (false);
                    break;
                case 7:
                    // (, line 128
                    // call R2, line 129
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 129
                    slice_del();
                    // try, line 130
                    v_5 = limit - cursor;
                    do
                    {
                        // (, line 130
                        // [, line 131
                        ket = cursor;
                        // substring, line 131
                        among_var = find_among_b(a_3, 3);
                        if (among_var == 0)
                        {
                            cursor = limit - v_5;
                            break;
                        }
                        // ], line 131
                        bra = cursor;
                        switch (among_var)
                        {
                            case 0:
                                cursor = limit - v_5;
                                subroot = true;
                                break;
                            case 1:
                                // (, line 132
                                // or, line 132
                                do
                                {
                                    v_6 = limit - cursor;
                                    do
                                    {
                                        // (, line 132
                                        // call R2, line 132
                                        if (!r_R2())
                                        {
                                            break;
                                        }
                                        // delete, line 132
                                        slice_del();
                                        subroot = true;
                                        if (subroot) break;
                                    } while (false);

                                    if (subroot) { subroot = false; break; }

                                    cursor = limit - v_6;
                                    // <-, line 132
                                    SliceFrom("abl");
                                } while (false);
                                break;
                            case 2:
                                // (, line 133
                                // or, line 133
                                do
                                {
                                    v_7 = limit - cursor;
                                    do
                                    {
                                        // (, line 133
                                        // call R2, line 133
                                        if (!r_R2())
                                        {
                                            break;
                                        }
                                        // delete, line 133
                                        slice_del();
                                        subroot = true;
                                        if (subroot) break;
                                    } while (false);

                                    if (subroot) { subroot = false; break; }

                                    cursor = limit - v_7;
                                    // <-, line 133
                                    SliceFrom("iqU");
                                } while (false);
                                break;
                            case 3:
                                // (, line 134
                                // call R2, line 134
                                if (!r_R2())
                                {
                                    cursor = limit - v_5;
                                    subroot = true;
                                    break;
                                }
                                // delete, line 134
                                slice_del();
                                break;
                        }
                        if (subroot) { subroot = false; break; }
                    } while (false);
                    break;
                case 8:
                    // (, line 140
                    // call R2, line 141
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 141
                    slice_del();
                    // try, line 142
                    v_8 = limit - cursor;
                    do
                    {
                        // (, line 142
                        // [, line 142
                        ket = cursor;
                        // literal, line 142
                        if (!(eq_s_b(2, "at")))
                        {
                            cursor = limit - v_8;
                            break;
                        }
                        // ], line 142
                        bra = cursor;
                        // call R2, line 142
                        if (!r_R2())
                        {
                            cursor = limit - v_8;
                            break;
                        }
                        // delete, line 142
                        slice_del();
                        // [, line 142
                        ket = cursor;
                        // literal, line 142
                        if (!(eq_s_b(2, "ic")))
                        {
                            cursor = limit - v_8;
                            break;
                        }
                        // ], line 142
                        bra = cursor;
                        // or, line 142
                        do
                        {
                            v_9 = limit - cursor;
                            do
                            {
                                // (, line 142
                                // call R2, line 142
                                if (!r_R2())
                                {
                                    break;
                                }
                                // delete, line 142
                                slice_del();
                                subroot = true;
                                if (subroot) break;
                            } while (false);

                            if (subroot) { subroot = false; break; }

                            cursor = limit - v_9;
                            // <-, line 142
                            SliceFrom("iqU");
                        } while (false);
                    } while (false);
                    break;
                case 9:
                    // (, line 144
                    // <-, line 144
                    SliceFrom("eau");
                    break;
                case 10:
                    // (, line 145
                    // call R1, line 145
                    if (!r_R1())
                    {
                        return false;
                    }
                    // <-, line 145
                    SliceFrom("al");
                    break;
                case 11:
                    // (, line 147
                    // or, line 147
                    do
                    {
                        v_10 = limit - cursor;
                        do
                        {
                            // (, line 147
                            // call R2, line 147
                            if (!r_R2())
                            {
                                break;
                            }
                            // delete, line 147
                            slice_del();
                            subroot = true;
                            if (subroot) break;
                        } while (false);

                        if (subroot) { subroot = false; break; }

                        cursor = limit - v_10;
                        // (, line 147
                        // call R1, line 147
                        if (!r_R1())
                        {
                            return false;
                        }
                        // <-, line 147
                        SliceFrom("eux");
                    } while (false);
                    break;
                case 12:
                    // (, line 150
                    // call R1, line 150
                    if (!r_R1())
                    {
                        return false;
                    }
                    if (!(OutGroupingB(g_v, 97, 251)))
                    {
                        return false;
                    }
                    // delete, line 150
                    slice_del();
                    break;
                case 13:
                    // (, line 155
                    // call RV, line 155
                    if (!r_RV())
                    {
                        return false;
                    }
                    // fail, line 155
                    // (, line 155
                    // <-, line 155
                    SliceFrom("ant");
                    return false;
                case 14:
                    // (, line 156
                    // call RV, line 156
                    if (!r_RV())
                    {
                        return false;
                    }
                    // fail, line 156
                    // (, line 156
                    // <-, line 156
                    SliceFrom("ent");
                    return false;
                case 15:
                    // (, line 158
                    // test, line 158
                    v_11 = limit - cursor;
                    // (, line 158
                    if (!(InGroupingB(g_v, 97, 251)))
                    {
                        return false;
                    }
                    // call RV, line 158
                    if (!r_RV())
                    {
                        return false;
                    }
                    cursor = limit - v_11;
                    // fail, line 158
                    // (, line 158
                    // delete, line 158
                    slice_del();
                    return false;
            }
            return true;
        }

        private bool r_i_verb_suffix()
        {
            int among_var;
            int v_1;
            int v_2;
            // setlimit, line 163
            v_1 = limit - cursor;
            // tomark, line 163
            if (cursor < I_pV)
            {
                return false;
            }
            cursor = I_pV;
            v_2 = limit_backward;
            limit_backward = cursor;
            cursor = limit - v_1;
            // (, line 163
            // [, line 164
            ket = cursor;
            // substring, line 164
            among_var = find_among_b(a_5, 35);
            if (among_var == 0)
            {
                limit_backward = v_2;
                return false;
            }
            // ], line 164
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    limit_backward = v_2;
                    return false;
                case 1:
                    // (, line 170
                    if (!(OutGroupingB(g_v, 97, 251)))
                    {
                        limit_backward = v_2;
                        return false;
                    }
                    // delete, line 170
                    slice_del();
                    break;
            }
            limit_backward = v_2;
            return true;
        }

        private bool r_verb_suffix()
        {
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            // setlimit, line 174
            v_1 = limit - cursor;
            // tomark, line 174
            if (cursor < I_pV)
            {
                return false;
            }
            cursor = I_pV;
            v_2 = limit_backward;
            limit_backward = cursor;
            cursor = limit - v_1;
            // (, line 174
            // [, line 175
            ket = cursor;
            // substring, line 175
            among_var = find_among_b(a_6, 38);
            if (among_var == 0)
            {
                limit_backward = v_2;
                return false;
            }
            // ], line 175
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    limit_backward = v_2;
                    return false;
                case 1:
                    // (, line 177
                    // call R2, line 177
                    if (!r_R2())
                    {
                        limit_backward = v_2;
                        return false;
                    }
                    // delete, line 177
                    slice_del();
                    break;
                case 2:
                    // (, line 185
                    // delete, line 185
                    slice_del();
                    break;
                case 3:
                    // (, line 190
                    // delete, line 190
                    slice_del();
                    // try, line 191
                    v_3 = limit - cursor;
                    //    lab0: 
                    do
                    {
                        // (, line 191
                        // [, line 191
                        ket = cursor;
                        // literal, line 191
                        if (!(eq_s_b(1, "e")))
                        {
                            cursor = limit - v_3;
                            //                                    break lab0;
                            break;
                        }
                        // ], line 191
                        bra = cursor;
                        // delete, line 191
                        slice_del();
                    } while (false);
                    break;
            }
            limit_backward = v_2;
            return true;
        }

        private bool r_residual_suffix()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            // (, line 198
            // try, line 199
            v_1 = limit - cursor;
            do
            {
                // (, line 199
                // [, line 199
                ket = cursor;
                // literal, line 199
                if (!(eq_s_b(1, "s")))
                {
                    cursor = limit - v_1;
                    break;
                }
                // ], line 199
                bra = cursor;
                // test, line 199
                v_2 = limit - cursor;
                if (!(OutGroupingB(g_keep_with_s, 97, 232)))
                {
                    cursor = limit - v_1;
                    break;
                }
                cursor = limit - v_2;
                // delete, line 199
                slice_del();
            } while (false);
            // setlimit, line 200
            v_3 = limit - cursor;
            // tomark, line 200
            if (cursor < I_pV)
            {
                return false;
            }
            cursor = I_pV;
            v_4 = limit_backward;
            limit_backward = cursor;
            cursor = limit - v_3;
            // (, line 200
            // [, line 201
            ket = cursor;
            // substring, line 201
            among_var = find_among_b(a_7, 7);
            if (among_var == 0)
            {
                limit_backward = v_4;
                return false;
            }
            // ], line 201
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    limit_backward = v_4;
                    return false;
                case 1:
                    // (, line 202
                    // call R2, line 202
                    if (!r_R2())
                    {
                        limit_backward = v_4;
                        return false;
                    }
                    // or, line 202
                    do
                    {
                        v_5 = limit - cursor;
                        do
                        {
                            // literal, line 202
                            if (!(eq_s_b(1, "s")))
                            {
                                break;
                            }
                            subroot = true;
                            if (subroot) break;

                        } while (false);

                        if (subroot) { subroot = false; break; }

                        cursor = limit - v_5;
                        // literal, line 202
                        if (!(eq_s_b(1, "t")))
                        {
                            limit_backward = v_4;
                            return false;
                        }
                    } while (false);
                    // delete, line 202
                    slice_del();
                    break;
                case 2:
                    // (, line 204
                    // <-, line 204
                    SliceFrom("i");
                    break;
                case 3:
                    // (, line 205
                    // delete, line 205
                    slice_del();
                    break;
                case 4:
                    // (, line 206
                    // literal, line 206
                    if (!(eq_s_b(2, "gu")))
                    {
                        limit_backward = v_4;
                        return false;
                    }
                    // delete, line 206
                    slice_del();
                    break;
            }
            limit_backward = v_4;
            return true;
        }

        private bool r_un_double()
        {
            int v_1;
            // (, line 211
            // test, line 212
            v_1 = limit - cursor;
            // among, line 212
            if (find_among_b(a_8, 5) == 0)
            {
                return false;
            }
            cursor = limit - v_1;
            // [, line 212
            ket = cursor;
            // next, line 212
            if (cursor <= limit_backward)
            {
                return false;
            }
            cursor--;
            // ], line 212
            bra = cursor;
            // delete, line 212
            slice_del();
            return true;
        }

        private bool r_un_accent()
        {
            bool subroot = false;
            int v_3;
            // (, line 215
            // atleast, line 216
            {
                int v_1 = 1;
                // atleast, line 216
                replab0: while (true)
                {
                    //lab1: 
                    do
                    {
                        if (!(OutGroupingB(g_v, 97, 251)))
                        {
                            break;
                        }
                        v_1--;
                        if (!subroot)
                        {
                            goto replab0;
                        }
                    } while (false);
                    break;
                }
                if (v_1 > 0)
                {
                    return false;
                }
            }
            // [, line 217
            ket = cursor;
            // or, line 217
            do
            {
                v_3 = limit - cursor;
                do
                {
                    // literal, line 217
                    if (!(eq_s_b(1, "\u00E9")))
                    {
                        break;
                    }
                    subroot = true;
                    if (subroot) break;
                } while (false);

                if (subroot) { subroot = false; break; }

                cursor = limit - v_3;
                // literal, line 217
                if (!(eq_s_b(1, "\u00E8")))
                {
                    return false;
                }
            } while (false);
            // ], line 217
            bra = cursor;
            // <-, line 217
            SliceFrom("e");
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
            int v_11;
            // (, line 221
            // do, line 223
            v_1 = cursor;
            do
            {
                // call prelude, line 223
                if (!r_prelude())
                {
                    break;
                }
            } while (false);
            cursor = v_1;
            // do, line 224
            v_2 = cursor;
            do
            {
                // call mark_regions, line 224
                if (!r_mark_regions())
                {
                    break;
                }
            } while (false);
            cursor = v_2;
            // backwards, line 225
            limit_backward = cursor; cursor = limit;
            // (, line 225
            // do, line 227
            v_3 = limit - cursor;
            do
            {
                // (, line 227
                // or, line 237
                do
                {
                    v_4 = limit - cursor;
                    do
                    {
                        // (, line 228
                        // and, line 233
                        v_5 = limit - cursor;
                        // (, line 229
                        // or, line 229
                        do
                        {
                            v_6 = limit - cursor;
                            do
                            {
                                // call standard_suffix, line 229
                                if (!r_standard_suffix())
                                {
                                    break;
                                }
                                subroot = true;
                                if (subroot) break;
                            } while (false);

                            if (subroot) { subroot = false; break; }

                            cursor = limit - v_6;
                            do
                            {
                                // call i_verb_suffix, line 230
                                if (!r_i_verb_suffix())
                                {
                                    break;
                                }
                                subroot = true;
                                if (subroot) break;
                            } while (false);

                            if (subroot) { subroot = false; break; }

                            cursor = limit - v_6;
                            // call verb_suffix, line 231
                            if (!r_verb_suffix())
                            {
                                subroot = true;
                                break;
                            }
                        } while (false);

                        if (subroot) { subroot = false; break; }

                        cursor = limit - v_5;
                        // try, line 234
                        v_7 = limit - cursor;
                        do
                        {
                            // (, line 234
                            // [, line 234
                            ket = cursor;
                            // or, line 234
                            do
                            {
                                v_8 = limit - cursor;
                                do
                                {
                                    // (, line 234
                                    // literal, line 234
                                    if (!(eq_s_b(1, "Y")))
                                    {
                                        break;
                                    }
                                    // ], line 234
                                    bra = cursor;
                                    // <-, line 234
                                    SliceFrom("i");
                                    subroot = true;
                                    if (subroot) break;
                                } while (false);

                                if (subroot) { subroot = false; break; }

                                cursor = limit - v_8;
                                // (, line 235
                                // literal, line 235
                                if (!(eq_s_b(1, "\u00E7")))
                                {
                                    cursor = limit - v_7;
                                    subroot = true;
                                    break;
                                }
                                // ], line 235
                                bra = cursor;
                                // <-, line 235
                                SliceFrom("c");
                            } while (false);
                            if (subroot) { subroot = false; break; }
                        } while (false);
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = limit - v_4;
                    // call residual_suffix, line 238
                    if (!r_residual_suffix())
                    {
                        subroot = true;
                        break;
                    }
                } while (false);

                if (subroot) { subroot = false; break; }

            } while (false);
            cursor = limit - v_3;
            // do, line 243
            v_9 = limit - cursor;
            //        lab11: 
            do
            {
                // call un_double, line 243
                if (!r_un_double())
                {
                    break;
                }
            } while (false);
            cursor = limit - v_9;
            // do, line 244
            v_10 = limit - cursor;
            do
            {
                // call un_accent, line 244
                if (!r_un_accent())
                {
                    break;
                }
            } while (false);
            cursor = limit - v_10;
            cursor = limit_backward;                    // do, line 246
            v_11 = cursor;
            do
            {
                // call postlude, line 246
                if (!r_postlude())
                {
                    break;
                }
            } while (false);
            cursor = v_11;
            return true;
        }


        /// <inheritdoc cref="IStemmer.Stem"/>
        public string Stem(string s)
        {
            SetCurrent(s.ToLowerInvariant());
            CanStem();
            return GetCurrent();
        }

    }
}
