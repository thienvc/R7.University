//
//  DivisionExtensions.cs
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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.Text;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class DivisionExtensions
    {
        public static IEnumerable<TDivision> CalculateLevelAndPath<TDivision> (this IEnumerable<TDivision> divisions)
            where TDivision : IDivisionWritable
        {
            var rootDivisions = divisions.Where (d => d.ParentDivisionID == null);
            foreach (var root in rootDivisions) {
                CalculateLevelAndPath (root, -1, string.Empty);
            }

            return divisions;
        }

        private static void CalculateLevelAndPath<TDivision> (TDivision division, int level, string path)
            where TDivision : IDivisionWritable
        {
            division.Level = level + 1;
            division.Path = path + "/" + division.DivisionID.ToString ().PadLeft (10, '0');

            if (division.SubDivisions != null) {
                foreach (var subDivision in division.SubDivisions) {
                    CalculateLevelAndPath (subDivision, division.Level, division.Path);
                }
            }
        }

        public static VCard VCard (this IDivision division)
        {
            var vcard = new VCard ();

            if (!string.IsNullOrWhiteSpace (division.Title)) {
                vcard.OrganizationName = division.Title;
            }

            if (!string.IsNullOrWhiteSpace (division.Email)) {
                vcard.Emails.Add (division.Email);
            }

            if (!string.IsNullOrWhiteSpace (division.SecondaryEmail)) {
                vcard.Emails.Add (division.SecondaryEmail);
            }

            if (!string.IsNullOrWhiteSpace (division.Phone)) {
                vcard.Phones.Add (new VCardPhone () { Number = division.Phone, Type = VCardPhoneType.Work });
            }

            if (!string.IsNullOrWhiteSpace (division.Fax)) {
                vcard.Phones.Add (new VCardPhone () { Number = division.Fax, Type = VCardPhoneType.Fax });
            }

            if (!string.IsNullOrWhiteSpace (division.WebSite)) {
                vcard.Url = division.WebSite;
            }

            if (!string.IsNullOrWhiteSpace (division.Location)) {
                // TODO: Add organization address
                vcard.DeliveryAddress = division.Location;
            }

            vcard.LastRevision = division.LastModifiedOnDate;

            return vcard;
        }

        public static IDivision GetParentDivision (this IDivision division, IModelContext modelContext)
        {
            Contract.Ensures (Contract.Result<IDivision> () != null);
            if (division.ParentDivisionID != null) {
                return modelContext.Get<DivisionInfo, int> (division.ParentDivisionID.Value);
            }

            return null;
        }

        public static void SetModelId (this IEduProgramDivisionWritable division, ModelType modelType, int modelId)
        {
            if (modelType == ModelType.EduProgram) {
                division.EduProgramId = modelId;
            }
            else if (modelType == ModelType.EduProgramProfile) {
                division.EduProgramProfileId = modelId;
            }
            else {
                throw new ArgumentException ($"Wrong modelType={modelType} argument.");
            }
        }

        public static IEnumerable<IOccupiedPosition> GetHeadEmployeePositions (this IDivision division)
        {
            return division.OccupiedPositions.Where (op => op.PositionID == division.HeadPositionID);
        }

        public static string SearchText (this IDivision division)
        {
            return FormatHelper.JoinNotNullOrEmpty (
                ", ",
                division.Title,
                UniversityModelHelper.HasUniqueShortTitle (division.ShortTitle, division.Title) ? division.ShortTitle : null,
                division.Phone,
                division.Fax,
                division.Email,
                division.SecondaryEmail,
                division.WebSite,
                division.Location,
                division.WorkingHours
            );
        }

        public static string GetSearchUrl (this IDivision division, ModuleInfo module, PortalSettings portalSettings)
        {
            if (!string.IsNullOrEmpty (division.HomePage)) {
                return Globals.NavigateURL (int.Parse (division.HomePage), false, portalSettings, "",
                    portalSettings.PortalAlias.CultureCode);
            }

            return Globals.NavigateURL (module.TabID, false, portalSettings, "",
                portalSettings.PortalAlias.CultureCode, "", "mid", module.ModuleID.ToString ());
        }
    }
}
