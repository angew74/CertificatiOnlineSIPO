<%@ Page Title="Recupero" Language="C#" MasterPageFile="~/Certificati.Master" AutoEventWireup="true" CodeBehind="Ritiro.aspx.cs" Inherits="Com.Unisys.CdR.Certi.WebApp.emissione.Ritiro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="contentgruppo">
        <h3 class="u-text-h3">Recupero del certificato</h3>
    </div>
    <div class="contentgruppo">
        <article class="Prose u-layout-prose-ritiro fullwidth">
            <p class="u-layout-prose-ritiro u-color-grey-90 u-text-p u-padding-r-all u">
                Il certificato anagrafico emesso online riporta un <b>C</b>odice <b>I</b>dentificativo <b>U</b>nivoco
            (CIU), composto da valori numerici che ne garantisce l'unicità. Contiene inoltre un timbro digitale
            contenente i dati del certificato e la firma digitale del Sindaco di Roma per consentire una verifica di integrità
            attraverso il programma messo a disposizione gratuitamente da Roma Capitale.
            </p>
        </article>
        <h4 class="u-text-h4">
            <b>AVVISO IMPORTANTE </b>
        </h4>
        <article class="Prose u-layout-prose-ritiro">
            <p class="u-layout-prose-ritiro u-color-grey-90 u-text-p u-padding-r-all">
                <b>La ricevuta dell'avvenuto pagamento, se dovuto, viene notificata all'indirizzo email dell'utente dal Servizio Pagamenti 
         di Roma Capitale. Se si è in possesso di tale ricevuta ma il certificato richiesto non può essere stampato in questa pagina,
         non effettuare una nuova richiesta ma contattare il Contact Center ChiamaRoma 060606, che provvederà ad inoltrare la richiesta
         all'ufficio competente per lo sblocco.<br />
                    Ogni nuova richiesta infatti genera un nuovo certificato con relativi diritti e bollo, per cui non sarà rimborsabile l'importo
         dovuto e versato erroneamente più volte dall'utente.</b>
            </p>
        </article>
    </div>

    <asp:Panel runat="server" ID="PanelPresenza" class="Prose Alert Alert--info Alert--withIcon u-layout-prose u-padding-r-bottom u-padding-r-right u-margin-r-bottom" 
        style="margin-left:30%" role="alert">         
        <h2 class="u-text-h3">Ritiro del certificato
        </h2>
        <p class="u-text-p">Non ci sono al momento certificati da ritirare</p>
    </asp:Panel>


    <asp:Panel ID="pnlCertificati" CssClass="contentgruppo" runat="server">
        <h4 class="u-text-h4">Ritiro del certificato</h4>
        <asp:Repeater ID="rptCertificati" runat="server" OnItemDataBound="rptCertificati_ItemDataBound">
            <HeaderTemplate>
                <table class="Table Table--withBorder js-TableResponsive tablesaw tablesaw-swipe" style="font-size: 2rem" data-tablesaw-mode="swipe">
            </HeaderTemplate>
            <ItemTemplate>
                <tbody>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCertificato" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "T_TIPO_CERTIFICATO") %>' />
                            <br />
                            <br />
                            <b>
                                <asp:Label ID="lblProdottoDa" runat="server" Text='<%# GetProduttoreCertificato(DataBinder.Eval(Container.DataItem, "XML_CERTIFICATO").ToString()) %>' />
                            </b>
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblDettagli" runat="server" />
                        </td>
                        <td valign="top">
                            <asp:Button ID="linkProduci" CssClass="linkritiro" runat="server" OnClick="linkProduci_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CIU")%>'
                                Text="Stampa"></asp:Button><br />
                            <asp:Button ID="linkRecup" CssClass="linkritiro" runat="server" OnClick="linkRecup_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CIU")%>'
                                Text="Recupera"></asp:Button>
                            <asp:Button ID="aggiornaPagamento" CssClass="linkritiro" Visible='<%# (DataBinder.Eval(Container.DataItem,"STATUS_ID").ToString() == "19") ? true : false %>' runat="server" OnClick="aggiornaPagamento_Click" Text="Aggiorna Pagamento"></asp:Button><br />
                        </td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <div class="contentgruppo">
            <article class="Prose u-layout-prose-ritiro">
                <p class="u-layout-prose-ritiro u-color-grey-90 u-text-p u-padding-r-all">
                    I documenti sono in formato standard <b>PDF:</b> per la visualizzazione è possibile
                utilizzare il programma gratuito <b>Adobe Reader</b> o uno equivalente a scelta
                dell’utente
                </p>
            </article>
            <article class="Prose u-layout-prose-ritiro">
                <p class="u-layout-prose-ritiro u-color-grey-90 u-text-p u-padding-r-all">
                    Il timbro digitale riportato sul documento può essere verificato con il programma gratuito disponibile <a href="https://www.comune.roma.it/web/it/scheda-servizi.page?contentId=INF422279">"qui"</a>
                </p>
            </article>
            <article class="Prose u-layout-prose-ritiro">
                <p class="u-layout-prose-ritiro u-color-grey-90 u-text-p u-padding-r-all">
                    <b>Attenzione:</b> per motivi di sicurezza il tuo indirizzo di connessione (IP)
                verrà registrato e mantenuto a norma di legge.
                </p>
            </article>
        </div>
    </asp:Panel>
</asp:Content>
