<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCRicerca.ascx.cs" Inherits="Com.Unisys.CdR.Certi.WebApp.controls.UCRicerca" %>
<%@ Register Src="UCPaging.ascx" TagName="UCPaging" TagPrefix="uc2" %>
<!-- CODICE FISCALE -->
<asp:Panel ID="PanelCodFisc" runat="server" CssClass="container" Visible="False">
    <div class="row" style="text-align: left">
        Ricerca per codice fiscale
        <asp:RadioButton ID="SearchCodFisc" runat="server" OnCheckedChanged="SearchCodFisc_CheckedChanged" AutoPostBack="True"></asp:RadioButton>
    </div>
    <div class="row">
        <div class="col-25">
            <label class="Form-label is-required" for="CodFisc">Codice Fiscale</label>
        </div>
        <div class="col-75">
            <asp:TextBox ID="CodFisc" TabIndex="2" runat="server"
                MaxLength="16" Enabled="False"></asp:TextBox><asp:RequiredFieldValidator ID="CFValidator" runat="server" ErrorMessage="*"
                    Font-Bold="True" ControlToValidate="CodFisc" Enabled="False">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="CFRegExpVal" runat="server" ErrorMessage="!"
                        Font-Bold="True" ControlToValidate="CodFisc" ValidationExpression="^[A-Za-z]{6}[0-9]{2}[A-Za-z]{1}[0-9]{2}[A-Za-z]{1}[A-Za-z_0-9]{3}[A-Za-z]{1}$">!</asp:RegularExpressionValidator>
        </div>
    </div>
</asp:Panel>
<!-- DATI ANAGRAFICI -->
<asp:Panel ID="PanelDatiAnag" runat="server" Visible="False" CssClass="container">
    <div class="row" style="text-align: left">
        Ricerca per dati anagrafici
        <asp:RadioButton ID="SearchDatiAnag" runat="server"
            AutoPostBack="True" OnCheckedChanged="SearchDatiAnag_CheckedChanged" Checked="False"></asp:RadioButton>
    </div>
    <div class="row">
        <div class="col-25">
            <label class="Form-label is-required" for="Cognome">Cognome</label>
        </div>
        <div class="col-75">
            <asp:TextBox ID="Cognome" TabIndex="3" runat="server"
                MaxLength="30"></asp:TextBox><asp:RequiredFieldValidator ID="CognomeValidator" runat="server" ErrorMessage="*"
                    Font-Bold="True" ControlToValidate="Cognome">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="CognomeRegExpVal" runat="server" ErrorMessage="!"
                        Font-Bold="True" ControlToValidate="Cognome" ValidationExpression="^[A-Z a-z\']{2,}">!</asp:RegularExpressionValidator><br />
        </div>
    </div>
    <div class="row">
        <div class="col-25">
            <label class="Form-label is-required" for="Nome">Nome</label>
        </div>
        <div class="col-75">
            <asp:TextBox ID="Nome" TabIndex="4" runat="server"
                MaxLength="30"></asp:TextBox><asp:RequiredFieldValidator ID="NomeValidator" runat="server" ErrorMessage="*"
                    Font-Bold="True" ControlToValidate="Nome">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="NomeRegExpVal" runat="server" ErrorMessage="!"
                        Font-Bold="True" ControlToValidate="Nome" ValidationExpression="^[A-Z a-z\']{2,}">!</asp:RegularExpressionValidator><br />
        </div>
    </div>
    <div class="row">
        <div class="col-25">
            <label class="Form-label is-required" for="Sex">Sesso</label>
        </div>
        <div class="col-75" style="text-align: left">
            <asp:RadioButtonList ID="Sex" runat="server" Width="72px" Height="20px" RepeatDirection="Horizontal"
                RepeatLayout="Flow">
                <asp:ListItem Value="M" Selected="True">M</asp:ListItem>
                <asp:ListItem Value="F">F</asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>
    <div class="row" style="text-align: left; margin-top: 20px">
        <div class="col-25">
            <label class="Form-label is-required" for="Sex">Data di nascita</label>
        </div>
        <div class="col-75" style="text-align: left">
            <div class="Grid Grid--fit Grid--withGutter">
                <div class="Form-field Form-field--date Grid-cell">
                    <label class="Form-label" for="giorno">Giorno</label>
                    <asp:TextBox ID="GGData" TabIndex="5" class="Form-input u-text-r-s u-borderRadius-m" runat="server"
                        MaxLength="2"></asp:TextBox>
                    <asp:RangeValidator ControlToValidate="GGData" Type="Integer" ID="RangeGiorno" ErrorMessage="+" MinimumValue="0" MaximumValue="31" runat="server"></asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="GGDataValidator" runat="server" ErrorMessage="*"
                        Font-Bold="True" ControlToValidate="GGData">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="GiornoRegExpVal" runat="server" ErrorMessage="!"
                            Font-Bold="True" ControlToValidate="GGData" ValidationExpression="^[0-9]{2}">!</asp:RegularExpressionValidator>
                </div>
                <div class="Form-field Form-field--date Grid-cell">
                    <label class="Form-label" for="mese">Mese</label>
                    <asp:TextBox ID="MMData" TabIndex="6"
                        class="Form-input u-text-r-s u-borderRadius-m" runat="server"
                        MaxLength="2"></asp:TextBox>
                    <asp:RangeValidator ControlToValidate="MMData" Type="Integer"
                        Display="Dynamic" ID="RangeMese" ErrorMessage="+" MinimumValue="0" MaximumValue="12" runat="server"></asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="MMDataValidator" runat="server" ErrorMessage="*"
                        Font-Bold="True" ControlToValidate="MMData">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="MeseRegExpVal" runat="server" ErrorMessage="!"
                            Font-Bold="True" ControlToValidate="MMData" ValidationExpression="^[0-9]{2}">!</asp:RegularExpressionValidator>
                </div>
                <div class="Form-field Form-field--date Grid-cell">
                    <label class="Form-label" for="anno">Anno</label>
                    <asp:TextBox ID="AAData"
                        class="Form-input u-text-r-s u-borderRadius-m"
                        TabIndex="7" runat="server"
                        MaxLength="4"></asp:TextBox>
                    <asp:RangeValidator ControlToValidate="AAData" Type="Integer" ID="RangeAnno" ErrorMessage="+" MinimumValue="1800" MaximumValue="2022" runat="server"></asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="AADataValidator" runat="server" ErrorMessage="*"
                        Font-Bold="True" ControlToValidate="AAData">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="AnnoRegExpVal" runat="server" ErrorMessage="!"
                            Font-Bold="True" ControlToValidate="AAData" ValidationExpression="^[0-9]{4}">!</asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
    </div>

