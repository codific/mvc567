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

using Codific.Mvc567.DataAccess.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Codific.Mvc567.DataAccess.Core.Repositories
{
    public abstract class RepositoryBase<TContext> : IRepositoryInjection<TContext>
        where TContext : DbContext
    {
        private DbContext context;

        protected RepositoryBase(TContext context)
        {
            this.context = context;
        }

        public DbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IRepositoryInjection<TContext> SetContext(TContext context)
        {
            this.context = context;
            return this;
        }
    }
}
