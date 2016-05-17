﻿//
// OccupiedPositionInfoEx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.UI.Modules;
using R7.University.ViewModels;

namespace R7.University.Data
{
    [TableName ("vw_University_OccupiedPositions")]
    [PrimaryKey ("OccupiedPositionID", AutoIncrement = false)]
    public class OccupiedPositionInfoEx: OccupiedPositionInfo
    {
        #region Extended (Position and Division) properties

        public string PositionShortTitle { get; set; }

        public string PositionTitle { get; set; }

        public string DivisionShortTitle { get; set; }

        public string DivisionTitle { get; set; }

        public int PositionWeight { get; set; }

        public string HomePage { get; set; }

        public int? ParentDivisionID { get; set; }

        public bool IsTeacher { get; set; }

        #endregion

        public string FormatDivisionLink (IModuleControl module)
        {
            // do not display division title for high-level divisions
            if (ParentDivisionID != null) {
                var strDivision = FormatHelper.FormatShortTitle (DivisionShortTitle, DivisionTitle);
                if (!string.IsNullOrWhiteSpace (HomePage))
                    strDivision = string.Format ("<a href=\"{0}\">{1}</a>", 
                        R7.University.Utilities.Utils.FormatURL (module, HomePage, false), strDivision);

                return strDivision;
            }
              
            return string.Empty;
        }
    }
}

