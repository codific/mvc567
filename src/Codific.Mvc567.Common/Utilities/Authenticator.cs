// This file is part of the mvc567 distribution (https://github.com/intellisoft567/mvc567).
// Copyright (C) 2019 Codific Ltd.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Codific.Mvc567.Common.Utilities
{
    public static class Authenticator
    {
        private const int IntervalLength = 30;
        private const int PinLength = 6;
        private const string Base32AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private static readonly int PinModulo = (int)Math.Pow(10, PinLength);
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///   Gets number of intervals that have elapsed.
        /// </summary>
        private static long CurrentInterval
        {
            get
            {
                var elapsedSeconds = (long)Math.Floor((DateTime.UtcNow - UnixEpoch).TotalSeconds);

                return elapsedSeconds / IntervalLength;
            }
        }

        public static string GeneratePin(string key)
        {
            return GeneratePin(key.ToByteArray(), CurrentInterval);
        }

        private static string GeneratePin(byte[] key, long counter)
        {
            const int SizeOfInt32 = 4;

            var counterBytes = BitConverter.GetBytes(counter);

            if (BitConverter.IsLittleEndian)
            {
                // spec requires bytes in big-endian order
                Array.Reverse(counterBytes);
            }

            var hash = new HMACSHA1(key).ComputeHash(counterBytes);
            var offset = hash[hash.Length - 1] & 0xF;

            var selectedBytes = new byte[SizeOfInt32];
            Buffer.BlockCopy(hash, offset, selectedBytes, 0, SizeOfInt32);

            if (BitConverter.IsLittleEndian)
            {
                // spec interprets bytes in big-endian order
                Array.Reverse(selectedBytes);
            }

            var selectedInteger = BitConverter.ToInt32(selectedBytes, 0);

            // remove the most significant bit for interoperability per spec
            var truncatedHash = selectedInteger & 0x7FFFFFFF;

            // generate number of digits for given pin length
            var pin = truncatedHash % PinModulo;

            return pin.ToString(CultureInfo.InvariantCulture).PadLeft(PinLength, '0');
        }

        private static byte[] ToByteArray(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new byte[0];
            }

            var bits = input.TrimEnd('=').ToUpper().ToCharArray().Select(c => Convert.ToString(Base32AllowedCharacters.IndexOf(c), 2).PadLeft(5, '0')).Aggregate((a, b) => a + b);
            var result = Enumerable.Range(0, bits.Length / 8).Select(i => Convert.ToByte(bits.Substring(i * 8, 8), 2)).ToArray();
            return result;
        }
    }
}
