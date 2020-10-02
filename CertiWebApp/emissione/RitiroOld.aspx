<%@ Page Language="C#" MasterPageFile="~/UnisysPortaleCdR.Master" Theme="unisysPortale"
    AutoEventWireup="true" Codebehind="Ritiro.aspx.cs" Inherits="Com.Unisys.CdR.Certi.WebApp.emissione.Ritiro"
    Title="Ritiro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h3 class="titologruppo">
        Recupero del certificato</h3>
    <div class="contentservizi">
        <div class="divCertiSpace">
        </div>
        <div class="testo" style="text-align: justify">
            Il certificato anagrafico emesso online riporta un <b>C</b>odice <b>I</b>dentificativo <b>U</b>nivoco
            (CIU), composto da valori numerici che ne garantisce l'unicità. Contiene inoltre un timbro digitale
            contenente i dati del certificato e la firma digitale del Sindaco di Roma per consentire una verifica di integrità
            attraverso il programma messo a disposizione gratuitamente da Roma Capitale.
          </div>
           <div class="divCertiSpace">
        </div>
        <div class="testo" style="text-align: center">
            <b>AVVISO IMPORTANTE </b>          
            <br />
        </div>
         <div class="divCertiSpace">
        </div>
         <div class="testo" style="text-align: justify">
         <b>La ricevuta dell'avvenuto pagamento, se dovuto, viene notificata all'indirizzo email dell'utente dal Servizio Pagamenti 
         di Roma Capitale. Se si è in possesso di tale ricevuta ma il certificato richiesto non può essere stampato in questa pagina,
         non effettuare una nuova richiesta ma contattare il Contact Center ChiamaRoma 060606, che provvederà ad inoltrare la richiesta
         all'ufficio competente per lo sblocco.<br />
         Ogni nuova richiesta infatti genera un nuovo certificato con relativi diritti e bollo, per cui non sarà rimborsabile l'importo
         dovuto e versato erroneamente più volte dall'utente.</b>
         </div>
        <div class="divCertiSpace">
        </div>
        <asp:Panel ID="pnlCertificati" runat="server">
            <div class="testo">
                <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                    <tr style="background-color: #ddd">
                        <td style="width: 65px">
                            <img src="../App_Themes/unisysPortale/images/uno.gif" alt="passo numero 1" height="40"
                                style="margin: 2px 0px 2px 2px" />
                        </td>
                        <td>
                            <span class="Titoletto">Ritiro del certificato</span>
                        </td>
                    </tr>
                    <tr style="background-color: #f0f0f0">
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblPresenza" runat="server" />
                        </td>
                    </tr>
                </table>
                <asp:Repeater ID="rptCertificati" runat="server" OnItemDataBound="rptCertificati_ItemDataBound">
                    <HeaderTemplate>
                        <table class="datiTableCerti" width="100%" cellspacing="0" cellpadding="4" border="0">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="datiCellCerti" valign="top" style="width: 140px">                                
                                <asp:Label ID="lblCertificato" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "T_TIPO_CERTIFICATO") %>' />
                                <br />
                                <br />
                                <b>
                                    <asp:Label ID="lblProdottoDa" runat="server" Text='<%# GetProduttoreCertificato(DataBinder.Eval(Container.DataItem, "XML_CERTIFICATO").ToString()) %>' />
                                </b>
                            </td>
                            <td class="datiCellCerti" valign="top">
                                <asp:Label ID="lblDettagli" runat="server" />
                            </td>
                            <td class="datiCellCerti" valign="top">
                                <asp:Button ID="linkProduci" runat="server" OnClick="linkProduci_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CIU")%>'
                                    Text="Stampa"></asp:Button><br />
                                <asp:Button ID="linkRecup" runat="server" OnClick="linkRecup_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CIU")%>'
                                    Text="Recupera"></asp:Button>
                                <asp:Button ID="aggiornaPagamento" class="extButton"  visible='<%# (DataBinder.Eval(Container.DataItem,"STATUS_ID").ToString() == "19") ? true : false %>' runat="server" OnClick="aggiornaPagamento_Click"  Text="Aggiorna Pagamento"></asp:Button><br />
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
            <div class="divCertiWarning">
                I documenti sono in formato standard <b>PDF:</b> per la visualizzazione è possibile
                utilizzare il programma gratuito <b>Adobe Reader</b> o uno equivalente a scelta
                dell’utente
            </div>
            <div class="divCertiWarning">
                Il timbro digitale riportato sul documento può essere verificato con il programma gratuito disponibile <a href="https://www.comune.roma.it/web/it/scheda-servizi.page?contentId=INF422279"> "qui"</a>                
            </div>
            <div class="divCertiWarning">
                <b>Attenzione:</b> per motivi di sicurezza il tuo indirizzo di connessione (IP)
                verrà registrato e mantenuto a norma di legge.
            </div>
        </asp:Panel>
    </div>
</asp:Content>
