﻿//
// ViewDivisionDirectory.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.FileSystem;
using R7.University;

namespace R7.University.DivisionDirectory
{
    // TODO: Make module instances co-exist on same page

    public partial class ViewDivisionDirectory : DivisionDirectoryPortalModuleBase
    {
        #region Properties

        protected string SearchText
        {
            get
            { 
                var objSearchText = Session ["DivisionDirectory.SearchText." + TabModuleId];
                return (string) objSearchText ?? string.Empty;
            }
            set { Session ["DivisionDirectory.SearchText." + TabModuleId] = value; }
        }

        protected string SearchDivision
        {
            get
            { 
                var objSearchDivision = Session ["DivisionDirectory.SearchDivision." + TabModuleId];
                return (string) objSearchDivision ?? Null.NullInteger.ToString ();

            }
            set { Session ["DivisionDirectory.SearchDivision." + TabModuleId] = value; }
        }

        protected bool SearchIncludeSubdivisions
        {
            get
            { 
                var objSearchIncludeSubdivisions = Session ["DivisionDirectory.SearchIncludeSubdivisions." + TabModuleId];
                return objSearchIncludeSubdivisions != null ? (bool) objSearchIncludeSubdivisions : false;

            }
            set { Session ["DivisionDirectory.SearchIncludeSubdivisions." + TabModuleId] = value; }
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles Init event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // display search hint
            Utils.Message (this, "SearchHint.Info", MessageType.Info, true); 

            var divisions = DivisionDirectoryController.GetObjects <DivisionInfo> ("ORDER BY [Title] ASC").ToList ();
            divisions.Insert (0, new DivisionInfo {
                DivisionID = Null.NullInteger, 
                Title = LocalizeString ("AllDivisions.Text") 
            });
           
            treeDivisions.DataSource = divisions;
            treeDivisions.DataBind ();

            // REVIEW: Level should be set in settings?
            Utils.ExpandToLevel (treeDivisions, 2);
        }

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
            
            try
            {
                if (!IsPostBack)
                {
                    if (!string.IsNullOrWhiteSpace (SearchText) || !string.IsNullOrWhiteSpace (SearchDivision))
                    {
                        // restore current search
                        textSearch.Text = SearchText;
                        Utils.SelectAndExpandByValue (treeDivisions, SearchDivision);
                        checkIncludeSubdivisions.Checked = SearchIncludeSubdivisions;

                        // perform search
                        if (SearchParamsOK (SearchText, SearchDivision, SearchIncludeSubdivisions, false))
                            DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        protected bool SearchParamsOK (string searchText, string searchDivision, bool includeSubdivisions, bool showMessages = true)
        {
            var divisionIsSpecified = Utils.ParseToNullableInt (searchDivision) != null;
            var searchTextIsEmpty = string.IsNullOrWhiteSpace (searchText);

            // no search params - shouldn't perform search
            if (searchTextIsEmpty && !divisionIsSpecified)
            {
                if (showMessages)
                    Utils.Message (this, "SearchParams.Warning", MessageType.Warning, true);

                gridDivisions.Visible = false;
                return false;
            }
                
            // There are not much divisions as employees, so it's OK to don't check search phrase length
            /*
            if ((!divisionIsSpecified || // no division specified
                (divisionIsSpecified && includeSubdivisions)) && // division specified, but subdivisions flag is set
                (searchTextIsEmpty || // search phrase is empty
                (!searchTextIsEmpty && searchText.Length < 3))) // search phrase is too short
            {
                if (showMessages)
                    Utils.Message (this, "SearchPhrase.Warning", MessageType.Warning, true);

                gridDivisions.Visible = false;
                return false;
            }*/
           
            return true;
        }

        protected void DoSearch (string searchText, string searchDivision, bool includeSubdivisions)
        {
            var divisions = DivisionDirectoryController.FindDivisions (searchText,
                includeSubdivisions, searchDivision); 

            if (divisions == null || !divisions.Any ())
            {
                Utils.Message (this, "NoDivisionsFound.Warning", MessageType.Warning, true);
            }

            gridDivisions.DataSource = divisions;
            gridDivisions.DataBind ();

            // make divisions grid visible anyway
            gridDivisions.Visible = true;
        }

        protected void linkSearch_Click (object sender, EventArgs e)
        {
            var searchText = textSearch.Text.Trim ();
            var searchDivision = (treeDivisions.SelectedNode != null) ? 
                treeDivisions.SelectedNode.Value : Null.NullInteger.ToString ();
            var includeSubdivisions = checkIncludeSubdivisions.Checked;

            if (SearchParamsOK (searchText, searchDivision, includeSubdivisions))
            {
                // save current search
                SearchText = searchText;
                SearchDivision = searchDivision;
                SearchIncludeSubdivisions = includeSubdivisions;

                // perform search
                DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions);
            }
        }

        protected void gridDivisions_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var division = (DivisionInfo) e.Row.DataItem;

                var labelTitle = (Label) e.Row.FindControl ("labelTitle");
                var linkTitle = (HyperLink) e.Row.FindControl ("linkTitle");
                var literalPhone = (Literal) e.Row.FindControl ("literalPhone");
                var linkEmail = (HyperLink) e.Row.FindControl ("linkEmail");
                var literalLocation = (Literal) e.Row.FindControl ("literalLocation");
                var linkDocument =  (HyperLink) e.Row.FindControl ("linkDocument");
                var linkHeadEmployee =  (HyperLink) e.Row.FindControl ("linkHeadEmployee");

                if (!string.IsNullOrWhiteSpace (division.HomePage))
                {
                    linkTitle.NavigateUrl = Utils.FormatURL (this, division.HomePage, false);
                    linkTitle.Text = division.DisplayShortTitle;
                    linkTitle.ToolTip = division.Title;
                    labelTitle.Visible = false;
                }
                else
                {
                    labelTitle.Text = division.DisplayShortTitle;
                    labelTitle.ToolTip = division.Title;
                    linkTitle.Visible = false;
                }

                literalPhone.Text = division.Phone;
                literalLocation.Text = division.Location;

                // email
                if (!string.IsNullOrWhiteSpace (division.Email))
                {
                    linkEmail.Text = division.Email;
                    linkEmail.NavigateUrl = "mailto:" + division.Email;
                }
                else
                    linkEmail.Visible = false;

                // (main) document
                if (!string.IsNullOrWhiteSpace (division.DocumentUrl))
                {
                    linkDocument.Text = LocalizeString ("DocumentUrl.Text");
                    linkDocument.NavigateUrl = Globals.LinkClick (division.DocumentUrl, TabId, ModuleId);

                    // REVIEW: Add GetUrlCssClass() method to the utils
                    // set link CSS class according to file extension
                    if (Globals.GetURLType (division.DocumentUrl) == TabType.File)
                    {
                        var fileId = int.Parse (division.DocumentUrl.Remove (0, "FileId=".Length));
                        var file = FileManager.Instance.GetFile (fileId);
                        if (file != null)
                            linkDocument.CssClass = file.Extension.ToLowerInvariant ();
                    }
                }
                else
                    linkDocument.Visible = false;

                // head employee
                var headEmployee = DivisionDirectoryController.GetHeadEmployee (division.DivisionID);
                if (headEmployee != null)
                {
                    linkHeadEmployee.Text = headEmployee.AbbrName;
                    linkHeadEmployee.ToolTip = headEmployee.FullName;
                    linkHeadEmployee.NavigateUrl = Utils.EditUrl (this, "Details", "employee_id", headEmployee.EmployeeID.ToString ());
                }
            }
        }
    }
}

