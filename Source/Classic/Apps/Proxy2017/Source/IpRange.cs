// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IpRange.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion

namespace Proxy2017
{
    [Serializable]
    public sealed class IpRange
        : ISerializable
    {
        public IPAddress Begin { get; set; }

        public IPAddress End { get; set; }

        public IpRange()
        {
            Begin = new IPAddress(0);
            End = new IPAddress(0);
        }

        public IpRange
            (
                IPAddress begin,
                IPAddress end
            )
        {
            Begin = begin;
            End = end;
        }

        public IpRange
            (
                string ipRangeString
            )
        {
            if (string.IsNullOrEmpty(ipRangeString))
            {
                Begin = new IPAddress(0);
                End = new IPAddress(0);
                return;
            }
            if (ipRangeString == "*")
            {
                Begin = new IPAddress(0);
                End = new IPAddress(0xFFFFFFFF);
                return;
            }

            ipRangeString = ipRangeString.Replace
                (
                    " ",
                    string.Empty
                );

            Match match1 = Regex.Match
                (
                    ipRangeString,
                    @"^(?<addr>[\da-f\.:]+)/(?<maskLen>\d+)$",
                    RegexOptions.IgnoreCase
                );
            if (match1.Success)
            {
                byte[] addressBytes = IPAddress.Parse(match1.Groups["addr"].Value).GetAddressBytes();
                byte[] bitMask = Bits.GetBitMask(addressBytes.Length, int.Parse(match1.Groups["maskLen"].Value));
                addressBytes = Bits.And(addressBytes, bitMask);
                Begin = new IPAddress(addressBytes);
                End = new IPAddress(Bits.Or(addressBytes, Bits.Not(bitMask)));
                return;
            }

            Match match2 = Regex.Match
                (
                    ipRangeString,
                    @"^(?<addr>[\da-f\.:]+)$",
                    RegexOptions.IgnoreCase
                );
            if (match2.Success)
            {
                Begin = End = IPAddress.Parse(ipRangeString);
                return;
            }

            Match match3 = Regex.Match
                (
                    ipRangeString,
                    @"^(?<begin>[\da-f\.:]+)-(?<end>[\da-f\.:]+)$",
                    RegexOptions.IgnoreCase
                );
            if (match3.Success)
            {
                Begin = IPAddress.Parse(match3.Groups["begin"].Value);
                End = IPAddress.Parse(match3.Groups["end"].Value);
                return;
            }

            Match match4 = Regex.Match
                (
                    ipRangeString,
                    @"^(?<addr>[\da-f\.:]+)/(?<mask>[\da-f].:]+)$",
                    RegexOptions.IgnoreCase
                );
            if (match4.Success)
            {
                byte[] addressBytes = IPAddress.Parse(match4.Groups["addr"].Value).GetAddressBytes();
                byte[] bitMask = IPAddress.Parse(match4.Groups["mask"].Value).GetAddressBytes();
                addressBytes = Bits.And(addressBytes, bitMask);
                Begin = new IPAddress(addressBytes);
                End = new IPAddress(Bits.Or(addressBytes, Bits.Not(bitMask)));
                return;
            }

            throw new FormatException("Bad IP range string");
        }

        private IpRange
            (
                SerializationInfo info,
                StreamingContext context
            )
        {
            List<string> names = new List<string>();
            foreach (SerializationEntry entry in info)
            {
                names.Add(entry.Name);
            }

            Func<string, IPAddress> deserialize = name => names.Contains(name)
                ? IPAddress.Parse(info.GetValue(name, typeof(object)).ToString())
                : new IPAddress(0L);

            Begin = deserialize("Begin");
            End = deserialize("End");
        }

        public bool Contains
            (
                IPAddress address
            )
        {
            byte[] bytes = address.GetAddressBytes();
            return Bits.GE(Begin.GetAddressBytes(), bytes)
                   && Bits.LE(End.GetAddressBytes(), bytes);
        }

        public void GetObjectData
            (
                SerializationInfo info,
                StreamingContext context
            )
        {
            info.AddValue("Begin", Begin != null ? Begin.ToString() : string.Empty);
            info.AddValue("End", End != null ? End.ToString() : string.Empty);
        }

        public override string ToString()
        {
            string begin = Begin.ToString();
            string end = End.ToString();

            return (begin == end)
                ? begin
                : string.Concat
                    (
                        begin,
                        "-",
                        end
                    );
        }
    }
}
