﻿<%@ Control Language="C#" AutoEventWireup="false" Inherits="R7.University.Controls.AgplSignature" CodeBehind="AgplSignature.ascx.cs" %>
<div class="u8y-agpl-footer">
    <% if (ShowRule) { %><hr /><% } %>
    <a href="https://github.com/roman-yagodin/R7.University" rel="nofollow" target="_blank" title="<%= LocalizeString ("SourceLink.Title") %>">
		<%: Name %> v<%: InformationalVersion ?? Version.ToString (3) %>
	</a>
</div>