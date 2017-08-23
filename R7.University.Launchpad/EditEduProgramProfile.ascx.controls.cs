﻿using System.Web.UI.WebControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University.Controls;

namespace R7.University.Launchpad
{
    public partial class EditEduProgramProfile
    {
        protected LinkButton buttonUpdate;
        protected LinkButton buttonDelete;
        protected HyperLink linkCancel;
        protected TextBox textProfileCode;
        protected TextBox textProfileTitle;
        protected TextBox textLanguages;
        protected DnnDateTimePicker datetimeStartDate;
        protected DnnDateTimePicker datetimeEndDate;
        protected DnnDatePicker dateAccreditedToDate;
        protected DnnDatePicker dateCommunityAccreditedToDate;
        protected DropDownList comboEduProgram;
        protected DropDownList comboEduProgramLevel;
        protected DropDownList comboEduLevel;
        protected EditDocuments formEditDocuments;
        protected EditEduForms formEditEduForms;
        protected EditDivisions formEditDivisions;
        protected ModuleAuditControl auditControl;
    }
}
