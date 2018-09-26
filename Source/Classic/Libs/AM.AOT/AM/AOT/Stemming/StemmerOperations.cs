// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StemmerOperations.cs --
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
    public class StemmerOperations
    {

        //    // current string
        internal StringBuilder current;
        internal int cursor;
        internal int limit;
        internal int limit_backward;
        internal int bra;
        internal int ket;



        /// <summary>
        /// Constructor.
        /// </summary>
        protected StemmerOperations()
        {
            current = new StringBuilder();
            SetCurrent("");
        }

        /// <summary>
        /// Set the current string.
        /// </summary>
        protected void SetCurrent
            (
                string value
            )
        {
            //           current.replace(0, current.length(), value);
            //current=current.Replace(current.ToString(), value);
            //current = StringBufferReplace(0, current.Length, current, value);
            //current = StringBufferReplace(0, value.Length, current, value);
            current.Remove(0, current.Length);
            current.Append(value);
            cursor = 0;
            limit = current.Length;
            limit_backward = 0;
            bra = cursor;
            ket = limit;
        }

        /// <summary>
        /// Get the current string.
        /// </summary>
        protected string GetCurrent()
        {
            string result = current.ToString();
            // Make a new StringBuffer.  If we reuse the old one, and a user of
            // the library keeps a reference to the buffer returned (for example,
            // by converting it to a String in a way which doesn't force a copy),
            // the buffer size will not decrease, and we will risk wasting a large
            // amount of memory.
            // Thanks to Wolfram Esser for spotting this problem.
            //current = new  StringBuilder();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CopyFrom
            (
                StemmerOperations other
            )
        {
            current = other.current;
            cursor = other.cursor;
            limit = other.limit;
            limit_backward = other.limit_backward;
            bra = other.bra;
            ket = other.ket;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool InGrouping(char[] s, int min, int max)
        {
            if (cursor >= limit) return false;
            //           char ch = current.charAt(cursor);
            int ch = (int)current[cursor];
            if (ch > max || ch < min) return false;
            //           ch -= min;
            ch -= min;
            if ((s[ch >> 3] & (0X1 << (ch & 0X7))) == 0) return false;
            cursor++;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool InGroupingB(char[] s, int min, int max)
        {
            if (cursor <= limit_backward) return false;
            //           char ch = current.charAt(cursor - 1);
            int ch = (int)current[cursor - 1];
            if (ch > max || ch < min) return false;
            ch -= min;
            if ((s[ch >> 3] & (0X1 << (ch & 0X7))) == 0) return false;
            cursor--;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool OutGrouping(char[] s, int min, int max)
        {
            if (cursor >= limit) return false;
            //           char ch = current.charAt(cursor);
            int ch = (int)current[cursor];
            if (ch > max || ch < min)
            {
                cursor++;
                return true;
            }
            ch -= min;
            if ((s[ch >> 3] & (0X1 << (ch & 0X7))) == 0)
            {
                cursor++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool OutGroupingB(char[] s, int min, int max)
        {
            if (cursor <= limit_backward) return false;
            //           char ch = current.charAt(cursor - 1);
            int ch = (int)current[cursor - 1];
            if (ch > max || ch < min)
            {
                cursor--;
                return true;
            }
            ch -= min;
            if ((s[ch >> 3] & (0X1 << (ch & 0X7))) == 0)
            {
                cursor--;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool InRange(int min, int max)
        {
            if (cursor >= limit) return false;
            //           char ch = current.charAt(cursor);
            int ch = (int)current[cursor];
            if (ch > max || ch < min) return false;
            cursor++;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool InRangeB(int min, int max)
        {
            if (cursor <= limit_backward) return false;
            //           char ch = current.charAt(cursor - 1);
            int ch = (int)current[cursor - 1];
            if (ch > max || ch < min) return false;
            cursor--;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool OutRange(int min, int max)
        {
            if (cursor >= limit) return false;
            //           char ch = current.charAt(cursor);
            int ch = (int)current[cursor];
            if (!(ch > max || ch < min)) return false;
            cursor++;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool OutRangeB(int min, int max)
        {
            if (cursor <= limit_backward) return false;
            //           char ch = current.charAt(cursor - 1);
            int ch = (int)current[cursor - 1];
            if (!(ch > max || ch < min)) return false;
            cursor--;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool eq_s(int s_size, string s)
        {
            if (limit - cursor < s_size) return false;
            int i;
            for (i = 0; i != s_size; i++)
            {
                if (current[cursor + i] != s[i]) return false;
                //               if (current[cursor + i] != s[i]) return false;
            }
            cursor += s_size;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool eq_s_b(int s_size, string s)
        {
            if (cursor - limit_backward < s_size) return false;
            int i;
            for (i = 0; i != s_size; i++)
            {
                //               if (current.charAt(cursor - s_size + i) != s.charAt(i)) return false;
                if (current[cursor - s_size + i] != s[i]) return false;
            }
            cursor -= s_size;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool eq_v(StringBuilder s)
        {
            return eq_s(s.Length, s.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool eq_v_b(StringBuilder s)
        {
            return eq_s_b(s.Length, s.ToString());
        }


        /// <summary>
        /// 
        /// </summary>
        internal int find_among(Among[] v, int v_size)
        {
            int i = 0;
            int j = v_size;

            int c = cursor;
            int l = limit;

            int common_i = 0;
            int common_j = 0;

            bool first_key_inspected = false;
            while (true)
            {
                int k = i + ((j - i) >> 1);
                int diff = 0;
                int common = common_i < common_j ? common_i : common_j; // smaller
                Among w = v[k];
                int i2;

                for (i2 = common; i2 < w.s_size; i2++)
                {
                    if (c + common == l)
                    {
                        diff = -1;
                        break;
                    }
                    diff = current[c + common] - w.s[i2];
                    if (diff != 0) break;
                    common++;
                }
                if (diff < 0)
                {
                    j = k;
                    common_j = common;
                }
                else
                {
                    i = k;
                    common_i = common;
                }
                if (j - i <= 1)
                {
                    if (i > 0) break; // v->s has been inspected
                    if (j == i) break; // only one item in v
                    // - but now we need to go round once more to get
                    // v->s inspected. This looks messy, but is actually
                    // the optimal approach.
                    if (first_key_inspected) break;
                    first_key_inspected = true;
                }
            }
            while (true)
            {
                Among w = v[i];
                if (common_i >= w.s_size)
                {
                    cursor = c + w.s_size;
                    if (w.method == null) return w.result;
                    //bool res;
                    //try
                    //{
                    //    Object resobj = w.method.invoke(w.methodobject,new Object[0]);
                    //    res = resobj.toString().equals("true");
                    //}
                    //catch (InvocationTargetException e)
                    //{
                    //    res = false;
                    //    // FIXME - debug message
                    //}
                    //catch (IllegalAccessException e)
                    //{
                    //    res = false;
                    //// FIXME - debug message
                    //}
                    //cursor = c + w.s_size;
                    //if (res) return w.result;
                }
                i = w.substring_i;
                if (i < 0) return 0;
            }
        }

        //    // find_among_b is for backwards processing. Same comments apply

        internal int find_among_b(Among[] v, int v_size)
        {
            int i = 0;
            int j = v_size;
            int c = cursor;
            int lb = limit_backward;
            int common_i = 0;
            int common_j = 0;
            bool first_key_inspected = false;
            while (true)
            {
                int k = i + ((j - i) >> 1);
                int diff = 0;
                int common = common_i < common_j ? common_i : common_j;
                Among w = v[k];
                int i2;
                for (i2 = w.s_size - 1 - common; i2 >= 0; i2--)
                {
                    if (c - common == lb)
                    {
                        diff = -1;
                        break;
                    }
                    //                   diff = current.charAt(c - 1 - common) - w.s[i2];
                    diff = current[c - 1 - common] - w.s[i2];
                    if (diff != 0) break;
                    common++;
                }
                if (diff < 0)
                {
                    j = k;
                    common_j = common;
                }
                else
                {
                    i = k;
                    common_i = common;
                }
                if (j - i <= 1)
                {
                    if (i > 0) break;
                    if (j == i) break;
                    if (first_key_inspected) break;
                    first_key_inspected = true;
                }
            }
            while (true)
            {
                Among w = v[i];
                if (common_i >= w.s_size)
                {
                    cursor = c - w.s_size;
                    if (w.method == null) return w.result;
                    //boolean res;
                    //try 
                    //{
                    //    Object resobj = w.method.invoke(w.methodobject,
                    //        new Object[0]);
                    //    res = resobj.toString().equals("true");
                    // } 
                    //catch (InvocationTargetException e) 
                    //{
                    //    res = false;
                    //    // FIXME - debug message
                    // } 
                    //catch (IllegalAccessException e) 
                    //{
                    //    res = false;
                    //    // FIXME - debug message
                    // }
                    //cursor = c - w.s_size;
                    //if (res) return w.result;
                }
                i = w.substring_i;
                if (i < 0) return 0;
            }
        }

        /// <summary>
        /// to replace chars between
        /// c_bra and c_ket in current by the
        /// chars in s.
        /// </summary>
        protected int ReplaceS
            (
                int c_bra,
                int c_ket,
                string s
            )
        {
            int adjustment = s.Length - (c_ket - c_bra);
            //           current.replace(c_bra, c_ket, s);
            current = StringBufferReplace(c_bra, c_ket, current, s);
            limit += adjustment;
            if (cursor >= c_ket) cursor += adjustment;
            else if (cursor > c_bra) cursor = c_bra;
            return adjustment;
        }

        private StringBuilder StringBufferReplace
            (
                int start,
                int end,
                StringBuilder s,
                string s1
            )
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < start; i++)
            {
#if WINMOBILE || PocketPC

                string tmp = s[i].ToString();
                sb.Insert(sb.Length, tmp);

#else

                sb.Insert(sb.Length, s[i]);

#endif
            }
            //           for (int i = 1; i < end - start + 1; i++)
            //           {
            sb.Insert(sb.Length, s1);
            //           }
            for (int i = end; i < s.Length; i++)
            {
#if WINMOBILE || PocketPC

                string tmp = s[i].ToString();
                sb.Insert(sb.Length, tmp);

#else

                sb.Insert(sb.Length, s[i]);

#endif
            }

            return sb;
            //string temp = s.ToString();
            //temp = temp.Substring(start - 1, end - start + 1);
            //s = s.Replace(temp, s1, start - 1, end - start + 1);
            //return s;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SliceCheck()
        {
            if (bra < 0 ||
                bra > ket ||
                ket > limit ||
                limit > current.Length)   // this line could be removed
            {
                //System.err.println("faulty slice operation");
                // FIXME: report error somehow.
                /*
                    fprintf(stderr, "faulty slice operation:\n");
                    debug(z, -1, 0);
                    exit(1);
                    */
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SliceFrom
            (
                string s
            )
        {
            SliceCheck();
            ReplaceS(bra, ket, s);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SliceFrom
            (
                StringBuilder s
            )
        {
            SliceFrom(s.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        protected void slice_del()
        {
            SliceFrom("");
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Insert
            (
                int c_bra,
                int c_ket,
                string s
            )
        {
            int adjustment = ReplaceS(c_bra, c_ket, s);
            if (c_bra <= bra) bra += adjustment;
            if (c_bra <= ket) ket += adjustment;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Insert
            (
                int c_bra,
                int c_ket,
                StringBuilder s
            )
        {
            Insert(c_bra, c_ket, s.ToString());
        }

        /// <summary>
        /// Copy the slice into the supplied
        /// <see cref="StringBuilder"/>.
        /// </summary>
        protected StringBuilder SliceTo
            (
                StringBuilder s
            )
        {
            SliceCheck();
            int len = ket - bra;
            //           s.replace(0, s.length(), current.substring(bra, ket));
            //           int lengh = string.IsNullOrEmpty(s.ToString())!= true ? s.Length : 0;
            //           if (ket == current.Length) ket--;
            //string ss = current.ToString().Substring(bra, len);
            //StringBufferReplace(0, s.Length, s, ss);
            //return s;
            return StringBufferReplace(0, s.Length, s, current.ToString().Substring(bra, len));
            //           return StringBufferReplace(0, lengh, s, current.ToString().Substring(bra, ket));
            //           return s;
        }

        //    /* Copy the slice into the supplied StringBuilder */
        //protected StringBuilder slice_to(StringBuilder s)
        //{
        //    slice_check();
        //    int len = ket - bra;
        //    s.replace(0, s.length(), current.substring(bra, ket));
        //    return s;
        //}

        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder AssignTo
            (
                StringBuilder s
            )
        {
            //s.replace(0, s.length(), current.substring(0, limit));
            //return s;
            return StringBufferReplace(0, s.Length, s, current.ToString().Substring(0, limit));
        }

        //    protected StringBuilder assign_to(StringBuilder s)
        //    {
        //    s.replace(0, s.length(), current.substring(0, limit));
        //    return s;
        //    }

        //
        //extern void debug(struct SN_env * z, int number, int line_count)
        //{   int i;
        //    int limit = SIZE(z->p);
        //    //if (number >= 0) printf("%3d (line %4d): '", number, line_count);
        //    if (number >= 0) printf("%3d (line %4d): [%d]'", number, line_count,limit);
        //    for (i = 0; i <= limit; i++)
        //    {   if (z->lb == i) printf("{");
        //        if (z->bra == i) printf("[");
        //        if (z->c == i) printf("|");
        //        if (z->ket == i) printf("]");
        //        if (z->l == i) printf("}");
        //        if (i < limit)
        //        {   int ch = z->p[i];
        //            if (ch == 0) ch = '#';
        //            printf("%c", ch);
        //        }
        //    }
        //    printf("'\n");
        //}
        //*/

        //};

        // METHODS FOR CZECH STEMMER AGRESSIVE

        /// <summary>
        /// 
        /// </summary>
        protected void RemoveDerivational()
        {
            int len = current.Length;
            if ((len > 8) &&
                current.ToString().Substring(len - 6, 6).Equals("obinec"))
            {
                current = current.Remove(len - 6, 6);
                return;
            }//len >8
            if (len > 7)
            {
                if (current.ToString().Substring(len - 5, 5).Equals("ion\u00e1\u0159"))
                { // -ionář 

                    current = current.Remove(len - 4, 4);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 5, 5).Equals("ovisk") ||
                        current.ToString().Substring(len - 5, 5).Equals("ovstv") ||
                        current.ToString().Substring(len - 5, 5).Equals("ovi\u0161t") ||  //-ovišt
                        current.ToString().Substring(len - 5, 5).Equals("ovn\u00edk"))
                { //-ovník

                    current = current.Remove(len - 5, 5);
                    return;
                }
            }//len>7
            if (len > 6)
            {
                if (current.ToString().Substring(len - 4, 4).Equals("\u00e1sek") || // -ásek 
                    current.ToString().Substring(len - 4, 4).Equals("loun") ||
                    current.ToString().Substring(len - 4, 4).Equals("nost") ||
                    current.ToString().Substring(len - 4, 4).Equals("teln") ||
                    current.ToString().Substring(len - 4, 4).Equals("ovec") ||
                    current.ToString().Substring(len - 5, 5).Equals("ov\u00edk") || //-ovík
                    current.ToString().Substring(len - 4, 4).Equals("ovtv") ||
                    current.ToString().Substring(len - 4, 4).Equals("ovin") ||
                    current.ToString().Substring(len - 4, 4).Equals("\u0161tin"))
                { //-štin

                    current = current.Remove(len - 4, 4);
                    return;
                }
                if (current.ToString().Substring(len - 4, 4).Equals("enic") ||
                        current.ToString().Substring(len - 4, 4).Equals("inec") ||
                        current.ToString().Substring(len - 4, 4).Equals("itel"))
                {

                    current = current.Remove(len - 3, 3);
                    Palatalise();
                    return;
                }
            }//len>6
            if (len > 5)
            {
                if (current.ToString().Substring(len - 3, 3).Equals("\u00e1rn"))
                { //-árn
                    current = current.Remove(len - 3, 3);
                    return;
                }
                if (current.ToString().Substring(len - 3, 3).Equals("\u011bnk"))
                { //-ěnk

                    current = current.Remove(len - 2, 2);
                    Palatalise();

                    return;
                }
                if (current.ToString().Substring(len - 3, 3).Equals("i\u00e1n") || //-ián
                        current.ToString().Substring(len - 3, 3).Equals("ist") ||
                        current.ToString().Substring(len - 3, 3).Equals("isk") ||
                        current.ToString().Substring(len - 3, 3).Equals("i\u0161t") || //-išt
                        current.ToString().Substring(len - 3, 3).Equals("itb") ||
                        current.ToString().Substring(len - 3, 3).Equals("\u00edrn"))
                {  //-írn

                    current = current.Remove(len - 2, 2);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 3, 3).Equals("och") ||
                        current.ToString().Substring(len - 3, 3).Equals("ost") ||
                        current.ToString().Substring(len - 3, 3).Equals("ovn") ||
                        current.ToString().Substring(len - 3, 3).Equals("oun") ||
                        current.ToString().Substring(len - 3, 3).Equals("out") ||
                        current.ToString().Substring(len - 3, 3).Equals("ou\u0161"))
                {  //-ouš

                    current = current.Remove(len - 3, 3);
                    return;
                }
                if (current.ToString().Substring(len - 3, 3).Equals("u\u0161k"))
                { //-ušk

                    current = current.Remove(len - 3, 3);
                    return;
                }
                if (current.ToString().Substring(len - 3, 3).Equals("kyn") ||
                    current.ToString().Substring(len - 3, 3).Equals("\u010dan") ||    //-čan
                    current.ToString().Substring(len - 3, 3).Equals("k\u00e1\u0159") || //kář
                    current.ToString().Substring(len - 3, 3).Equals("n\u00e9\u0159") || //néř
                    current.ToString().Substring(len - 3, 3).Equals("n\u00edk") ||      //-ník
                    current.ToString().Substring(len - 3, 3).Equals("ctv") ||
                    current.ToString().Substring(len - 3, 3).Equals("stv"))
                {

                    current = current.Remove(len - 3, 3);
                    return;
                }
            }//len>5
            if (len > 4)
            {
                if (current.ToString().Substring(len - 2, 2).Equals("\u00e1\u010d") || // -áč
                    current.ToString().Substring(len - 2, 2).Equals("a\u010d") ||      //-ač
                    current.ToString().Substring(len - 2, 2).Equals("\u00e1n") ||      //-án
                        current.ToString().Substring(len - 2, 2).Equals("an") ||
                        current.ToString().Substring(len - 2, 2).Equals("\u00e1\u0159") || //-ář
                        current.ToString().Substring(len - 2, 2).Equals("as"))
                {

                    current = current.Remove(len - 2, 2);
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("ec") ||
                        current.ToString().Substring(len - 2, 2).Equals("en") ||
                        current.ToString().Substring(len - 2, 2).Equals("\u011bn") ||   //-ěn
                        current.ToString().Substring(len - 2, 2).Equals("\u00e9\u0159"))
                {  //-éř

                    current = current.Remove(len - 1, 1);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("\u00ed\u0159") || //-íř
                        current.ToString().Substring(len - 2, 2).Equals("ic") ||
                        current.ToString().Substring(len - 2, 2).Equals("in") ||
                        current.ToString().Substring(len - 2, 2).Equals("\u00edn") ||  //-ín
                        current.ToString().Substring(len - 2, 2).Equals("it") ||
                        current.ToString().Substring(len - 2, 2).Equals("iv"))
                {

                    current = current.Remove(len - 1, 1);
                    Palatalise();
                    return;
                }

                if (current.ToString().Substring(len - 2, 2).Equals("ob") ||
                        current.ToString().Substring(len - 2, 2).Equals("ot") ||
                        current.ToString().Substring(len - 2, 2).Equals("ov") ||
                        current.ToString().Substring(len - 2, 2).Equals("o\u0148"))
                { //-oň 

                    current = current.Remove(len - 2, 2);
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("ul"))
                {

                    current = current.Remove(len - 2, 2);
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("yn"))
                {

                    current = current.Remove(len - 2, 2);
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("\u010dk") ||              //-čk
                        current.ToString().Substring(len - 2, 2).Equals("\u010dn") ||  //-čn
                        current.ToString().Substring(len - 2, 2).Equals("dl") ||
                        current.ToString().Substring(len - 2, 2).Equals("nk") ||
                        current.ToString().Substring(len - 2, 2).Equals("tv") ||
                        current.ToString().Substring(len - 2, 2).Equals("tk") ||
                        current.ToString().Substring(len - 2, 2).Equals("vk"))
                {

                    current = current.Remove(len - 2, 2);
                    return;
                }
            }//len>4
            if (len > 3)
            {
                if (current.ToString()[current.Length - 1] == 'c' ||
                   current.ToString()[current.Length - 1] == '\u010d' || //-č
                   current.ToString()[current.Length - 1] == 'k' ||
                   current.ToString()[current.Length - 1] == 'l' ||
                   current.ToString()[current.Length - 1] == 'n' ||
                   current.ToString()[current.Length - 1] == 't')
                {

                    current = current.Remove(len - 1, 1);
                    return;
                }
            }//len>3	
        }//removeDerivational

        /// <summary>
        /// 
        /// </summary>
        protected void RemoveAugmentative()
        {
            int len = current.Length;
            //
            if ((len > 6) &&
                 current.ToString().Substring(len - 4, 4).Equals("ajzn"))
            {

                current = current.Remove(len - 4, 4);
                return;
            }
            if ((len > 5) &&
                (current.ToString().Substring(len - 3, 3).Equals("izn") ||
                 current.ToString().Substring(len - 3, 3).Equals("isk")))
            {

                current = current.Remove(len - 2, 2);
                Palatalise();
                return;
            }
            if ((len > 4) &&
                 current.ToString().Substring(len - 2, 2).Equals("\00e1k"))
            { //-ák

                current = current.Remove(len - 2, 2);
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void RemoveDiminutive()
        {
            int len = current.Length;
            // 
            if ((len > 7) &&
                 current.ToString().Substring(len - 5, 5).Equals("ou\u0161ek"))
            {  //-oušek

                current = current.Remove(len - 5, 5);
                return;
            }
            if (len > 6)
            {
                if (current.ToString().Substring(len - 4, 4).Equals("e\u010dek") ||      //-eček
                   current.ToString().Substring(len - 4, 4).Equals("\u00e9\u010dek") ||    //-éček
                   current.ToString().Substring(len - 4, 4).Equals("i\u010dek") ||         //-iček
                   current.ToString().Substring(len - 4, 4).Equals("\u00ed\u010dek") ||    //íček
                   current.ToString().Substring(len - 4, 4).Equals("enek") ||
                   current.ToString().Substring(len - 4, 4).Equals("\u00e9nek") ||      //-ének
                   current.ToString().Substring(len - 4, 4).Equals("inek") ||
                   current.ToString().Substring(len - 4, 4).Equals("\u00ednek"))
                {      //-ínek

                    current = current.Remove(len - 3, 3);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 4, 4).Equals("\u00e1\u010dek") || //áček
                     current.ToString().Substring(len - 4, 4).Equals("a\u010dek") ||   //aček
                     current.ToString().Substring(len - 4, 4).Equals("o\u010dek") ||   //oček
                     current.ToString().Substring(len - 4, 4).Equals("u\u010dek") ||   //uček
                     current.ToString().Substring(len - 4, 4).Equals("anek") ||
                     current.ToString().Substring(len - 4, 4).Equals("onek") ||
                     current.ToString().Substring(len - 4, 4).Equals("unek") ||
             current.ToString().Substring(len - 4, 4).Equals("\u00e1nek"))
                {   //-ánek

                    current = current.Remove(len - 4, 4);
                    return;
                }
            }//len>6
            if (len > 5)
            {
                if (current.ToString().Substring(len - 3, 3).Equals("e\u010dk") ||   //-ečk
                   current.ToString().Substring(len - 3, 3).Equals("\u00e9\u010dk") ||  //-éčk 
                   current.ToString().Substring(len - 3, 3).Equals("i\u010dk") ||   //-ičk
                   current.ToString().Substring(len - 3, 3).Equals("\u00ed\u010dk") ||    //-íčk
                   current.ToString().Substring(len - 3, 3).Equals("enk") ||   //-enk
                   current.ToString().Substring(len - 3, 3).Equals("\u00e9nk") ||  //-énk 
                   current.ToString().Substring(len - 3, 3).Equals("ink") ||   //-ink
                   current.ToString().Substring(len - 3, 3).Equals("\u00ednk"))
                {   //-ínk

                    current = current.Remove(len - 3, 3);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 3, 3).Equals("\u00e1\u010dk") ||  //-áčk
                    current.ToString().Substring(len - 3, 3).Equals("au010dk") || //-ačk
                    current.ToString().Substring(len - 3, 3).Equals("o\u010dk") ||  //-očk
                    current.ToString().Substring(len - 3, 3).Equals("u\u010dk") ||   //-učk 
                    current.ToString().Substring(len - 3, 3).Equals("ank") ||
                    current.ToString().Substring(len - 3, 3).Equals("onk") ||
                    current.ToString().Substring(len - 3, 3).Equals("unk"))
                {

                    current = current.Remove(len - 3, 3);
                    return;

                }
                if (current.ToString().Substring(len - 3, 3).Equals("\u00e1tk") || //-átk
                   current.ToString().Substring(len - 3, 3).Equals("\u00e1nk") ||  //-ánk
           current.ToString().Substring(len - 3, 3).Equals("u\u0161k"))
                {   //-ušk

                    current = current.Remove(len - 3, 3);
                    return;
                }
            }//len>5
            if (len > 4)
            {
                if (current.ToString().Substring(len - 2, 2).Equals("ek") ||
                   current.ToString().Substring(len - 2, 2).Equals("\u00e9k") ||  //-ék
                   current.ToString().Substring(len - 2, 2).Equals("\u00edk") ||  //-ík
                   current.ToString().Substring(len - 2, 2).Equals("ik"))
                {

                    current = current.Remove(len - 1, 1);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("\u00e1k") ||  //-ák
                    current.ToString().Substring(len - 2, 2).Equals("ak") ||
                    current.ToString().Substring(len - 2, 2).Equals("ok") ||
                    current.ToString().Substring(len - 2, 2).Equals("uk"))
                {

                    current = current.Remove(len - 1, 1);
                    return;
                }
            }
            if ((len > 3) &&
                 current.ToString().Substring(len - 1, 1).Equals("k"))
            {

                current = current.Remove(len - 1, 1);
                return;
            }
        }//removeDiminutives

        /// <summary>
        /// 
        /// </summary>
        protected void RemoveComparative()
        {
            int len = current.Length;
            // 
            if ((len > 5) &&
                (current.ToString().Substring(len - 3, 3).Equals("ej\u0161") ||  //-ejš
                 current.ToString().Substring(len - 3, 3).Equals("\u011bj\u0161")))
            {   //-ějš

                current = current.Remove(len - 2, 2);
                Palatalise();
                return;
            }
        }

        private void Palatalise()
        {
            int len = current.Length;

            if (current.ToString().Substring(len - 2, 2).Equals("ci") ||
                 current.ToString().Substring(len - 2, 2).Equals("ce") ||
                 current.ToString().Substring(len - 2, 2).Equals("\u010di") ||      //-či
                 current.ToString().Substring(len - 2, 2).Equals("\u010de"))
            {   //-če

                current = StringBufferReplace(len - 2, len, current, "k");
                return;
            }
            if (current.ToString().Substring(len - 2, 2).Equals("zi") ||
                 current.ToString().Substring(len - 2, 2).Equals("ze") ||
                 current.ToString().Substring(len - 2, 2).Equals("\u017ei") ||    //-ži
                 current.ToString().Substring(len - 2, 2).Equals("\u017ee"))
            {  //-že

                current = StringBufferReplace(len - 2, len, current, "h");
                return;
            }
            if (current.ToString().Substring(len - 3, 3).Equals("\u010dt\u011b") ||     //-čtě
                 current.ToString().Substring(len - 3, 3).Equals("\u010dti") ||   //-čti
                 current.ToString().Substring(len - 3, 3).Equals("\u010dt\u00ed"))
            {   //-čtí

                current = StringBufferReplace(len - 3, len, current, "ck");
                return;
            }
            if (current.ToString().Substring(len - 2, 2).Equals("\u0161t\u011b") ||   //-ště
                current.ToString().Substring(len - 2, 2).Equals("\u0161ti") ||   //-šti
                 current.ToString().Substring(len - 2, 2).Equals("\u0161t\u00ed"))
            {  //-ští

                current = StringBufferReplace(len - 2, len, current, "sk");
                return;
            }
            current = current.Remove(len - 1, 1);
            return;
        }//palatalise

        /// <summary>
        /// 
        /// </summary>
        protected void RemovePossessives()
        {
            int len = current.Length;

            if (len > 5)
            {
                if (current.ToString().Substring(len - 2, 2).Equals("ov"))
                {
                    current = current.Remove(len - 2, 2);
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("\u016fv"))
                { //-ův
                    current = current.Remove(len - 2, 2);
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("in"))
                {
                    current = current.Remove(len - 1, 1);
                    Palatalise();
                    return;
                }
            }
        }//removePossessives

        /// <summary>
        /// 
        /// </summary>
        protected void RemoveCase()
        {
            int len = current.Length;
            // 
            if ((len > 7) &&
                 current.ToString().Substring(len - 5, 5).Equals("atech"))
            {
                current = current.Remove(len - 5, 5);
                return;
            }//len>7
            if (len > 6)
            {
                if (current.ToString().Substring(len - 4, 4).Equals("\u011btem"))
                {   //-ětem

                    current = current.Remove(len - 3, 3);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 4, 4).Equals("at\u016fm"))
                {  //-atům
                    current = current.Remove(len - 4, 4);
                    return;
                }
            }
            if (len > 5)
            {
                if (current.ToString().Substring(len - 3, 3).Equals("ech") ||
                      current.ToString().Substring(len - 3, 3).Equals("ich") ||
              current.ToString().Substring(len - 3, 3).Equals("\u00edch"))
                { //-ích

                    current = current.Remove(len - 2, 2);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 3, 3).Equals("\u00e9ho") || //-ého
                    current.ToString().Substring(len - 3, 3).Equals("\u011bmi") ||  //-ěmu
                    current.ToString().Substring(len - 3, 3).Equals("emi") ||
                    current.ToString().Substring(len - 3, 3).Equals("\u00e9mu") ||  // -ému				                                                                current.substring( len-3,len).equals("ete")||
                    current.ToString().Substring(len - 3, 3).Equals("eti") ||
                    current.ToString().Substring(len - 3, 3).Equals("iho") ||
                    current.ToString().Substring(len - 3, 3).Equals("\u00edho") ||  //-ího
                    current.ToString().Substring(len - 3, 3).Equals("\u00edmi") ||  //-ími
                    current.ToString().Substring(len - 3, 3).Equals("imu"))
                {

                    current = current.Remove(len - 2, 2);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 3, 3).Equals("\u00e1ch") || //-ách
                    current.ToString().Substring(len - 3, 3).Equals("ata") ||
                    current.ToString().Substring(len - 3, 3).Equals("aty") ||
                    current.ToString().Substring(len - 3, 3).Equals("\u00fdch") ||   //-ých
                    current.ToString().Substring(len - 3, 3).Equals("ama") ||
                    current.ToString().Substring(len - 3, 3).Equals("ami") ||
                    current.ToString().Substring(len - 3, 3).Equals("ov\u00e9") ||   //-ové
                    current.ToString().Substring(len - 3, 3).Equals("ovi") ||
                    current.ToString().Substring(len - 3, 3).Equals("\u00fdmi"))
                {  //-ými

                    current = current.Remove(len - 3, 3);
                    return;
                }
            }
            if (len > 4)
            {
                if (current.ToString().Substring(len - 2, 2).Equals("em"))
                {

                    current = current.Remove(len - 1, 1);
                    Palatalise();
                    return;

                }
                if (current.ToString().Substring(len - 2, 2).Equals("es") ||
                    current.ToString().Substring(len - 2, 2).Equals("\u00e9m") ||    //-ém
                    current.ToString().Substring(len - 2, 2).Equals("\u00edm"))
                {   //-ím

                    current = current.Remove(len - 2, 2);
                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("\u016fm"))
                {

                    current = current.Remove(len - 2, 2);
                    return;
                }
                if (current.ToString().Substring(len - 2, 2).Equals("at") ||
                    current.ToString().Substring(len - 2, 2).Equals("\u00e1m") ||    //-ám
                    current.ToString().Substring(len - 2, 2).Equals("os") ||
                    current.ToString().Substring(len - 2, 2).Equals("us") ||
                    current.ToString().Substring(len - 2, 2).Equals("\u00fdm") ||     //-ým
                    current.ToString().Substring(len - 2, 2).Equals("mi") ||
                    current.ToString().Substring(len - 2, 2).Equals("ou"))
                {

                    current = current.Remove(len - 2, 2);
                    return;
                }
            }//len>4
            if (len > 3)
            {
                if (current.ToString().Substring(len - 1, 1).Equals("e") ||
                   current.ToString().Substring(len - 1, 1).Equals("i"))
                {

                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 1, 1).Equals("\u00ed") ||    //-é
                    current.ToString().Substring(len - 1, 1).Equals("\u011b"))
                {   //-ě

                    Palatalise();
                    return;
                }
                if (current.ToString().Substring(len - 1, 1).Equals("u") ||
                    current.ToString().Substring(len - 1, 1).Equals("y") ||
                    current.ToString().Substring(len - 1, 1).Equals("\u016f"))
                {   //-ů

                    current = current.Remove(len - 1, 1);
                    return;
                }
                if (current.ToString().Substring(len - 1, 1).Equals("a") ||
                    current.ToString().Substring(len - 1, 1).Equals("o") ||
                    current.ToString().Substring(len - 1, 1).Equals("\u00e1") ||  // -á
                    current.ToString().Substring(len - 1, 1).Equals("\u00e9") ||  //-é
                    current.ToString().Substring(len - 1, 1).Equals("\u00fd"))
                {   //-ý

                    current = current.Remove(len - 1, 1);
                    return;
                }
            }//len>3
        }
    }
}
