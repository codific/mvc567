// This file is part of the mvc567 distribution (https://github.com/intellisoft567/mvc567).
// Copyright (C) 2019 Georgi Karagogov
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
using Mvc567.DataAccess.Abstraction;
using Mvc567.Entities.Database;
using Mvc567.Services.Abstractions;

namespace Mvc567.Services.Infrastructure
{
    public class LogService : AbstractService, ILogService
    {
        public LogService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {

        }

        public async Task<bool> ClearAllLogsAsync()
        {
            try
            {
                await this.standardRepository.DeleteAllAsync<Log>();
                await this.uow.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, nameof(ClearAllLogsAsync));
                return false;
            }
        }
    }
}
