<%@ Page Title="Emissione" Language="C#" MasterPageFile="~/Certificati.Master" AutoEventWireup="true"
    ValidateRequest="false"
    CodeBehind="Emissione.aspx.cs" Inherits="Com.Unisys.CdR.Certi.WebApp.emissione.Emissione" %>

<%@ Register Src="~/controls/UCRicerca.ascx" TagName="UCRicerca" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="contentgruppo">
        <h3 class="u-text-h3">Richiesta del certificato</h3>
    </div>
    <div class="contentgruppo" style="background: transparent;">
        <asp:HiddenField ID="fase" runat="server" Value="0" />
        <asp:HiddenField ID="cfIntestatario" runat="server" Value="" />
        <asp:HiddenField ID="nomeIntestatario" runat="server" Value="" />
        <asp:HiddenField ID="cognomeIntestatario" runat="server" Value="" />
        <asp:HiddenField ID="idCertificato" runat="server" Value="" />
        <asp:HiddenField ID="idPublicCertificato" runat="server" Value="" />
        <asp:HiddenField ID="idTipoUso" runat="server" Value="" />
        <asp:HiddenField ID="idRichiesta" runat="server" Value="" />
        <asp:HiddenField ID="idEsenzione" runat="server" Value="" />
        <!-- panel componenti della famiglia -->
        <ol class="Prose Bullets">
            <li class="u-margin-bottom-s pagination" id="liIntestatario" runat="server">
                <asp:Panel ID="pnlIntestatario" CssClass="Prose u-layout-prose" runat="server" Visible="false">
                    <h4>Scelta dell'intestatario</h4>
                    <p>
                        Selezionare la persona per la quale si sta richiedendo la certificazione
                    </p>
                    <div class="Grid Grid--fit Grid--withGutter u-padding-all-l">
                        <div class="Grid-cell">
                            <asp:RadioButtonList ID="rblIntestatario" CssClass="Table js-TableResponsive tablesaw tablesaw-stack" data-tablesaw-mode="stack" runat="server" DataTextField="NomeIndiv"
                                DataValueField="CodiceFiscale" />                            
                        </div>
                    </div>
                    <div class="Form-field Grid-cell u-textCenter">
                        <asp:Button ID="btnIntestatario" runat="server" Text="Avanti" class="Button Button--default" OnClick="btnIntestatario_Click" />
                    </div>
                    <asp:Panel ID="pnlRicerca" runat="server" Visible="false" Style="margin-left:-10%">
                                <uc1:UCRicerca ID="UCRicerca1" runat="server" flagric="ZEROSEI" bottonevisualizza="true" OnSelectIndividuo="OnSelectIndividuo" />
                            </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="pnlIntestatarioRo" CssClass="Grid Grid--withGutter u-padding-all-l " runat="server" Visible="false">
                    <div class="Grid-cell">
                        <b>
                            <asp:Label ID="lblNomeIntestatario" runat="server" /><asp:Label ID="lblCognomeIntestatario" runat="server" />
                            <asp:Label ID="lblCodiceFiscaleIntestatario" runat="server"  /></b>
                    </div>
                </asp:Panel>
            </li>
            <!-- panel certificati -->
            <li class="u-margin-bottom-s" id="liCertificati" runat="server">
                <asp:Panel ID="pnlCertificati" runat="server" CssClass="fullwidth">
                    <asp:Repeater ID="rptCertificati" runat="server" OnItemDataBound="rptCertificati_ItemDataBound">
                        <HeaderTemplate>
                            <table class="Table Table--withBorder Table--compact js-TableResponsive tablesaw tablesaw-swipe" data-tablesaw-mode="swipe">
                                <thead>
                                    <tr>
                                        <th scope="col" style="min-width: 69%" colspan="2">Scelta del Certificato</th>
                                        <th scope="col" style="max-width: 15%;">semplice</th>
                                        <th scope="col" style="max-width: 15%;">in bollo</th>
                                    </tr>
                                </thead>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tbody>
                                <tr>
                                    <td style="text-align: right; max-width: 1%;" valign="top">
                                        <asp:RadioButton ID="rbCert" GroupName='<%# DataBinder.Eval(Container.DataItem, "PUBLIC_ID") %>'
                                            runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PUBLIC_ID") %>' CssClass="hiddenText"
                                            AutoPostBack="true" OnCheckedChanged="rbCert_CheckedChanged" />
                                    </td>
                                    <td valign="top" style="min-width: 69%">
                                        <asp:Label ID="certNome" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Nome") %>' />
                                    </td>
                                    <td valign="top" style="max-width: 15%;">
                                        <asp:CheckBox ID="chbSemp" runat="server" AutoPostBack="true" OnCheckedChanged="chbSemp_CheckedChanged" />
                                    </td>
                                    <td valign="top" style="max-width: 15%;">
                                        <asp:CheckBox ID="chbBoll" runat="server" AutoPostBack="true" OnCheckedChanged="chbBoll_CheckedChanged" />
                                    </td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <div class="Form-field Grid-cell" style="margin-top: 10px">
                        <asp:Button ID="btnCertiOff" runat="server" Text="Indietro" class="Button Button--default" OnClick="btnCertiOff_Click" />
                        <asp:Button ID="btnCertiOn" runat="server" Text="Avanti" class="Button Button--default" OnClick="btnCertiOn_Click" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlCertificatiRo"
                    CssClass="Grid Grid--withGutter u-padding-all-l " runat="server">
                    <div class="Grid-cell">
                        <b>
                            <asp:Label ID="lblCertificato" runat="server" /></b>
                    </div>
                </asp:Panel>
            </li>
            <!-- panel motivazioni -->
            <li class="u-margin-bottom-s" id="liMotivazioni" runat="server">
                <asp:Panel ID="pnlMotivazioni" runat="server" Visible="false">
                    <table class="Table Table--withBorder Table--compact js-TableResponsive tablesaw tablesaw-swipe" data-tablesaw-mode="swipe">
                        <thead>
                            <tr>
                                <th scope="col" colspan="2">Motivo per esenzione dal bollo</th>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:RadioButtonList ID="rblMotivazioni" CssClass="testoSmall" runat="server" DataTextField="Descrizione"
                                        DataValueField="ID" />

                                </td>
                            </tr>
                    </table>
                    <div class="Form-field Grid-cell" style="margin-top: 10px">
                        <asp:Button ID="btnMotivoOff" runat="server" Text="Indietro" class="Button Button--default" OnClick="btnMotivoOff_Click" />
                        <asp:Button ID="btnMotivoOn" runat="server" Text="Avanti" class="Button Button--default" OnClick="btnMotivoOn_Click" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="PnlMotivazioniro" CssClass="Grid Grid--withGutter u-padding-all-l " runat="server" Visible="false">
                    <div class="Grid-cell">
                        <b>
                            <asp:Label ID="lblMotivazioni" runat="server" Visible="true" /></b>
                    </div>
                </asp:Panel>
            </li>
        </ol>
        <asp:Panel ID="pnlConferma" runat="server" Visible="false">
            <div class="Form-field Grid-cell" style="margin-top: 10px">
                <asp:Button ID="btnOff" runat="server" Text=" Indietro " class="Button Button--default" OnClick="btnOff_Click" />
                <asp:Button ID="btnOn" runat="server" Text=" Effettua pagamento " class="Button Button--default" OnClick="btnOn_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
