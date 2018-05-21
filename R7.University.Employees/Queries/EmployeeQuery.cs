//
//  EmployeeQuery.cs
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

using System.Collections.Generic;
using System.Linq;
using R7.Dnn.Extensions.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Employees.Queries
{
    internal class EmployeeQuery: QueryBase
    {
        public EmployeeQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public EmployeeInfo SingleOrDefault (int employeeId)
        {
            return ModelContext.QueryOne<EmployeeInfo> (e => e.EmployeeID == employeeId)
                               .IncludePositions ()
                               .IncludeAchievements ()
                               .IncludeDisciplines ()
                               .SingleOrDefault ();
        }

        public EmployeeInfo SingleOrDefaultByUserId (int userId)
        {
            return ModelContext.QueryOne<EmployeeInfo> (e => e.UserID == userId)
                               .IncludePositions ()
                               .IncludeAchievements ()
                               .IncludeDisciplines ()
                               .SingleOrDefault ();
        }

        public IEnumerable<EmployeeInfo> ListByDivisionId (int divisionId, bool includeSubDivisions, int sortType)
        {
            var spName = includeSubDivisions ? "GetEmployees_ByDivisionID_Recursive" : "GetEmployees_ByDivisionID";
            return ModelContext.Query<EmployeeInfo> ("EXECUTE {objectQualifier}University_" + spName + " {0}, {1}", divisionId, sortType);
        }

        public IList<EmployeeInfo> ListByIds (IEnumerable<int> employeeIds)
        {
        	if (employeeIds.Any ()) {
        		return ModelContext.Query<EmployeeInfo> ()
                                   .IncludeAchievements ()
                                   .IncludePositions ()
                                   .Where (e => employeeIds.Contains (e.EmployeeID))
                                   .ToList ();
        	}

        	return new List<EmployeeInfo> ();
        }
    }
}
