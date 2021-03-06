//
//  QueryableExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace R7.University.Queries
{
    public static class QueryableExtensions
    {
        // TODO: Should be removed after encapsulating all Include/ThenInclude inside extensions
        public static IQueryable<T> Include2<T,TProperty> (this IQueryable<T> source, Expression<Func<T,TProperty>> path) where T: class
        {
            return EntityFrameworkQueryableExtensions.Include (source, path);
        }
    }
}
