//
//  EditDocuments.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.University.ControlExtensions;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Controls
{
    public partial class EditDocuments: 
        GridAndFormControlBase<DocumentInfo,DocumentViewModel>
    {
        #region Control properties

        public string ForModel { get; set; }

        #endregion

        public void OnInit (PortalModuleBase module, IEnumerable<DocumentTypeInfo> documentTypes)
        {
            Module = module;

            var documentTypeViewModels = DocumentTypeViewModel.GetBindableList (documentTypes, ViewModelContext);
            ViewState ["documentTypes"] = XmlSerializationHelper.Serialize (documentTypeViewModels);

            comboDocumentType.DataSource = documentTypeViewModels.OrderBy (dt => dt.LocalizedType);
            comboDocumentType.DataBind ();

            var filenameFormats = new StringBuilder ();
            var first = true;
            foreach (var documentType in documentTypes) {
                if (!first) {
                    filenameFormats.Append (",");
                }
                first = false;
                filenameFormats.Append ($"{{\"id\":\"{documentType.DocumentTypeID}\",\"match\":\"{documentType.FilenameFormat?.Replace ("\\","\\\\")}\"}}");
            }

            comboDocumentType.Attributes.Add ("data-validation", $"[{filenameFormats}]");
            valDocumentUrl.Attributes.Add ("data-message-template", Localization.GetString ("FileName.Invalid", LocalResourceFile));
        }

        protected DocumentTypeViewModel GetDocumentType (int? documentTypeId)
        {
            if (documentTypeId != null) {
                var documentTypes = XmlSerializationHelper.Deserialize<List<DocumentTypeViewModel>> (ViewState ["documentTypes"]);
                return documentTypes.Single (dt => dt.DocumentTypeID == documentTypeId.Value);
            }

            return new DocumentTypeViewModel
            {
                Type = string.Empty,
                DocumentTypeID = Null.NullInteger
            };
        }

        // TODO: Move to the base class by introducing IPublishable
        protected void gridDocuments_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                var document = (IDocument) e.Row.DataItem;
                if (!document.IsPublished (HttpContext.Current.Timestamp)) {
                    e.Row.CssClass = gridDocuments.GetDataRowStyle (e.Row).CssClass + " u8y-not-published";
                }
            }
        }

        #region Implemented abstract members of GridAndFormEditControlBase

        protected override string TargetItemKey
        {
            get { return ForModel; }
        }

        protected override void OnInitControls ()
        {
            InitControls (gridDocuments, hiddenDocumentItemID, 
                          buttonAddDocument, buttonUpdateDocument, buttonCancelEditDocument, buttonResetForm);
        }

        protected override void OnLoadItem (DocumentViewModel item)
        {
            comboDocumentType.SelectByValue (item.DocumentTypeID);
            textDocumentTitle.Text = item.Title;
            textDocumentGroup.Text = item.Group;
            textDocumentSortIndex.Text = item.SortIndex.ToString ();
            datetimeDocumentStartDate.SelectedDate = item.StartDate;
            datetimeDocumentEndDate.SelectedDate = item.EndDate;
            urlDocumentUrl.Url = item.Url;
        }

        protected override void OnUpdateItem (DocumentViewModel item)
        {
            item.Title = textDocumentTitle.Text.Trim ();
            item.Group = textDocumentGroup.Text.Trim ();
            item.DocumentTypeID = int.Parse (comboDocumentType.SelectedValue);
            item.DocumentTypeViewModel = GetDocumentType (item.DocumentTypeID);
            item.SortIndex = TypeUtils.ParseToNullable<int> (textDocumentSortIndex.Text) ?? 0;
            item.StartDate = datetimeDocumentStartDate.SelectedDate;
            item.EndDate = datetimeDocumentEndDate.SelectedDate;
            item.Url = urlDocumentUrl.Url;
        }

        protected override void OnCancelEdit (DocumentViewModel item)
        {
            // fix for DnnUrlControl looses its state on postback
            urlDocumentUrl.Url = item.Url;

            base.OnCancelEdit (item);
        }

        protected override void OnResetForm ()
        {
            textDocumentTitle.Text = string.Empty;
            textDocumentGroup.Text = string.Empty;
            comboDocumentType.SelectedIndex = 0;
            textDocumentSortIndex.Text = "0";
            datetimeDocumentStartDate.SelectedDate = null;
            datetimeDocumentEndDate.SelectedDate = null;
            urlDocumentUrl.UrlType = "F";
        }

        #endregion

        public override void SetData (IEnumerable<DocumentInfo> items, int targetItemId)
        {
            base.SetData (items, targetItemId);

            // speedup adding new documents by autoselecting first document's folder
            if (items.Any ()) {
                var firstItem = items.FirstOrDefault (d => Globals.GetURLType (d.Url) == TabType.File);
                if (firstItem != null) {
                    urlDocumentUrl.Url = firstItem.Url;
                }
            }
        }
    }
}
