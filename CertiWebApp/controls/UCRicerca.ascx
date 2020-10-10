<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCRicerca.ascx.cs" Inherits="Com.Unisys.CdR.Certi.WebApp.controls.UCRicerca" %>
<%@ Register Src="UCPaging.ascx" TagName="UCPaging" TagPrefix="uc2" %>
<!-- CODICE FISCALE -->
        <div class="tab">
            <button class="tablinks" id="defaultOpen" onclick="openTab(event, 'CodiceFiscale');return false;">Codice Fiscale</button>
            <button class="tablinks" onclick="openTab(event, 'DatiAnagrafici');return false;">Dati Anagrafici</button>
        </div>
        <div id="CodiceFiscale"  class="tabcontent u-background-grey-10">
            <div class="row">
                <div class="col-25">
                    <label class="Form-label is-required" for="CodFisc">Codice Fiscale</label>
                </div>
                <div class="col-75">
                    <asp:TextBox ID="CodFisc" TabIndex="1" runat="server"
                        MaxLength="16"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CFValidator" runat="server" ErrorMessage="*" ValidationGroup="CodFiscaleValidationGroup"
                        Font-Bold="True" ControlToValidate="CodFisc">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="CFRegExpVal" runat="server" ErrorMessage="!"
                        ValidationGroup="CodFiscaleValidationGroup"
                            Font-Bold="True" ControlToValidate="CodFisc" ValidationExpression="^[A-Za-z]{6}[0-9]{2}[A-Za-z]{1}[0-9]{2}[A-Za-z]{1}[A-Za-z_0-9]{3}[A-Za-z]{1}$">!</asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="Form-field Grid-cell" style="margin-top: 10px">
                <asp:Button ID="Annulla" runat="server" CssClass="Button Button--default" Text="Annulla"
                    ValidationGroup="CodFiscaleValidationGroup"
                    OnClick="AnnullaCodFiscale_Click"></asp:Button>
                <asp:Button ID="ConfermaCodFiscale" runat="server" 
                    ValidationGroup="CodFiscaleValidationGroup"                    
                    CausesValidation="true"
                    CssClass="Button Button--default" Text="Conferma"                    
                    OnClick="ConfermaCodFiscale_Click"></asp:Button>
            </div>
        </div>

        <!-- DATI ANAGRAFICI -->
        <div id="DatiAnagrafici" class="tabcontent u-background-grey-10">
            <div class="row">
                <div class="col-25">
                    <label class="Form-label is-required" for="Cognome">Cognome</label>
                </div>
                <div class="col-75">
                    <asp:TextBox ID="Cognome" TabIndex="3" runat="server"
                        MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CognomeValidator" runat="server" ErrorMessage="*"
                        ValidationGroup="DatiAnagraficiValidationGroup"
                        Font-Bold="True" ControlToValidate="Cognome">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="CognomeRegExpVal" runat="server" ErrorMessage="!"
                        ValidationGroup="DatiAnagraficiValidationGroup"
                        Font-Bold="True" ControlToValidate="Cognome" ValidationExpression="^[A-Z a-z\']{2,}">!</asp:RegularExpressionValidator><br />
                </div>
            </div>
            <div class="row">
                <div class="col-25">
                    <label class="Form-label is-required" for="Nome">Nome</label>
                </div>
                <div class="col-75">
                    <asp:TextBox ID="Nome" TabIndex="4" runat="server"
                        MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NomeValidator" runat="server" ErrorMessage="*"
                        ValidationGroup="DatiAnagraficiValidationGroup"
                        Font-Bold="True" ControlToValidate="Nome">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="NomeRegExpVal" runat="server" ErrorMessage="!"
                        ValidationGroup="DatiAnagraficiValidationGroup"
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
                            <asp:RangeValidator ControlToValidate="GGData" Type="Integer" ID="RangeGiorno" ErrorMessage="+"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                MinimumValue="0" MaximumValue="31" runat="server"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="GGDataValidator" runat="server" ErrorMessage="*"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                Font-Bold="True" ControlToValidate="GGData">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="GiornoRegExpVal" runat="server" ErrorMessage="!"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                Font-Bold="True" ControlToValidate="GGData" ValidationExpression="^[0-9]{2}">!</asp:RegularExpressionValidator>
                        </div>
                        <div class="Form-field Form-field--date Grid-cell">
                            <label class="Form-label" for="mese">Mese</label>
                            <asp:TextBox ID="MMData" TabIndex="6"
                                class="Form-input u-text-r-s u-borderRadius-m" runat="server"
                                MaxLength="2"></asp:TextBox>
                            <asp:RangeValidator ControlToValidate="MMData" Type="Integer"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                Display="Dynamic" ID="RangeMese" ErrorMessage="+" MinimumValue="0" MaximumValue="12" runat="server"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="MMDataValidator" runat="server" ErrorMessage="*"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                Font-Bold="True" ControlToValidate="MMData">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="MeseRegExpVal" runat="server" ErrorMessage="!"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                Font-Bold="True" ControlToValidate="MMData" ValidationExpression="^[0-9]{2}">!</asp:RegularExpressionValidator>
                        </div>
                        <div class="Form-field Form-field--date Grid-cell">
                            <label class="Form-label" for="anno">Anno</label>
                            <asp:TextBox ID="AAData"
                                class="Form-input u-text-r-s u-borderRadius-m"
                                TabIndex="7" runat="server"
                                MaxLength="4"></asp:TextBox>
                            <asp:RangeValidator ControlToValidate="AAData"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                Type="Integer" ID="RangeAnno" ErrorMessage="+" MinimumValue="1800" MaximumValue="2022" runat="server"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="AADataValidator" runat="server" ErrorMessage="*"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                Font-Bold="True" ControlToValidate="AAData">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="AnnoRegExpVal" runat="server" ErrorMessage="!"
                                ValidationGroup="DatiAnagraficiValidationGroup"
                                Font-Bold="True" ControlToValidate="AAData" ValidationExpression="^[0-9]{4}">!</asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="Form-field Grid-cell" style="margin-top: 10px">
                <asp:Button ID="AnnullaDatiAnagrafici"  runat="server" CssClass="Button Button--default" Text="Annulla"
                    OnClick="AnnullaDatiAnagrafici_Click"></asp:Button>
                <asp:Button ID="ConfermaDatiAnagrafici" runat="server" CssClass="Button Button--default" Text="Conferma"
                    ValidationGroup="DatiAnagraficiValidationGroup" CausesValidation="true"
                    OnClick="ConfermaDatiAnagrafici_Click"></asp:Button>
            </div>
        </div>
        <!-- PULSANTI -->

        <br />
        <asp:Panel runat="server" ID="PnlRisultati" Visible="false">
            <asp:GridView ID="gridDett" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="Table Table--withBorder js-TableResponsive tablesaw tablesaw-stack" data-tablesaw-mode="stack"
                PageSize="5" OnRowCommand="OnDettaglioRowCommand">
                <Columns>

                    <asp:BoundField DataField="CodiceFiscale"
                        HeaderText="Codice Fiscale">
                        <HeaderStyle Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CognomePersona"
                        HeaderText="Cognome">
                        <HeaderStyle Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NomePersona"
                        HeaderText="Nome">
                        <HeaderStyle Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SessoPersona"
                        HeaderText="Sesso">
                        <HeaderStyle Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" />
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

<script>
    document.getElementById("defaultOpen").click();
    function openTab(evt, cityName) {
        var i, tabcontent, tablinks;
        tabcontent = document.getElementsByClassName("tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }
        document.getElementById(cityName).style.display = "block";
         evt.currentTarget.className += " active";
    }
</script>
