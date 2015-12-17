﻿//
// EduProgramProfileInfo.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
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
using System.Collections.Generic;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University
{
    public interface IEduProgramProfile
    {
        int EduProgramProfileID { get; set; }

        int EduProgramID { get; set; }

        string ProfileCode { get; set; }

        string ProfileTitle { get; set; }

        DateTime? AccreditedToDate { get; set; }

        DateTime? CommunityAccreditedToDate { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }

        // TODO: Use IEduProgram interface here
        EduProgramInfo EduProgram { get; set; }

        IList<IEduProgramProfileForm> EduProgramProfileForms { get; set; }
    }

    [TableName ("University_EduProgramProfiles")]
    [PrimaryKey ("EduProgramProfileID", AutoIncrement = true)]
    public class EduProgramProfileInfo: UniversityEntityBase, IEduProgramProfile
    {
        #region IEduProgramProfile implementation

        public int EduProgramProfileID { get; set; }

        public int EduProgramID { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        public DateTime? AccreditedToDate { get; set; }

        public DateTime? CommunityAccreditedToDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [IgnoreColumn]
        public EduProgramInfo EduProgram { get; set; }

        [IgnoreColumn] 
        public IList<IEduProgramProfileForm> EduProgramProfileForms { get; set; }

        #endregion

        [IgnoreColumn]
        public string EduProgramProfileString
        {
            get 
            {
                if (EduProgram != null)
                {
                    return FormatEduProgramProfile (EduProgram.Code, EduProgram.Title, ProfileCode, ProfileTitle); 
                }

                return FormatEduProgramProfile (string.Empty, string.Empty, ProfileCode, ProfileTitle);
            }
        }

        public static string FormatEduProgramProfile (string code, string title, 
            string profileCode, string profileTitle)
        {
            var profileString = Utils.FormatList (" ", profileCode, profileTitle);
            var profileStringInBrackets = 
                !string.IsNullOrWhiteSpace (profileString)? "(" + profileString + ")" : string.Empty;
                
            return Utils.FormatList (" ", code, title, profileStringInBrackets);
        }
    }
}