</asp:Panel>
<!-- PULSANTI -->
<asp:Panel ID="PanelButton" runat="server" CssClass="Form-field Grid-cell" Style="margin-top: 10px">
    <asp:Button ID="Annulla" runat="server" CssClass="Button Button--default" Text="Annulla"
        OnClick="Annulla_Click"></asp:Button>
    <asp:Button ID="Conferma" runat="server" CssClass="Button Button--default" Text="Conferma"
        OnClick="Conferma_Click"></asp:Button>
</asp:Panel>
<br />
<asp:Panel runat="server" ID="PnlRisultati" Visible="false">
    <asp:GridView ID="gridDett" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="Table Table--withBorder js-TableResponsive tablesaw tablesaw-stack" data-tablesaw-mode="stack"
        PageSize="5" OnRowCommand="OnDettaglioRowCommand">
        <Columns>

            <asp:BoundField DataField="CodiceFiscale"
                HeaderText="Codice Fiscale">
                <HeaderStyle Wrap="False" />
                <ItemStyle HorizontalAlign="Center"  />
            </asp:BoundField>
            <asp:BoundField DataField="CognomePersona"
                HeaderText="Cognome">
                <HeaderStyle Wrap="False" />
                <ItemStyle HorizontalAlign="Center"  />
            </asp:BoundField>
            <asp:BoundField DataField="NomePersona"
                HeaderText="Nome">
                <HeaderStyle Wrap="False" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="SessoPersona"
                HeaderText="Sesso">
                <HeaderStyle Wrap="False" />
                <ItemStyle HorizontalAlign="Center"  />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Nato Il">
                <ItemTemplate>
                    <asp:Label ID="lblDataNasc" runat="server" Text='<%# FormatDataAMGToGMA(Eval("DataDiNascitaPersona").ToString().Trim(), "/") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle Wrap="False" />
                <ItemStyle HorizontalAlign="Center" Width="80px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CodiceIndividuale" Visible="false" HeaderStyle-Wrap="false" ItemStyle-Width="15px"
                ItemStyle-CssClass="hide" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblCodInd" runat="server" Text='<%#  Eval("CodiceIndiv").ToString().Trim() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Descrizione" Visible="false"
                HeaderText="Descrizione">
                <HeaderStyle Wrap="False" />
                <ItemStyle HorizontalAlign="Center" Width="130px" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="ibVisualizza" runat="server" CausesValidation="false" ImageUrl="~/build/vendor/iconfinder.png"
                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="Visualizza" ToolTip="seleziona" />
                </ItemTemplate>
                <HeaderStyle Wrap="False" />
                <ItemStyle HorizontalAlign="Center" Width="30px" />
            </asp:TemplateField>
        </Columns>
        <PagerTemplate>
            <uc2:UCPaging ID="ucPaging" runat="server" OnPagerIndexChanged="OnPagerIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</asp:Panel>
