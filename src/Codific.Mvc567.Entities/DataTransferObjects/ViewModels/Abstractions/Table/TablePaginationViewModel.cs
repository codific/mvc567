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

namespace Codific.Mvc567.Entities.ViewModels.Abstractions.Table
{
    public class TablePaginationViewModel
    {
        public TablePaginationViewModel(int currentPage, int pagesCount)
        {
            CurrentPage = currentPage;

            if (CurrentPage != 1)
            {
                PreviousPage = CurrentPage - 1;
            }
            if (CurrentPage != pagesCount)
            {
                NextPage = pagesCount;
            }
            for (int i = 1; i <= 2; i++)
            {
                if (CurrentPage - i >= 1)
                {
                    PreviousPagesCount++;
                }
                if (CurrentPage + i <= pagesCount)
                {
                    NextPagesCount++;
                }
            }
        }

        public int CurrentPage { get; private set; }

        public int? NextPage { get; private set; }

        public int? PreviousPage { get; private set; }

        public int NextPagesCount { get; private set; }

        public int PreviousPagesCount { get; private set; }
    }
}
