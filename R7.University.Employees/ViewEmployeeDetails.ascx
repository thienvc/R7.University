<%@ Control Language="C#" AutoEventWireup="false" EnableViewState="false" CodeBehind="ViewEmployeeDetails.ascx.cs" Inherits="R7.University.Employees.ViewEmployeeDetails" %>
<%@ Register TagPrefix="dnn" TagName="JavaScriptLibraryInclude" Src="~/admin/Skins/JavaScriptLibraryInclude.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="controls" TagName="AgplSignature" Src="~/DesktopModules/MVC/R7.University/R7.University.Controls/AgplSignature.ascx" %>

<dnn:JavaScriptLibraryInclude runat="server" Name="React" />
<dnn:JavaScriptLibraryInclude runat="server" Name="ReactDOM" />
<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/css/module.css" />
<dnn:DnnJsInclude runat="server" FilePath="~/DesktopModules/MVC/R7.University/R7.University/assets/js/WorkbookDownloader.min.js" />

<asp:Panel id="panelEmployeeDetails" runat="server" CssClass="dnnForm dnnClear u8y-employee-details">
    <div class="row no-gutters">
		<div class="col-md-3 mb-3 mb-md-0">
    	    <div class="border bg-light rounded p-3">
				<asp:Image id="imagePhoto" runat="server" CssClass="img-fluid d-block mx-auto" />
				<asp:Panel id="panelContacts" runat="server" CssClass="u8y-employee-contacts">
					<div class="_section">
    					<asp:HyperLink id="linkEmail" runat="server" CssClass="email _email" />
        				<asp:HyperLink id="linkSecondaryEmail" runat="server" CssClass="email _email" />
        				<asp:HyperLink id="linkWebSite" runat="server" Target="_blank" CssClass="_website" />
                        <asp:HyperLink id="linkUserProfile" runat="server" resourcekey="VisitProfile.Text" Target="_blank" CssClass="_userprofile more" />
        			</div>
                    <div class="_section">
        				<asp:Label id="labelMessenger" runat="server" CssClass="_label" />
        			</div>
        			<div class="_section">
        				<asp:Label id="labelPhone" runat="server" CssClass="_label" />
        				<asp:Label id="labelFax" runat="server" CssClass="_label" />
        				<asp:Label id="labelCellPhone" runat="server" CssClass="_label" />
        			</div>
                    <div class="_section">
        				<asp:Label id="labelWorkingPlaceAndHours" runat="server" CssClass="_label" />
        			</div>
				</asp:Panel>
				<asp:HyperLink id="linkBarcode" runat="server" resourcekey="Barcode.Action" role="button"
			        CssClass="btn btn-outline-secondary btn-block btn-sm btn-barcode" data-toggle="modal" />
				<button type="button" class="btn btn-outline-secondary btn-block btn-sm" data-toggle="modal" data-target="#u8y_employee_wbdl_dlg_<%: ModuleId %>">
					<i class="fas fa-file-excel">Export to .XLSX</i>
				</button>
			</div>
		</div>
    	<div id="employeeTabs_<%= ModuleId %>" class="col-md-9 pl-md-3">
            <asp:Literal id="literalFullName" runat="server" />
    		<ul class="nav nav-pills u8y-employee-details-tabs" role="tablist">
    		    <li class="nav-item"><a class="nav-link active" role="tab" data-toggle="pill" href="#employeeCommon-<%= ModuleId %>" aria-controls="employeeCommon-<%= ModuleId %>" aria-selected="true"><%= LocalizeString("Common.Tab") %></a></li>
    			<li class="nav-item" id="tabExperience" runat="server"><a class="nav-link" role="tab" data-toggle="pill" href="#employeeExperience-<%= ModuleId %>" aria-controls="employeeExperience-<%= ModuleId %>" aria-selected="false"><%= LocalizeString("Experience.Tab") %></a></li>
    			<li class="nav-item" id="tabAchievements" runat="server"><a class="nav-link" role="tab" data-toggle="pill" href="#employeeAchievements-<%= ModuleId %>" aria-controls="employeeAchievements-<%= ModuleId %>" aria-selected="false"><%= LocalizeString("Achievements.Tab") %></a></li>
    			<li class="nav-item" id="tabDisciplines" runat="server"><a class="nav-link" role="tab" data-toggle="pill" href="#employeeDisciplines-<%= ModuleId %>" aria-controls="employeeDisciplines-<%= ModuleId %>" aria-selected="false"><%= LocalizeString("Disciplines.Tab") %></a></li>
    			<li class="nav-item" id="tabAbout" runat="server"><a class="nav-link" role="tab" data-toggle="pill" href="#employeeAbout-<%= ModuleId %>" aria-controls="employeeAbout-<%= ModuleId %>" aria-selected="false"><%= LocalizeString("About.Tab") %></a></li>
    		</ul>
			<div class="tab-content">
        		<div id="employeeCommon-<%= ModuleId %>" class="tab-pane fade show active" role="tabpanel">
                    <p><asp:Label id="labelAcademicDegreeAndTitle" runat="server" /></p>
					<asp:Panel id="panelPositions" runat="server" CssClass="_section">
                        <label><%: LocalizeString ("OccupiedPositions.Text") %></label>
            			<asp:Repeater id="repeaterPositions" runat="server" OnItemDataBound="repeaterPositions_ItemDataBound">
            				<HeaderTemplate><ul></HeaderTemplate>
            				<ItemTemplate>
            					<li>
            						<asp:Label id="labelPosition" runat="server" />
            						<asp:Label id="labelDivision" runat="server" />
            						<asp:HyperLink id="linkDivision" runat="server" Target="_blank" />
            					</li>
            				</ItemTemplate>
            				<FooterTemplate></ul></FooterTemplate>
            			</asp:Repeater>
						<asp:Panel id="pnlScienceIndexCounter" runat="server" CssClass="u8y-science-index-counter d-inline-block border rounded p-2">
							<!--Science Index counter-->
							<script type="text/javascript"><!--
							document.write('<a href="https://elibrary.ru/author_counter_click.asp?id=<%: Employee.ScienceIndexAuthorId %>"'+
							' target=_blank><img src="https://elibrary.ru/author_counter.aspx?id=<%: Employee.ScienceIndexAuthorId %>&rand='+
							Math.random()+'" title="<%: LocalizeString ("ScienceIndexAuthorProfile.Text") %>" border="0" '+
							'height="31" width="88" border="0"><\/a>')
							//--></script>
							<!--/Science Index counter-->
						</asp:Panel>
				    </asp:Panel>
				</div>
        		<div id="employeeExperience-<%= ModuleId %>" class="tab-pane fade" role="tabpanel">
        			<asp:Label id="labelExperienceYears" runat="server" CssClass="_label" />
        			<div class="table-responsive">
        				<asp:GridView id="gridExperience" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-striped table-bordered table-hover grid-experience"
                            UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
    						<Columns>
                                <asp:BoundField DataField="Years_String" HeaderText="Years.Column" />
                                <asp:BoundField DataField="Title_Link" HeaderText="Title.Column" HtmlEncode="false" />
                                <asp:BoundField DataField="AchievementType_String" HeaderText="AchievementType.Column" />
                                <asp:BoundField DataField="DocumentUrl_Link" HeaderText="DocumentUrl.Column" HtmlEncode="false" />
                            </Columns>
        			    </asp:GridView>
        			</div>
        		</div>
        		<div id="employeeAchievements-<%= ModuleId %>" class="tab-pane fade" role="tabpanel">
        			<div class="table-responsive">
        				<asp:GridView id="gridAchievements" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-striped table-bordered table-hover grid-achievements"
        			        UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
    						<Columns>
                                <asp:BoundField DataField="Years_String" HeaderText="Years.Column" />
                                <asp:BoundField DataField="Title_Link" HeaderText="Title.Column" HtmlEncode="false" />
                                <asp:BoundField DataField="AchievementType_String" HeaderText="AchievementType.Column" />
                                <asp:BoundField DataField="DocumentUrl_Link" HeaderText="DocumentUrl.Column" HtmlEncode="false" />
                            </Columns>
        			    </asp:GridView>
        			</div>
        		</div>
        		<div id="employeeDisciplines-<%= ModuleId %>" class="tab-pane fade" role="tabpanel">
                    <div class="table-responsive">
                        <asp:GridView id="gridDisciplines" runat="server" AutoGenerateColumns="false" CssClass="table table-sm table-striped table-bordered table-hover grid-disciplines"
                            UseAccessibleHeader="true" OnRowCreated="grid_RowCreated" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="EduProgramProfile_String" HeaderText="EduProgramProfile.Column" />
                                <asp:BoundField DataField="EduLevel_String" HeaderText="EduLevel.Column" />
                                <asp:BoundField DataField="Disciplines" HeaderText="Disciplines.Column" />
                            </Columns>
                        </asp:GridView>
                    </div>
        			<asp:Literal id="litDisciplines" runat="server" />
        		</div>
        		<div id="employeeAbout-<%= ModuleId %>" class="tab-pane fade u8y-employee-about" role="tabpanel">
        			<asp:Literal id="litAbout" runat="server" />
        		</div>
			</div>
    	</div>
    </div>
    <ul class="dnnActions dnnClear" style="margin-bottom:1em">
		<li>
            <asp:HyperLink id="linkEdit" runat="server" role="button" CssClass="btn btn-primary" Visible="false">
                <span class="glyphicon glyphicon glyphicon-pencil" aria-hidden="true"></span>
                <%: LocalizeString ("cmdEdit") %>
            </asp:HyperLink>
        </li>
		<li>
			<asp:HyperLink id="linkReturn" runat="server" role="button" CssClass="btn btn-secondary">
			    <span class="glyphicon glyphicon glyphicon-remove" aria-hidden="true"></span>
				<%: LocalizeString ("Close.Text") %>
			</asp:HyperLink>
		</li>
    </ul>
	<controls:AgplSignature id="agplSignature" runat="server" ShowRule="true" />
