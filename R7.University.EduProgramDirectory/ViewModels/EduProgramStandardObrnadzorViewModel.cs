﻿//
// EduProgramStandardObrnadzorViewModel.cs
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
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Tabs;

namespace R7.University.EduProgramDirectory
{
    public class EduProgramStandardObrnadzorViewModel: EduProgramInfo
    {
        public ViewModelContext Context { get; protected set; }

        public EduProgramStandardObrnadzorViewModel (EduProgramInfo ep, ViewModelContext context, ref int order)
        {
            CopyCstor.Copy<EduProgramInfo> (ep, this);
            Context = context;
            OrderString = (++order) + ".";
        }

        public string OrderString { get; protected set; }

        public string EduLevelString
        {
            get { return EduLevel.DisplayShortTitle; }
        }

        public string EduStandardLink
        {
            get
            {
                var eduStandardDocuments = EduStandardDocuments
                    .Where (d => d.IsPublished || Context.ModuleContext.IsEditable).ToList ();
                
                if (eduStandardDocuments != null 
                    && eduStandardDocuments.Count > 0
                    && !string.IsNullOrWhiteSpace (eduStandardDocuments [0].Url))
                {
                    return string.Format ("<a href=\"{0}\"{1}{2} itemprop=\"EduStandartDoc\">{3}</a>",
                        Globals.LinkClick (eduStandardDocuments [0].Url, Context.ModuleContext.TabId, Context.ModuleContext.ModuleId), 
                        Globals.GetURLType (eduStandardDocuments [0].Url) == TabType.Url? " target=\"_blank\"" : string.Empty,
                        !eduStandardDocuments [0].IsPublished? " class=\"not-published-document\"" : string.Empty,
                        Localization.GetString ("EduProgramStandardLink.Text", Context.Control.LocalResourceFile)
                    );
                }

                return string.Empty;
            }
        }
    }
}

