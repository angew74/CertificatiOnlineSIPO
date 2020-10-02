<%@ Page Language="C#" MasterPageFile="~/UnisysPortaleCdR.Master" Theme="unisysPortale" ValidateRequest="false"
    AutoEventWireup="true" Codebehind="Emissione.aspx.cs" Inherits="Com.Unisys.CdR.Certi.WebApp.emissione.Emissione"
    Title="Emissione" %>
    <%@ Register Src="~/controls/UCRicerca.ascx" TagName="UCRicerca" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
     <h3 class="titologruppo">
        Richiesta del certificato</h3>
    <div class="contentservizi">
        <asp:HiddenField ID="fase" runat="server" Value="0" />
        <asp:HiddenField ID="cfIntestatario" runat="server" Value="" />
         <asp:HiddenField ID="nomeIntestatario" runat="server" Value="" />
         <asp:HiddenField ID="cognomeIntestatario" runat="server" Value="" />
        <asp:HiddenField ID="idCertificato" runat="server" Value="" />
        <asp:HiddenField ID="idPublicCertificato" runat="server" Value="" />
        <asp:HiddenField ID="idTipoUso" runat="server" Value ="" />  
        <asp:HiddenField ID="idRichiesta" runat="server" Value="" />
        <asp:HiddenField ID="idEsenzione" runat="server" Value="" />
        
        <div class="divCertiSpace">
        </div>     
        <div class="divCertiSpace">
        </div>
        
        <!-- panel componenti della famiglia -->
        <asp:Panel ID="pnlIntestatario" runat="server" Visible="false">
             
            <div class="testo" >
            
                <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                <tr style="background-color:#ddd">
                <td style="width:65px">
                <img src="../App_Themes/unisysPortale/images/uno.gif" alt="passo numero 1" height="40" style="margin:2px 0px 2px 2px" />
                </td>
                <td>
                <span class="Titoletto">Scelta dell'intestatario<br />
                    <span style="font-size:10pt ">Selezionare la persona per la quale si sta richiedendo la certificazione</span> 
                </span> 
                </td>
                </tr>
                 <tr style="background-color:#f0f0f0">
                <td>
                </td>
                <td>
                <asp:RadioButtonList ID="rblIntestatario" runat="server" DataTextField="NomeIndiv" 
                    DataValueField="CodiceFiscale" />
                    <asp:Panel ID="pnlRicerca" runat="server" Visible="false">
         <uc1:UCRicerca ID="UCRicerca1" runat="server"  FlagRic="ZEROSEI" BottoneVisualizza="true" OnSelectIndividuo="OnSelectIndividuo" />
        </asp:Panel>
                </td>
                </tr>
                </table>
            </div>
            <div class="divCertiSpace">
            </div>
            <div style="text-align: center">
                <asp:Button ID="btnIntestatario" runat="server" Text="Avanti" CssClass="frmBTN" OnClick="btnIntestatario_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlIntestatarioRo" runat="server" Visible="false">
            <div class="testo">
                <table style="width:100%" cellpadding="0px" cellspacing="0px" border="0px">
                    <tr>
                         <td style="width:65px">
                            <img src="../App_Themes/unisysPortale/images/uno.gif" alt="passo numero 1" height="40" style="margin:2px 0px 2px 2px" />
                        </td>
                        <td style="width:79pt;">
                            <span class="Titoletto">Intestatario</span>
                        </td>
                        <td>
                            <b><asp:Label ID="lblNomeIntestatario" runat="server" CssClass="txtCerti" /><asp:Label ID="lblCognomeIntestatario" runat="server" CssClass="txtCerti" /><asp:Label ID="lblCodiceFiscaleIntestatario" runat="server" CssClass="txtCerti" /></b>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <!-- panel certificati --> 
        <asp:Panel ID="pnlCertificati" runat="server">
        <div class="testo">
                <asp:Repeater ID="rptCertificati" runat="server" OnItemDataBound="rptCertificati_ItemDataBound">
                    <HeaderTemplate>
                        <table style="width:100%" cellpadding="0px" cellspacing="0px" border="0px">
                <tr style="background-color:#ddd">
                <td style="width:65px">
                <img src="../App_Themes/unisysPortale/images/due.gif" alt="passo numero 1" height="40" style="margin:2px 0px 2px 2px" />
                </td>
                <td >
                <span class="Titoletto">Scelta del Certificato</span>
                </td>
                <td style="width:60px">
                semplice
                </td>
                <td style="width:50px">
                in bollo
                </td>
                </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr style="background-color:#f0f0f0">
                            <td style="text-align:right" valign="top">
                            <asp:RadioButton ID="rbCert" GroupName='<%# DataBinder.Eval(Container.DataItem, "PUBLIC_ID") %>'
                                    runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PUBLIC_ID") %>' CssClass="hiddenText"
                                    AutoPostBack="true" OnCheckedChanged="rbCert_CheckedChanged" />
                            </td>
                            <td valign="top">
                                <asp:Label ID="certNome" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Nome") %>' />
                            </td>
                            <td  valign="top">
                                <asp:CheckBox ID="chbSemp" runat="server" AutoPostBack="true" OnCheckedChanged="chbSemp_CheckedChanged"/>
                            </td>
                            <td  valign="top">
                                <asp:CheckBox ID="chbBoll" runat="server" AutoPostBack="true" OnCheckedChanged="chbBoll_CheckedChanged"  />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="divCertiSpace">
            </div>
            <div class="div1Of2Buttons">
                <asp:Button ID="btnCertiOff" runat="server" Text="Indietro" CssClass="frmBTN" OnClick="btnCertiOff_Click" />
            </div>
            <div class="divSp2Buttons">
            </div>
            <div class="div2Of2Buttons">
                <asp:Button ID="btnCertiOn" runat="server" Text="Avanti" CssClass="frmBTN" OnClick="btnCertiOn_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlCertificatiRo" runat="server">
        <div class="testo">
                 <table style="width:100%" cellpadding="0px" cellspacing="0px" border="0px">
                    <tr>
                         <td style="width:65px">
                            <img src="../App_Themes/unisysPortale/images/due.gif" alt="passo numero 2" height="40" style="margin:2px 0px 2px 2px" />
                        </td>
                        <td style="width:98pt">
                            <span class="Titoletto">Certificato di</span>
                        </td>
                        <td>
                            <b><asp:Label ID="lblCertificato" runat="server" CssClass="txtCerti" /></b>
                        </td>
                    </tr>
                </table>
        </div>
