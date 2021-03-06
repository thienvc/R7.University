using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.FileSystem;
using R7.Dnn.Extensions.Text;
using R7.University.Controls.EditModels;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditDocuments: GridAndFormControlBase<DocumentInfo,DocumentEditModel>
    {
        #region Control properties

        public string ForModel { get; set; }

        #endregion

        public void OnInit (PortalModuleBase module, IEnumerable<DocumentTypeInfo> documentTypes)
        {
            Module = module;

            var documentTypeViewModels = DocumentTypeViewModel.GetBindableList (documentTypes, ViewModelContext);
            ViewState ["documentTypes"] = Json.Serialize (documentTypeViewModels);

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

            var lastFolderId = FolderHistory.GetLastFolderId (Request, Module.PortalId);
            if (lastFolderId != null) {
                urlDocumentUrl.SelectFolder (lastFolderId.Value);
            }
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            // HACK: Try to fix GH-348 DnnUrlControl looses its state on postback -
            // this will preserve currently selected folder between edits, but broke general URLs (GH-335)
            if (Page.IsPostBack) {
                urlDocumentUrl.Url = urlDocumentUrl.Url;
            }
        }

        string GetDocumentUrl ()
        {
            if (!string.IsNullOrEmpty (txtDocumentUrl.Text.Trim ())) {
                return txtDocumentUrl.Text.Trim ();
            }
            return urlDocumentUrl.Url;
        }

        void SetDocumentUrl (string url)
        {
            var urlType = Globals.GetURLType (url);
            if (urlType == TabType.Tab || urlType == TabType.File) {
                urlDocumentUrl.Url = url;
                txtDocumentUrl.Text = string.Empty;
            }
            else {
                urlDocumentUrl.Url = string.Empty;
                urlDocumentUrl.UrlType = "N";
                txtDocumentUrl.Text = url;
            }
        }

        protected DocumentTypeViewModel GetDocumentType (int? documentTypeId)
        {
            if (documentTypeId != null) {
                var documentTypes = Json.Deserialize<List<DocumentTypeViewModel>> ((string) ViewState ["documentTypes"]);
                return documentTypes.Single (dt => dt.DocumentTypeID == documentTypeId.Value);
            }

            return new DocumentTypeViewModel
            {
                Type = string.Empty,
                DocumentTypeID = Null.NullInteger
            };
        }

        #region Implemented abstract members of GridAndFormEditControlBase

        protected override string TargetItemKey
        {
            get { return ForModel; }
        }

        protected override void OnLoadItem (DocumentEditModel item)
        {
            comboDocumentType.SelectByValue (item.DocumentTypeID);
            textDocumentTitle.Text = item.Title;
            textDocumentGroup.Text = item.Group;
            textDocumentSortIndex.Text = item.SortIndex.ToString ();
            datetimeDocumentStartDate.SelectedDate = item.StartDate;
            datetimeDocumentEndDate.SelectedDate = item.EndDate;
            SetDocumentUrl (item.Url);
        }

        protected override void OnUpdateItem (DocumentEditModel item)
        {
            item.Title = textDocumentTitle.Text.Trim ();
            item.Group = textDocumentGroup.Text.Trim ();
            item.DocumentTypeID = int.Parse (comboDocumentType.SelectedValue);
            item.DocumentTypeViewModel = GetDocumentType (item.DocumentTypeID);
            item.SortIndex = ParseHelper.ParseToNullable<int> (textDocumentSortIndex.Text) ?? 0;
            item.StartDate = datetimeDocumentStartDate.SelectedDate;
            item.EndDate = datetimeDocumentEndDate.SelectedDate;
            item.Url = GetDocumentUrl ();

            FolderHistory.RememberFolderByFileUrl (Request, Response, item.Url, Module.PortalId);
        }

        protected override void OnResetForm ()
        {
            OnPartialResetForm ();

            textDocumentTitle.Text = string.Empty;
            textDocumentGroup.Text = string.Empty;
            comboDocumentType.SelectedIndex = 0;
        }

        protected override void OnPartialResetForm ()
        {
            base.OnPartialResetForm ();

            // reset only fields within collapsed panel
            textDocumentSortIndex.Text = "0";
            txtDocumentUrl.Text = string.Empty;
            datetimeDocumentStartDate.SelectedDate = null;
            datetimeDocumentEndDate.SelectedDate = null;
        }

        #endregion

        public override void SetData (IEnumerable<DocumentInfo> items, int targetItemId)
        {
            base.SetData (
                items.OrderByDescending (d => d.Group, DocumentGroupComparer.Instance)
                .ThenBy (d => d.DocumentType.DocumentTypeID)
                .ThenBy (d => d.SortIndex)
                .ThenBy (d => d.Title)
                , targetItemId
            );

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
