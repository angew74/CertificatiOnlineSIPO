
<%@ Page Language="C#" MasterPageFile="~/UnisysPortaleCdR.Master" Theme="unisysPortale"
    AutoEventWireup="true" Codebehind="Consegna.aspx.cs" Inherits="Com.Unisys.CdR.Certi.WebApp.recupero.Consegna"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h3 class="titologruppo">
        Consegna certificato</h3>
    <div class="contentservizi">
    <asp:HiddenField ID="fase" runat="server" Value="0" />
    <asp:HiddenField ID="tipoCertificato" runat="server" />
    <asp:HiddenField ID="intestatario" runat="server" /> 
    <asp:HiddenField ID="dataEmissione" runat="server" />
    <asp:HiddenField ID="soggetto" runat="server" />
    <asp:HiddenField ID="codicePagamento" runat="server" />   
    <div class="divCertiSpace">
    </div>
 <%--   <div class="testo" style="text-align:justify">
        Il certificato anagrafico riporta nella parte bassa un codice identificativo univoco (CIU), composto da 15 simboli alfanumerici, 
        attraverso il quale è possibile recuperare in un secondo momento il certificato 
        accedendo all’area pubblica del Portale denominata <a href="https://www.comune.roma.it/wass/wps/myportal/%21ut/p/_s.7_0_A/7_0_21L?menuPage=/Area_di_navigazione/Servizi_on_line/Elenco_servizi_online/Servizi_Anagrafici/Recupero_Certificati_Anagrafici_Online/">Recupero Certificati Anagrafici Online</a>; 
        contiene inoltre un timbro digitale contenente i dati del certificato e la firma digitale 
        del Sindaco di Roma per consentire una verifica di integrità attraverso il programma 
        messo a disposizione gratuitamente dal Comune di Roma.<br /><br />
        Nota bene: Il certificato emesso è utilizzabile una sola volta. 
        La riproduzione plurima dello stesso certificato deve considerarsi illecita  in quanto, 
        ogni singolo certificato, comporta l’obbligo del versamento dei diritti di segreteria e dell’imposta di bollo se dovuta. I contravventori saranno perseguiti nelle forme e nei termini previsti dalla legge. <br />
    </div>--%>
    <div class="divCertiSpace">
    </div>
    <asp:Panel ID="pnlCaptcha"  runat="server" Visible="false">
        <div class="testo">
                <table style="width:100%" cellpadding="0px" cellspacing="0px" border="0px">
                <tr style="background-color:#ddd">
                <td style="width:65px">
                <img src="../App_Themes/unisysPortale/images/uno.gif" alt="passo numero 1" height="40" style="margin:2px 0px 2px 2px" />
                </td>
                <td>
                <span class="Titoletto">Inserisci il codice del Certificato</span>
                </td>
                </tr>
                <tr style="background-color:#f0f0f0">
                <td>
                </td>
                <td>
                </td>
                </tr>
                </table>
            </div>
        <table class="datiTableModuli" cellspacing="0" cellpadding="4" border="0">
            <tr>
                <td class="datiCellModuliFirstColumn" valign="top" style="height: 50px"></td>
                <td class="datiCellModuliSecondColumn" style="height: 50px">
                    <img class="captchaImage" title="captcha" height="0" alt="codice anti automazione"
                        src="captcha.do" style="width: 150px; border:solid 1px #ddd " /></td>
                <td class="datiCellModuli" valign="top" style="height: 50px">
                    <div style="text-align: left">
                        <br />
                        <asp:HyperLink ID="HyperLink2" runat="server" EnableViewState="False" NavigateUrl="consegna.aspx"
                            ForeColor="Black" Font-Bold="True" Font-Underline="True">Clicca qui</asp:HyperLink>&nbsp;se
                        non riesci a leggere il codice alfanumerico presente nell'immagine a sinistra.<br />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="datiCellModuliFirstColumn" valign="top">
                   <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" ControlToValidate="CodeNumberTextBox"
                        ErrorMessage="<b>Errore:</b> Codice di controllo assente" EnableViewState="False" EnableClientScript="False">*</asp:RequiredFieldValidator><asp:CustomValidator
                            ID="CustomValidator1" runat="server" ErrorMessage="<b>Errore:</b> Codice di controllo errato"
                            ControlToValidate="CodeNumberTextBox" EnableViewState="False" EnableClientScript="False">*</asp:CustomValidator></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                    <div style="text-align: center">
                        <asp:TextBox ID="CodeNumberTextBox" runat="server" Width="150px"></asp:TextBox></div>
                </td>
                <td class="datiCellModuli" valign="top" align="left">
                    Inserire il codice alfanumerico presente nell' immagine
                </td>
            </tr>
            <tr>
                <td class="datiCellModuliFirstColumn" valign="top">
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" ControlToValidate="CiuTextbox"
                        ErrorMessage="<b>Errore:</b> Codice CIU assente" EnableViewState="False" EnableClientScript="False">*</asp:RequiredFieldValidator><asp:CustomValidator
                            ID="CustomValidator2" runat="server" ErrorMessage="<b>Errore:</b> Codice CIU errato"
                            EnableViewState="False" EnableClientScript="False">*</asp:CustomValidator></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                    <div style="text-align: center">
                        <asp:TextBox ID="CiuTextbox" runat="server" Width="150px"></asp:TextBox></div>
                </td>
                <td class="datiCellModuli" valign="top" align="left">
                    Inserire il Codice Identificativo Univoco del documento
                </td>
            </tr>
        </table>
        <div class="divCertiSpace">
        </div>
        <div style="text-align: center">
       <asp:Button ID="SubmitButton" runat="server" Text="Cerca" CssClass="frmBTN" OnClick="SubmitButton_Click" CausesValidation="False">
                            </asp:Button>
        </div>
    </asp:Panel> 
    <asp:Panel ID="pnlCaptchaRo" runat="server" Visible="false">
    <div class="testo">
                <table style="width:100%" cellpadding="0px" cellspacing="0px" border="0px">
                    <tr>
                         <td style="width:65px">
                            <img src="../App_Themes/unisysPortale/images/uno.gif" alt="passo numero 1" height="40" style="margin:2px 0px 2px 2px" />
                        </td>
                        <td style="width:120pt;">
                            <span class="Titoletto">Codice Certificato</span>
                        </td>
                        <td>
                            <b><asp:Label ID="lblCIU" runat="server" CssClass="txtCerti" /></b>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    <!-- Pannello consegna certificato-->
    <asp:Panel ID="pnlCertificato" runat="server">
         <div class="testo">
                <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                <tr style="background-color:#ddd">
                <td style="width:65px">
                <img src="../App_Themes/unisysPortale/images/due.gif" alt="passo numero 2" height="40" style="margin:2px 0px 2px 2px" />
                </td>
                <td>
                <span class="Titoletto">Dettagli Certificato</span>
                </td>
                </tr>
                <tr style="background-color:#f0f0f0">
                <td>
                </td>
                <td>
                </td>
                </tr>
                </table>
            </div>
         <table class="datiTableModuli" cellspacing="0" cellpadding="4" border="0">
            <tr>
                <td class="datiCellModuliFirstColumn"></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                   <b>Certificato di:</b> <asp:Label ID="lblTipoCertificato" runat="server"></asp:Label>
                </td>
                <td class="datiCellModuli" valign="top">
                </td>
            </tr>
            <tr>
            <td class="datiCellModuliFirstColumn"></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                    <b>Intestato a:</b> <asp:Label ID="lblIntestatario" runat="server"></asp:Label>
                </td>
                <td class="datiCellModuli" valign="top" >
                </td>
            </tr>
            <tr>
            <td class="datiCellModuliFirstColumn"></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                    <b>Emesso il giorno:</b> <asp:Label ID="lblData" runat="server"></asp:Label>
                </td>
                <td class="datiCellModuli" valign="top" >
                </td>
            </tr>
            <tr>
            <td class="datiCellModuliFirstColumn"></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                    <asp:Label ID="lblCodicePagamento" runat="server"></asp:Label>
                </td>
                <td class="datiCellModuli" valign="top" align="right">
                    <asp:Button ID="linkCerti"  runat="server"  Text="Download" CssClass="extButton" Enabled="false" OnClick="linkCerti_Click" CausesValidation="False"></asp:Button>
                     <%--<asp:Button ID="linkCerti"  runat="server" CssClass="extButton" Enabled="false" OnClick="linkCerti_Click" CausesValidation="False">Download</asp:Button>--%>
                </td>
            </tr>
            <tr>
            <td class="datiCellModuliFirstColumn"></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                    <asp:Label ID="lblSoggetto" runat="server"></asp:Label>
                </td>
                <td class="datiCellModuli" valign="top">
                    </td>
            </tr>
        </table>
        </asp:Panel>
        <div class="divCertiSpace">
    </div>
       <asp:Panel ID="pnlRitiri" runat="server">
         <div class="testo">
                <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                <tr style="background-color:#ddd">
                <td style="width:65px">
                <img src="../App_Themes/unisysPortale/images/tre.gif" alt="passo numero 3" height="40" style="margin:2px 0px 2px 2px" />
                </td>
                <td>
                <span class="Titoletto">Notifica di Consegna</span>
                </td>
                </tr>
                <tr style="background-color:#f0f0f0">
                <td>
                </td>
                <td>
                </td>
                </tr>
                </table>
            </div>
        <table class="datiTableModuli" cellspacing="0" cellpadding="4" border="0">
            <tr>
            <td class="datiCellModuliFirstColumn"></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                    Inserire il <b>Nome</b> o la <b>Ragione Sociale</b> del soggetto a cui il cittadino ha consegnato il certificato 
                </td>
                
            </tr>
            <tr>
               <td class="datiCellModuliFirstColumn"></td>
                <td class="datiCellModuliSecondColumn" valign="top">
                    <asp:TextBox ID="txtSoggetto" MaxLength="255" Width="99%"  runat="server"></asp:TextBox> 
                </td>
            </tr>
        </table>
        <div style="text-align: center">
        <asp:Button ID="btnRitiro" runat="server" Text="Avanti" CssClass="frmBTN" OnClick="btnRitiro_Click" CausesValidation="False" />
         </div>
        </asp:Panel>
         
       <div class="divCertiSpace"></div>
       <div class="divCertiWarning">
            I documenti sono in formato standard <b>PDF:</b> per la visualizzazione è possibile utilizzare 
            il programma gratuito <b>Adobe Reader</b> o uno equivalente a scelta dell’utente.
            </div>
       <div class="divCertiWarning">
            Il timbro digitale riportato sul documento può essere verificato con il programma gratuito disponibile 
            <a href="http://www.comune.roma.it/AreaDownload/"><b>qui</b></a>
            </div>
       <div class="divCertiWarning">
            <b>Attenzione:</b> per motivi di sicurezza il tuo indirizzo di connessione (IP) verrà registrato e mantenuto a norma di legge.
        </div>
     </div>
</asp:Content>
