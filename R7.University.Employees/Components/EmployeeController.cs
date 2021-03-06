//
//  EmployeeController.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2018 Roman M. Yagodin
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
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search.Entities;
using R7.University.Employees.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Employees.Components
{
    public class EmployeeController : ModuleSearchBase
    {
        #region ModuleSearchBase implementaion

        public override IList<SearchDocument> GetModifiedSearchDocuments (ModuleInfo moduleInfo, DateTime beginDateUtc)
        {
            var searchDocs = new List<SearchDocument> ();
            var settings = new EmployeeSettingsRepository ().GetSettings (moduleInfo);
            var portalSettings = HttpOffContextHelper.GetPortalSettings (moduleInfo.PortalID, moduleInfo.TabID, moduleInfo.CultureCode);

            var employee = default (EmployeeInfo);
            using (var modelContext = new UniversityModelContext ()) {
                employee = modelContext.Get<EmployeeInfo,int> (settings.EmployeeID);
            }

            if (employee != null && employee.LastModifiedOnDate.ToUniversalTime () > beginDateUtc.ToUniversalTime ()) {
                var sd = new SearchDocument {
                    PortalId = moduleInfo.PortalID,
                    AuthorUserId = employee.LastModifiedByUserId,
                    Title = employee.FullName (),
                    Body = employee.SearchText (),
                    ModifiedTimeUtc = employee.LastModifiedOnDate.ToUniversalTime (),
                    UniqueKey = string.Format ("University_Employee_{0}", employee.EmployeeID),
                    Url = employee.GetSearchUrl (moduleInfo, portalSettings),
                    IsActive = employee.IsPublished (DateTime.Now)
                };
	
                searchDocs.Add (sd);
            }
            return searchDocs;
        }

        #endregion
    }
}

