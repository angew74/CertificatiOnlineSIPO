<%@ Control Language="C#" AutoEventWireup="true" Codebehind="UCPaging.ascx.cs" Inherits="Com.Unisys.CdR.Certi.WebApp.controls.UCPaging" %>
<div style="z-index: 0" class="tabella">
    <asp:Label ID="labPager" ForeColor="#15428B" Text="Pagina:" runat="server" />
    <asp:DropDownList ID="ddlPagerPages" CausesValidation="false" AutoPostBack="true" runat="server" OnSelectedIndexChanged="OnPagerIndexChanged" />
    <asp:Label ID="labPagerPages" ForeColor="#15428B" runat="server" />
    <asp:HiddenField ID="hfPagingValue" Value="0" runat="server" />
</div>
