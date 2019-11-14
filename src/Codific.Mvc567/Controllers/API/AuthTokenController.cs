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

using System.Linq;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Extensions;
using Codific.Mvc567.Dtos.Api;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.API
{
    [Route("/api/auth/token")]
    public class AuthTokenController : Controller
    {
        private readonly IAuthenticationService authenticationService;

        public AuthTokenController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BearerAuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BearerAuthResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestToken([FromBody]TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await this.authenticationService.BuildTokenAsync(tokenRequest.Email, tokenRequest.Password);
                if (result != null)
                {
                    if (result.Success)
                    {
                        return Ok(result);
                    }

                    return BadRequest(result);
                }

                return BadRequest();
            }

            return BadRequest(ModelState.Select(x => x.Value.Errors).ToArray());
        }

        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(typeof(BearerAuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BearerAuthResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenRequest refreshTokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await this.authenticationService.RefreshTokenAsync(HttpContext.GetJwtUserId(), refreshTokenRequest.RefreshToken);
                if (result != null)
                {
                    if (result.Success)
                    {
                        return Ok(result);
                    }

                    return BadRequest(result);
                }

                return BadRequest();
            }

            return BadRequest(ModelState.Select(x => x.Value.Errors).ToArray());
        }

        [HttpPost]
        [Route("validate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ValidateToken()
        {
            return Ok();
        }
    }
}
