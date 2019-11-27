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

using System;
using System.Threading.Tasks;
using AutoMapper;
using Codific.Mvc567.DataAccess.Abstraction;
using Codific.Mvc567.Entities.Database;
using Codific.Mvc567.Services.Abstractions;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class LogService : Service, ILogService
    {
        public LogService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper)
        {
        }

        public async Task<bool> ClearAllLogsAsync()
        {
            try
            {
                await this.StandardRepository.DeleteAllAsync<Log>();
                await this.Uow.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await this.LogErrorAsync(ex, nameof(this.ClearAllLogsAsync));
                return false;
            }
        }
    }
}