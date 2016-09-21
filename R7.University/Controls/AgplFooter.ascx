﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="R7.University.Controls.AgplFooter" CodeBehind="AgplFooter.ascx.cs" %>
<div class="u8y-agpl-footer">
    <% if (ShowRule) { %><hr /><% } %>
    <a href="https://github.com/roman-yagodin/R7.University" rel="nofollow" target="_blank" title="<%= LocalizeString ("AgplLabel.Title") %>">
	    <%= string.Format ("{0} v{1}", AppName, AppVersion.ToString (3)) %>
	</a>
</div>