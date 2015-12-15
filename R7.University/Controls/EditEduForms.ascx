﻿<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditEduForms.ascx.cs" Inherits="R7.University.Controls.EditEduForms" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/urlcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div class="dnnForm dnnClear university-edit-eduforms">
    <fieldset>
        <div class="dnnFormItem">
            <asp:GridView id="gridEduForms" runat="server" AutoGenerateColumns="false" CssClass="dnnGrid"
                GridLines="None" Style="margin-bottom:30px;width:775px">
                <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Left" />
                <RowStyle CssClass="dnnGridItem" HorizontalAlign="Left" />
                <AlternatingRowStyle CssClass="dnnGridAltItem" />
                <SelectedRowStyle CssClass="dnnFormError" />
                <EditRowStyle CssClass="dnnFormInput" />
                <FooterStyle CssClass="dnnGridFooter" />
                <PagerStyle CssClass="dnnGridPager" />
                <Columns>
                    <asp:TemplateField>
                       <ItemTemplate>
                            <span style="white-space:nowrap">
                                <asp:LinkButton id="linkEdit" runat="server" OnCommand="OnEditItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# EditIconUrl %>" />
                                </asp:LinkButton>
                                <asp:LinkButton id="linkDelete" runat="server" OnCommand="OnDeleteItemCommand" >
                                    <asp:Image runat="server" ImageUrl="<%# DeleteIconUrl %>" />
                                </asp:LinkButton>
                            </span>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ViewItemID" />
                    <asp:BoundField DataField="EduFormTitleLocalized" HeaderText="EduFormTitle" />
                    <asp:BoundField DataField="TimeToLearnString" HeaderText="TimeToLearn" />
                    <asp:BoundField DataField="IsAdmissive" HeaderText="IsAdmissive" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelEduForm" runat="server" ControlName="comboEduForm" />
            <asp:DropDownList id="comboEduForm" runat="server"
                DataTextField="TitleLocalized"
                DataValueField="EduFormID"
            /> 
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelTimeToLearn" runat="server" ControlName="textTimeToLearnYears" />
            <asp:TextBox id="textTimeToLearnYears" runat="server" Value="0" />
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel"></div>
            <asp:TextBox id="textTimeToLearnMonths" runat="server" Value="0" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="labelIsAdmissive" runat="server" ControlName="checkIsAdmissive" />
            <asp:CheckBox id="checkIsAdmissive" runat="server" />
        </div>
        <div class="dnnFormItem">
            <div class="dnnLabel"></div>
            <asp:LinkButton id="buttonAddEduForm" runat="server" resourcekey="buttonAddEduForm" 
                CssClass="dnnPrimaryAction" CommandArgument="Add" />
            <asp:LinkButton id="buttonUpdateEduForm" runat="server" resourcekey="buttonUpdateEduForm" 
                CssClass="dnnPrimaryAction" Visible="false" CommandArgument="Update" />
            <asp:LinkButton id="buttonCancelEditEduForm" runat="server" resourcekey="buttonCancelEditEduForm" 
                        CssClass="dnnSecondaryAction" />
        </div>
        <asp:HiddenField id="hiddenEduFormItemID" runat="server" />
    </fieldset>
</div>
