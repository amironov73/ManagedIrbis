// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EnglishStemmer.cs --
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
    public class EnglishStemmer : StemmerOperations, IStemmer
    {
        private readonly static Among[] a_0 =
        {
            new Among ( "arsen", -1, -1,null ),
            new Among ( "commun", -1, -1, null ),
            new Among ( "gener", -1, -1, null )
        };


        private readonly static Among[] a_1 =
        {
            new Among ( "'", -1, 1, null),
            new Among ( "'s'", 0, 1, null),
            new Among ( "'s", -1, 1, null)
        };


        private readonly static Among[] a_2 =
        {
            new Among ( "ied", -1, 2, null),
            new Among ( "s", -1, 3, null),
            new Among ( "ies", 1, 2, null),
            new Among ( "sses", 1, 1, null),
            new Among ( "ss", 1, -1, null),
            new Among ( "us", 1, -1, null)
        };


        private readonly static Among[] a_3 =
        {
            new Among ( "", -1, 3, null),
            new Among ( "bb", 0, 2, null),
            new Among ( "dd", 0, 2, null),
            new Among ( "ff", 0, 2, null),
            new Among ( "gg", 0, 2, null),
            new Among ( "bl", 0, 1, null),
            new Among ( "mm", 0, 2, null),
            new Among ( "nn", 0, 2, null),
            new Among ( "pp", 0, 2, null),
            new Among ( "rr", 0, 2, null),
            new Among ( "at", 0, 1, null),
            new Among ( "tt", 0, 2, null),
            new Among ( "iz", 0, 1, null)
        };


        private readonly static Among[] a_4 =
        {
            new Among ( "ed", -1, 2, null),
            new Among ( "eed", 0, 1, null),
            new Among ( "ing", -1, 2, null),
            new Among ( "edly", -1, 2, null),
            new Among ( "eedly", 3, 1, null),
            new Among ( "ingly", -1, 2, null)
        };


        private readonly static Among[] a_5 =
        {
            new Among ( "anci", -1, 3, null),
            new Among ( "enci", -1, 2, null),
            new Among ( "ogi", -1, 13, null),
            new Among ( "li", -1, 16, null),
            new Among ( "bli", 3, 12, null),
            new Among ( "abli", 4, 4, null),
            new Among ( "alli", 3, 8, null),
            new Among ( "fulli", 3, 14, null),
            new Among ( "lessli", 3, 15, null),
            new Among ( "ousli", 3, 10, null),
            new Among ( "entli", 3, 5, null),
            new Among ( "aliti", -1, 8, null),
            new Among ( "biliti", -1, 12, null),
            new Among ( "iviti", -1, 11, null),
            new Among ( "tional", -1, 1, null),
            new Among ( "ational", 14, 7, null),
            new Among ( "alism", -1, 8, null),
            new Among ( "ation", -1, 7, null),
            new Among ( "ization", 17, 6, null),
            new Among ( "izer", -1, 6, null),
            new Among ( "ator", -1, 7, null),
            new Among ( "iveness", -1, 11, null),
            new Among ( "fulness", -1, 9, null),
            new Among ( "ousness", -1, 10, null)
        };




        private readonly static Among[] a_6 =
        {
            new Among ( "icate", -1, 4, null),
            new Among ( "ative", -1, 6, null),
            new Among ( "alize", -1, 3, null),
            new Among ( "iciti", -1, 4, null),
            new Among ( "ical", -1, 4, null),
            new Among ( "tional", -1, 1, null),
            new Among ( "ational", 5, 2, null),
            new Among ( "ful", -1, 5, null),
            new Among ( "ness", -1, 5, null)
        };



        private readonly static Among[] a_7 =
        {
            new Among ( "ic", -1, 1, null),
            new Among ( "ance", -1, 1, null),
            new Among ( "ence", -1, 1, null),
            new Among ( "able", -1, 1, null),
            new Among ( "ible", -1, 1, null),
            new Among ( "ate", -1, 1, null),
            new Among ( "ive", -1, 1, null),
            new Among ( "ize", -1, 1, null),
            new Among ( "iti", -1, 1, null),
            new Among ( "al", -1, 1, null),
            new Among ( "ism", -1, 1, null),
            new Among ( "ion", -1, 2, null),
            new Among ( "er", -1, 1, null),
            new Among ( "ous", -1, 1, null),
            new Among ( "ant", -1, 1, null),
            new Among ( "ent", -1, 1, null),
            new Among ( "ment", 15, 1, null),
            new Among ( "ement", 16, 1, null)
        };




        private readonly static Among[] a_8 =
        {
            new Among ( "e", -1, 1, null),
            new Among ( "l", -1, 2, null)
        };


        private readonly static Among[] a_9 =
        {
            new Among ( "succeed", -1, -1, null),
            new Among ( "proceed", -1, -1, null),
            new Among ( "exceed", -1, -1, null),
            new Among ( "canning", -1, -1, null),
            new Among ( "inning", -1, -1, null),
            new Among ( "earring", -1, -1, null),
            new Among ( "herring", -1, -1, null),
            new Among ( "outing", -1, -1, null)
        };



        private readonly static Among[] a_10 =
        {
            new Among ( "andes", -1, -1, null),
            new Among ( "atlas", -1, -1, null),
            new Among ( "bias", -1, -1, null),
            new Among ( "cosmos", -1, -1, null),
            new Among ( "dying", -1, 3, null),
            new Among ( "early", -1, 9, null),
            new Among ( "gently", -1, 7, null),
            new Among ( "howe", -1, -1, null),
            new Among ( "idly", -1, 6, null),
            new Among ( "lying", -1, 4, null),
            new Among ( "news", -1, -1, null),
            new Among ( "only", -1, 10, null),
            new Among ( "singly", -1, 11, null),
            new Among ( "skies", -1, 2, null),
            new Among ( "skis", -1, 1, null),
            new Among ( "sky", -1, -1, null),
            new Among ( "tying", -1, 5, null),
            new Among ( "ugly", -1, 8, null)
        };



        private static readonly char[] g_v = { (char)17, (char)65, (char)16, (char)1 };

        private static readonly char[] g_v_WXY = { (char)1, (char)17, (char)65, (char)208, (char)1 };

        private static readonly char[] g_valid_LI = { (char)55, (char)141, (char)2 };

        private bool B_Y_found;
        private int I_p2;
        private int I_p1;


        private void copy_from(EnglishStemmer other)
        {
            B_Y_found = other.B_Y_found;
            I_p2 = other.I_p2;
            I_p1 = other.I_p1;
            copy_from(other);
        }


        private bool r_prelude()
        {
            bool returnn = false;
            bool subroot = false;
            int v_1;
            int v_2;
            int v_3;
            int v_4;
            int v_5;
            // (, line 25
            // unset Y_found, line 26
            B_Y_found = false;
            // do, line 27
            v_1 = cursor;
            //        lab0:
            do
            {
                // (, line 27
                // [, line 27
                bra = cursor;
                // literal, line 27
                if (!(eq_s(1, "'")))
                {
                    break;
                }
                // ], line 27
                ket = cursor;
                // delete, line 27
                slice_del();
            } while (false);
            cursor = v_1;
            // do, line 28
            v_2 = cursor;
            do
            {
                // (, line 28
                // [, line 28
                bra = cursor;
                // literal, line 28
                if (!(eq_s(1, "y")))
                {
                    break;
                }
                // ], line 28
                ket = cursor;
                SliceFrom("Y");
                // set Y_found, line 28
                B_Y_found = true;
            } while (false);
            cursor = v_2;
            // do, line 29
            v_3 = cursor;
            do
            {
                // repeat, line 29
                replab3:
                while (true)
                {
                    v_4 = cursor;
                    do
                    {
                        // (, line 29
                        // goto, line 29
                        while (true)
                        {
                            v_5 = cursor;
                            do
                            {
                                // (, line 29
                                if (!(InGrouping(g_v, 97, 121)))
                                {
                                    break;
                                }
                                // [, line 29
                                bra = cursor;
                                // literal, line 29
                                if (!(eq_s(1, "y")))
                                {
                                    break;
                                }
                                // ], line 29
                                ket = cursor;
                                cursor = v_5;
                                subroot = true;
                                if (subroot) break;
                            } while (false);
                            if (subroot) { subroot = false; break; }
                            cursor = v_5;
                            if (cursor >= limit)
                            {
                                subroot = true;
                                break;
                            }
                            cursor++;
                        }
                        returnn = true;
                        if (subroot) { subroot = false; break; }

                        SliceFrom("Y");
                        // set Y_found, line 29
                        B_Y_found = true;
                        if (returnn)
                        {
                            goto replab3;
                        }
                    } while (false);
                    cursor = v_4;
                    break;
                }
            } while (false);
            cursor = v_3;
            return true;
        }


        private bool r_mark_regions()
        {
            bool subroot = false;
            int v_1;
            int v_2;
            // (, line 32
            I_p1 = limit;
            I_p2 = limit;
            // do, line 35
            v_1 = cursor;
            do
            {
                // (, line 35
                // or, line 41
                do
                {
                    v_2 = cursor;
                    do
                    {
                        // among, line 36
                        if (find_among(a_0, 3) == 0)
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = v_2;
                    // (, line 41
                    // gopast, line 41
                    while (true)
                    {
                        do
                        {
                            if (!(InGrouping(g_v, 97, 121)))
                            {
                                break;
                            }
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        if (cursor >= limit)
                        {
                            goto breaklab0;
                        }
                        cursor++;
                    }
                    // gopast, line 41
                    while (true)
                    {
                        do
                        {
                            if (!(OutGrouping(g_v, 97, 121)))
                            {
                                break;
                            }
                            //                                    break golab5;
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        if (cursor >= limit)
                        {
                            goto breaklab0;
                        }
                        cursor++;
                    }
                } while (false);
                // setmark p1, line 42
                I_p1 = cursor;
                // gopast, line 43
                while (true)
                {
                    do
                    {
                        if (!(InGrouping(g_v, 97, 121)))
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    if (cursor >= limit)
                    {
                        goto breaklab0;
                    }
                    cursor++;
                }
                // gopast, line 43
                while (true)
                {
                    do
                    {
                        if (!(OutGrouping(g_v, 97, 121)))
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    if (cursor >= limit)
                    {
                        goto breaklab0;
                    }
                    cursor++;
                }
                // setmark p2, line 43
                I_p2 = cursor;
            } while (false);
            breaklab0:
            cursor = v_1;
            return true;
        }


        private bool r_shortv()
        {
            bool subroot = false;
            int v_1;
            // (, line 49
            // or, line 51
            //        lab0: 
            do
            {
                v_1 = limit - cursor;
                do
                {
                    // (, line 50
                    if (!(OutGroupingB(g_v_WXY, 89, 121)))
                    {
                        break;
                    }
                    if (!(InGroupingB(g_v, 97, 121)))
                    {
                        break;
                    }
                    if (!(OutGroupingB(g_v, 97, 121)))
                    {
                        break;
                    }
                    subroot = true;
                    if (subroot) break;
                } while (false);
                if (subroot) { subroot = false; break; }
                cursor = limit - v_1;
                // (, line 52
                if (!(OutGroupingB(g_v, 97, 121)))
                {
                    return false;
                }
                if (!(InGroupingB(g_v, 97, 121)))
                {
                    return false;
                }
                // atlimit, line 52
                if (cursor > limit_backward)
                {
                    return false;
                }
            } while (false);
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


        private bool r_Step_1a()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            int v_2;
            // (, line 58
            // try, line 59
            v_1 = limit - cursor;
            do
            {
                // (, line 59
                // [, line 60
                ket = cursor;
                // substring, line 60
                among_var = find_among_b(a_1, 3);
                if (among_var == 0)
                {
                    cursor = limit - v_1;
                    break;
                }
                // ], line 60
                bra = cursor;
                switch (among_var)
                {
                    case 0:
                        cursor = limit - v_1;
                        subroot = true;
                        break;
                    case 1:
                        // (, line 62
                        // delete, line 62
                        slice_del();
                        break;
                }
                if (subroot) { subroot = false; break; }
            } while (false);
            // [, line 65
            ket = cursor;
            // substring, line 65
            among_var = find_among_b(a_2, 6);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 65
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    SliceFrom("ss");
                    break;
                case 2:
                    // (, line 68
                    // or, line 68
                    //    lab1: 
                    do
                    {
                        v_2 = limit - cursor;
                        do
                        {
                            // (, line 68
                            // hop, line 68
                            {
                                int c = cursor - 2;
                                if (limit_backward > c || c > limit)
                                {
                                    break;
                                }
                                cursor = c;
                            }

                            SliceFrom("i");
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        cursor = limit - v_2;
                        SliceFrom("ie");
                    } while (false);
                    break;
                case 3:
                    // (, line 69
                    // next, line 69
                    if (cursor <= limit_backward)
                    {
                        return false;
                    }
                    cursor--;
                    // gopast, line 69
                    while (true)
                    {
                        do
                        {
                            if (!(InGroupingB(g_v, 97, 121)))
                            {
                                break;
                            }
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        if (cursor <= limit_backward)
                        {
                            return false;
                        }
                        cursor--;
                    }
                    // delete, line 69
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_Step_1b()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            int v_3;
            int v_4;
            // (, line 74
            // [, line 75
            ket = cursor;
            // substring, line 75
            among_var = find_among_b(a_4, 6);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 75
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 77
                    // call R1, line 77
                    if (!r_R1())
                    {
                        return false;
                    }

                    SliceFrom("ee");
                    break;
                case 2:
                    // (, line 79
                    // test, line 80
                    v_1 = limit - cursor;
                    // gopast, line 80
                    while (true)
                    {
                        do
                        {
                            if (!(InGroupingB(g_v, 97, 121)))
                            {
                                break;
                            }
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        if (cursor <= limit_backward)
                        {
                            return false;
                        }
                        cursor--;
                    }
                    cursor = limit - v_1;
                    // delete, line 80
                    slice_del();
                    // test, line 81
                    v_3 = limit - cursor;
                    // substring, line 81
                    among_var = find_among_b(a_3, 13);
                    if (among_var == 0)
                    {
                        return false;
                    }
                    cursor = limit - v_3;
                    switch (among_var)
                    {
                        case 0:
                            return false;
                        case 1:
                            // (, line 83
                            // <+, line 83
                            {
                                int c = cursor;
                                Insert(cursor, cursor, "e");
                                cursor = c;
                            }
                            break;
                        case 2:
                            // (, line 86
                            // [, line 86
                            ket = cursor;
                            // next, line 86
                            if (cursor <= limit_backward)
                            {
                                return false;
                            }
                            cursor--;
                            // ], line 86
                            bra = cursor;
                            // delete, line 86
                            slice_del();
                            break;
                        case 3:
                            // (, line 87
                            // atmark, line 87
                            if (cursor != I_p1)
                            {
                                return false;
                            }
                            // test, line 87
                            v_4 = limit - cursor;
                            // call shortv, line 87
                            if (!r_shortv())
                            {
                                return false;
                            }
                            cursor = limit - v_4;
                            // <+, line 87
                            {
                                int c = cursor;
                                Insert(cursor, cursor, "e");
                                cursor = c;
                            }
                            break;
                    }
                    break;
            }
            return true;
        }


        private bool r_Step_1c()
        {
            bool returnn = false;
            bool subroot = false;
            int v_1;
            int v_2;
            // (, line 93
            // [, line 94
            ket = cursor;
            // or, line 94
            //        lab0: 
            do
            {
                v_1 = limit - cursor;
                do
                {
                    // literal, line 94
                    if (!(eq_s_b(1, "y")))
                    {
                        break;
                    }
                    subroot = true;
                    if (subroot) break;
                } while (false);
                if (subroot) { subroot = false; break; }
                cursor = limit - v_1;
                // literal, line 94
                if (!(eq_s_b(1, "Y")))
                {
                    return false;
                }
            } while (false);
            // ], line 94
            bra = cursor;
            if (!(OutGroupingB(g_v, 97, 121)))
            {
                return false;
            }
            // not, line 95
            {
                v_2 = limit - cursor;
                do
                {
                    returnn = true;
                    // atlimit, line 95
                    if (cursor > limit_backward)
                    {
                        break;
                    }
                    if (returnn)
                    {
                        return false;
                    }
                } while (false);
                cursor = limit - v_2;
            }

            SliceFrom("i");
            return true;
        }


        private bool r_Step_2()
        {
            int among_var;
            // (, line 99
            // [, line 100
            ket = cursor;
            // substring, line 100
            among_var = find_among_b(a_5, 24);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 100
            bra = cursor;
            // call R1, line 100
            if (!r_R1())
            {
                return false;
            }
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 101
                    // <-, line 101
                    SliceFrom("tion");
                    break;
                case 2:
                    // (, line 102
                    // <-, line 102
                    SliceFrom("ence");
                    break;
                case 3:
                    // (, line 103
                    // <-, line 103
                    SliceFrom("ance");
                    break;
                case 4:
                    // (, line 104
                    // <-, line 104
                    SliceFrom("able");
                    break;
                case 5:
                    // (, line 105
                    // <-, line 105
                    SliceFrom("ent");
                    break;
                case 6:
                    // (, line 107
                    // <-, line 107
                    SliceFrom("ize");
                    break;
                case 7:
                    // (, line 109
                    // <-, line 109
                    SliceFrom("ate");
                    break;
                case 8:
                    // (, line 111
                    // <-, line 111
                    SliceFrom("al");
                    break;
                case 9:
                    // (, line 112
                    // <-, line 112
                    SliceFrom("ful");
                    break;
                case 10:
                    // (, line 114
                    // <-, line 114
                    SliceFrom("ous");
                    break;
                case 11:
                    // (, line 116
                    // <-, line 116
                    SliceFrom("ive");
                    break;
                case 12:
                    // (, line 118
                    // <-, line 118
                    SliceFrom("ble");
                    break;
                case 13:
                    // (, line 119
                    // literal, line 119
                    if (!(eq_s_b(1, "l")))
                    {
                        return false;
                    }
                    // <-, line 119
                    SliceFrom("og");
                    break;
                case 14:
                    // (, line 120
                    // <-, line 120
                    SliceFrom("ful");
                    break;
                case 15:
                    // (, line 121
                    // <-, line 121
                    SliceFrom("less");
                    break;
                case 16:
                    // (, line 122
                    if (!(InGroupingB(g_valid_LI, 99, 116)))
                    {
                        return false;
                    }
                    // delete, line 122
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_Step_3()
        {
            int among_var;
            // (, line 126
            // [, line 127
            ket = cursor;
            // substring, line 127
            among_var = find_among_b(a_6, 9);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 127
            bra = cursor;
            // call R1, line 127
            if (!r_R1())
            {
                return false;
            }
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 128
                    // <-, line 128
                    SliceFrom("tion");
                    break;
                case 2:
                    // (, line 129
                    // <-, line 129
                    SliceFrom("ate");
                    break;
                case 3:
                    // (, line 130
                    // <-, line 130
                    SliceFrom("al");
                    break;
                case 4:
                    // (, line 132
                    // <-, line 132
                    SliceFrom("ic");
                    break;
                case 5:
                    // (, line 134
                    // delete, line 134
                    slice_del();
                    break;
                case 6:
                    // (, line 136
                    // call R2, line 136
                    if (!r_R2())
                    {
                        return false;
                    }
                    // delete, line 136
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_Step_4()
        {
            bool subroot = false;
            int among_var;
            int v_1;
            // (, line 140
            // [, line 141
            ket = cursor;
            // substring, line 141
            among_var = find_among_b(a_7, 18);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 141
            bra = cursor;
            // call R2, line 141
            if (!r_R2())
            {
                return false;
            }
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 144
                    // delete, line 144
                    slice_del();
                    break;
                case 2:
                    // (, line 145
                    // or, line 145
                    do
                    {
                        v_1 = limit - cursor;
                        do
                        {
                            // literal, line 145
                            if (!(eq_s_b(1, "s")))
                            {
                                break;
                            }
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        cursor = limit - v_1;
                        // literal, line 145
                        if (!(eq_s_b(1, "t")))
                        {
                            return false;
                        }
                    } while (false);
                    // delete, line 145
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_Step_5()
        {
            bool returnn = false;
            bool subroot = false;
            int among_var;
            int v_1;
            int v_2;
            // (, line 149
            // [, line 150
            ket = cursor;
            // substring, line 150
            among_var = find_among_b(a_8, 2);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 150
            bra = cursor;
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 151
                    // or, line 151
                    do
                    {
                        v_1 = limit - cursor;
                        do
                        {
                            // call R2, line 151
                            if (!r_R2())
                            {
                                break;
                            }
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        cursor = limit - v_1;
                        // (, line 151
                        // call R1, line 151
                        if (!r_R1())
                        {
                            return false;
                        }
                        // not, line 151
                        {
                            v_2 = limit - cursor;
                            do
                            {
                                returnn = true;
                                // call shortv, line 151
                                if (!r_shortv())
                                {
                                    break;
                                }
                                if (returnn)
                                {
                                    return false;
                                }
                            } while (false);
                            cursor = limit - v_2;
                        }
                    } while (false);
                    // delete, line 151
                    slice_del();
                    break;
                case 2:
                    // (, line 152
                    // call R2, line 152
                    if (!r_R2())
                    {
                        return false;
                    }
                    // literal, line 152
                    if (!(eq_s_b(1, "l")))
                    {
                        return false;
                    }
                    // delete, line 152
                    slice_del();
                    break;
            }
            return true;
        }


        private bool r_exception2()
        {
            // (, line 156
            // [, line 158
            ket = cursor;
            // substring, line 158
            if (find_among_b(a_9, 8) == 0)
            {
                return false;
            }
            // ], line 158
            bra = cursor;
            // atlimit, line 158
            if (cursor > limit_backward)
            {
                return false;
            }
            return true;
        }


        private bool r_exception1()
        {
            int among_var;
            // (, line 168
            // [, line 170
            bra = cursor;
            // substring, line 170
            among_var = find_among(a_10, 18);
            if (among_var == 0)
            {
                return false;
            }
            // ], line 170
            ket = cursor;
            // atlimit, line 170
            if (cursor < limit)
            {
                return false;
            }
            switch (among_var)
            {
                case 0:
                    return false;
                case 1:
                    // (, line 174
                    // <-, line 174
                    SliceFrom("ski");
                    break;
                case 2:
                    // (, line 175
                    // <-, line 175
                    SliceFrom("sky");
                    break;
                case 3:
                    SliceFrom("die");
                    break;
                case 4:
                    // (, line 177
                    // <-, line 177
                    SliceFrom("lie");
                    break;
                case 5:
                    // (, line 178
                    // <-, line 178
                    SliceFrom("tie");
                    break;
                case 6:
                    // (, line 182
                    // <-, line 182
                    SliceFrom("idl");
                    break;
                case 7:
                    // (, line 183
                    // <-, line 183
                    SliceFrom("gentl");
                    break;
                case 8:
                    // (, line 184
                    // <-, line 184
                    SliceFrom("ugli");
                    break;
                case 9:
                    // (, line 185
                    // <-, line 185
                    SliceFrom("earli");
                    break;
                case 10:
                    // (, line 186
                    // <-, line 186
                    SliceFrom("onli");
                    break;
                case 11:
                    // (, line 187
                    // <-, line 187
                    SliceFrom("singl");
                    break;
            }
            return true;
        }


        private bool r_postlude()
        {
            bool returnn = false;
            bool subroot = false;
            int v_1;
            int v_2;
            // (, line 203
            // Boolean test Y_found, line 203
            if (!(B_Y_found))
            {
                return false;
            }
            // repeat, line 203
            replab0: while (true)
            {
                v_1 = cursor;
                do
                {
                    // (, line 203
                    // goto, line 203
                    while (true)
                    {
                        v_2 = cursor;
                        do
                        {
                            // (, line 203
                            // [, line 203
                            bra = cursor;
                            // literal, line 203
                            if (!(eq_s(1, "Y")))
                            {
                                break;
                            }
                            // ], line 203
                            ket = cursor;
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
                    returnn = true;
                    if (subroot) { subroot = false; break; }
                    // <-, line 203
                    SliceFrom("y");
                    if (returnn)
                    {
                        goto replab0;
                    }
                } while (false);
                cursor = v_1;
                break;
            }
            return true;
        }


        private bool CanStem()
        {
            bool returnn = true;
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
            int v_12;
            int v_13;
            // (, line 205
            // or, line 207
            do
            {
                v_1 = cursor;
                do
                {
                    // call exception1, line 207
                    if (!r_exception1())
                    {
                        break;
                    }
                    subroot = true;
                    if (subroot) break;
                } while (false);
                if (subroot) { subroot = false; break; }
                cursor = v_1;
                do
                {
                    // not, line 208
                    {
                        v_2 = cursor;
                        do
                        {
                            // hop, line 208
                            {
                                int c = cursor + 3;
                                if (0 > c || c > limit)
                                {
                                    break; ;
                                }
                                cursor = c;
                            }
                            subroot = true;
                            if (subroot) break;
                        } while (false);
                        if (subroot) { subroot = false; break; }
                        cursor = v_2;
                    }
                    returnn = true;
                    if (returnn) goto breaklab0;
                } while (false);
                cursor = v_1;
                // (, line 208
                // do, line 209
                v_3 = cursor;
                do
                {
                    // call prelude, line 209
                    if (!r_prelude())
                    {
                        break;
                    }
                } while (false);
                cursor = v_3;
                // do, line 210
                v_4 = cursor;
                do
                {
                    // call mark_regions, line 210
                    if (!r_mark_regions())
                    {
                        break;
                    }
                } while (false);
                cursor = v_4;
                // backwards, line 211
                limit_backward = cursor; cursor = limit;
                // (, line 211
                // do, line 213
                v_5 = limit - cursor;
                do
                {
                    // call Step_1a, line 213
                    if (!r_Step_1a())
                    {
                        break;
                    }
                } while (false);
                cursor = limit - v_5;
                // or, line 215
                do
                {
                    v_6 = limit - cursor;
                    do
                    {
                        // call exception2, line 215
                        if (!r_exception2())
                        {
                            break;
                        }
                        subroot = true;
                        if (subroot) break;
                    } while (false);
                    if (subroot) { subroot = false; break; }
                    cursor = limit - v_6;
                    // (, line 215
                    // do, line 217
                    v_7 = limit - cursor;
                    do
                    {
                        // call Step_1b, line 217
                        if (!r_Step_1b())
                        {
                            break;
                        }
                    } while (false);
                    cursor = limit - v_7;
                    // do, line 218
                    v_8 = limit - cursor;
                    do
                    {
                        // call Step_1c, line 218
                        if (!r_Step_1c())
                        {
                            break;
                        }
                    } while (false);
                    cursor = limit - v_8;
                    // do, line 220
                    v_9 = limit - cursor;
                    do
                    {
                        // call Step_2, line 220
                        if (!r_Step_2())
                        {
                            break;
                        }
                    } while (false);
                    cursor = limit - v_9;
                    // do, line 221
                    v_10 = limit - cursor;
                    do
                    {
                        // call Step_3, line 221
                        if (!r_Step_3())
                        {
                            break;
                        }
                    } while (false);
                    cursor = limit - v_10;
                    // do, line 222
                    v_11 = limit - cursor;
                    do
                    {
                        // call Step_4, line 222
                        if (!r_Step_4())
                        {
                            break;
                        }
                    } while (false);
                    cursor = limit - v_11;
                    // do, line 224
                    v_12 = limit - cursor;
                    do
                    {
                        // call Step_5, line 224
                        if (!r_Step_5())
                        {
                            break;
                        }
                    } while (false);
                    cursor = limit - v_12;
                } while (false);
                cursor = limit_backward;                        // do, line 227
                v_13 = cursor;
                do
                {
                    // call postlude, line 227
                    if (!r_postlude())
                    {
                        break;
                    }
                } while (false);
                cursor = v_13;
            } while (false);
            breaklab0:
            return true;
        }

        /// <inheritdoc cref="IStemmer.Stem"/>
        public string Stem(string s)
        {
            this.SetCurrent(s.ToLowerInvariant());
            this.CanStem();
            return this.GetCurrent();
        }

    }
}
