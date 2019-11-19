// This file is part of the mvc567 distribution (https://github.com/codific/mvc567).
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

namespace Codific.Mvc567.Dtos.ServiceResults
{
    public class BearerAuthResponse
    {
        public static BearerAuthResponse FailedResult
        {
            get
            {
                return new BearerAuthResponse
                {
                    Success = false,
                    Message = "Authentication failed.",
                };
            }
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public string JsonWebToken { get; set; }

        public string RefreshToken { get; set; }

        public static BearerAuthResponse SuccessResult(string jsonWebToken, string refreshToken)
        {
            return new BearerAuthResponse
            {
                Success = true,
                Message = "Successful authentication.",
                JsonWebToken = jsonWebToken,
                RefreshToken = refreshToken,
            };
        }
    }
}