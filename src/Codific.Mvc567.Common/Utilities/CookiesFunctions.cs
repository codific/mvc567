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

using Codific.Mvc567.Common.Utilities;
using System.Diagnostics;
using System.Linq;

namespace Codific.Mvc567.Common.Utilities
{
    public static class CookiesFunctions
    {
        [DebuggerHidden]
        public static string[] GenerateAdminLoginCookieValues(string cookieStringFormat, string[] cookieParams, string secretIndexesString)
        {
            string cookieValue = CryptoFunctions.MD5Hash(string.Format(cookieStringFormat, cookieParams));
            char[] cookieValueArray = cookieValue.ToCharArray();
            int[] secretIndexes = secretIndexesString.Split(',').Select(x => int.Parse(x)).ToArray();
            string cookieName = $"MVC.Allow.{CryptoFunctions.MD5Hash($"{cookieValueArray[secretIndexes[0]]}{cookieValueArray[secretIndexes[1]]}{cookieValueArray[secretIndexes[2]]}_login_cookie_value_{cookieValueArray[secretIndexes[3]]}{cookieValueArray[secretIndexes[4]]}{cookieValueArray[secretIndexes[5]]}")}";

            return new string[] { cookieName, cookieValue };
        }
    }
}
