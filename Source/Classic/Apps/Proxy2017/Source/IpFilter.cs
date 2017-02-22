// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IpFilter.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Proxy2017
{
    [Serializable]
    public sealed class IpFilter
    {
        public List<IpRange> Ranges { get { return _ranges; } }

        private readonly List<IpRange> _ranges = new List<IpRange>();

        public bool AllowAnyAddress { get; set; }

        public IpFilter()
        {
        }

        public IpFilter
            (
                string specification
            )
        {
            if (string.IsNullOrEmpty(specification)
                || (specification == "*"))
            {
                AllowAnyAddress = true;
                return;
            }

            string[] parts = specification.Split(';', ',');
            foreach (string part in parts)
            {
                if (!string.IsNullOrEmpty(part))
                {
                    Ranges.Add(new IpRange(part));
                }
            }
        }

        public bool Allowed
            (
                IPAddress address
            )
        {
            if (AllowAnyAddress)
            {
                return true;
            }

            foreach (IpRange range in Ranges)
            {
                if (range.Contains(address))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Allowed
            (
                EndPoint endPoint
            )
        {
            IPEndPoint ip = endPoint as IPEndPoint;
            if (ReferenceEquals(ip, null))
            {
                return false;
            }
            return Allowed(ip.Address);
        }

        public override string ToString()
        {
            if (AllowAnyAddress)
            {
                return "*";
            }
            StringBuilder result = new StringBuilder();
            bool first = true;
            foreach (IpRange range in Ranges)
            {
                if (!first)
                {
                    result.Append(";");
                }
                result.Append(range);
                first = false;
            }
            return result.ToString();
        }
    }
}