</asp:Panel>
        <!-- panel motivazioni -->
        <asp:Panel ID="pnlMotivazioni" runat="server" Visible="false">
            <div class="testo">
                <table style="width:100%" cellpadding="0px" cellspacing="0px" border="0px">
                <tr style="background-color:#ddd">
                <td style="width:65px">
                <img src="../App_Themes/unisysPortale/images/tre.gif" alt="passo numero 1" height="40" style="margin:2px 0px 2px 2px" />
                </td>
                <td>
                <span class="Titoletto">Motivo per esenzione dal bollo</span>
                </td>
                </tr>
                <tr style="background-color:#f0f0f0">
                <td>
                </td>
                <td>
                    <div class="testoSmall">
                        <%--<asp:CheckBoxList ID="chlMotivazioni" CssClass="testoSmall" runat="server" DataTextField="Descrizione"
                    DataValueField="ID" />--%>
                    <asp:RadioButtonList ID="rblMotivazioni" CssClass="testoSmall" runat="server" DataTextField="Descrizione"
                    DataValueField="ID" />
                    </div>
                </td>
                </tr>
                </table>
            </div>
            <div class="divCertiSpace">
            </div>
            <div class="div1Of2Buttons">
                <asp:Button ID="btnMotivoOff" runat="server" Text="Indietro" CssClass="frmBTN" OnClick="btnMotivoOff_Click" />
            </div>
            <div class="divSp2Buttons">
            </div>
            <div class="div2Of2Buttons">
                <asp:Button ID="btnMotivoOn" runat="server" Text="Avanti" CssClass="frmBTN" OnClick="btnMotivoOn_Click" />
            </div>
            <div class="divCertiSpace">
            </div>
        </asp:Panel>
        <asp:Panel ID="PnlMotivazioniro" runat="server" Visible="false">
        <div class="testo">
                 <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                    <tr style="vertical-align:top">
                        <td style="width:65px;">
                            <img src="../App_Themes/unisysPortale/images/tre.gif" alt="passo numero 1" height="40" style="margin:2px 0px 2px 2px" />
                        </td>
                        <td style="width:98pt;padding-top:15px">
                            <span class="Titoletto">Esenzione per </span>
                        </td>
                        <%--<td style="padding-top:15px">
                        <b><asp:BulletedList ID="listMotivazioni" CssClass="txtCerti" runat="server" Visible="true" /></b>
                        </td>--%>
                        <td style="padding-top:15px">
                        <b><asp:Label ID="lblMotivazioni" runat="server" CssClass="txtCerti" Visible="true" /></b>
                        </td>
                    </tr>
                </table>
                
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlConferma" runat="server" Visible="false">
        <div class="divCertiSpace">
        </div>
        <div class="div1Of2Buttons">
            <asp:Button ID="btnOff" runat="server" Text=" Indietro " CssClass="frmBTN" OnClick="btnOff_Click"
               />
        </div>
        <div class="divSp2Buttons">
        </div>
        <div class="div2Of2Buttons">
            <asp:Button ID="btnOn" runat="server" Text=" Effettua pagamento " CssClass="frmBTN" OnClick="btnOn_Click"
                />
        </div>
        </asp:Panel>
       
    </div>
</asp:Content>