</asp:Panel>
<div id="employee-barcode-dialog-<%: ModuleId %>" class="modal fade" role="dialog" tabindex="-1" aria-labelledby="employee-barcode-dialog-title-<%: ModuleId %>">
    <div class="modal-dialog modal-sm" role="document">
	    <div class="modal-content">
	        <div class="modal-header">
				<h5 id="employee-barcode-dialog-title-<%: ModuleId %>" class="modal-title"><asp:Label id="labelBarcodeEmployeeName" runat="server" /></h5>
			    <button type="button" class="close" data-dismiss="modal" aria-label='<%: LocalizeString("Close") %>'><span aria-hidden="true">&times;</span></button>
			</div>
			<div class="modal-body">
				<p><asp:Label runat="server" resourcekey="BarcodeScan.Text" /></p>
				<asp:Image id="imageBarcode" runat="server" CssClass="img-thumbnail d-block mx-auto" />
			</div>
        </div>
	</div>
</div>
<div id="u8y_employee_wbdl_dlg_<%: ModuleId %>" class="modal fade" role="dialog" tabindex="-1" aria-labelledby="u8y_employee_wbdl_dlg_title_<%: ModuleId %>">
    <div class="modal-dialog" role="document">
	    <div class="modal-content">
	        <div class="modal-header">
				<h5 id="u8y_employee_wbdl_dlg_title_<%: ModuleId %>" class="modal-title"><%: LocalizeString("WorkbookDownloaderDialogTitle") %></h5>
			    <button type="button" class="close" data-dismiss="modal" aria-label='<%: LocalizeString("Close") %>'><span aria-hidden="true">&times;</span></button>
			</div>
			<div class="modal-body"
				 data-module-id="<%: ModuleId %>"
				 data-employee-id="<%: Employee.EmployeeID %>"
				 data-is-authenticated="<%: Request.IsAuthenticated.ToString().ToLowerInvariant() %>"
				 data-is-admin='<%: (UserInfo.IsSuperUser || UserInfo.IsInRole ("Administrators")).ToString().ToLowerInvariant() %>'
				 data-login-url='<%: DotNetNuke.Common.Globals.LoginURL ("", false) %>'
				 data-resources="<%: WorkbookDownloaderResources %>">
			</div>
        </div>
	</div>
</div>
<script>
(function($, window, document) {
	$(document).ready(function() {
		$("#u8y_employee_wbdl_dlg_<%: ModuleId %>").on("shown.bs.modal", function (e) {
			var root = $(e.target).find(".modal-body");
			var moduleId = root.data("module-id");
			var props = {
				moduleId: moduleId,
				employeeId: root.data("employee-id"),
				isAuthenticated: root.data("is-authenticated"),
				isAdmin: root.data("is-admin"),
				loginUrl: root.data("login-url"),
				resources: root.data("resources")
			};
			ReactDOM.render(
		  		React.createElement(WorkbookDownloader, props, null), root.get(0)
			);
		});
	});
} (jQuery, window, document));
</script>