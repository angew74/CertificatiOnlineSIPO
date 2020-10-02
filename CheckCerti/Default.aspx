<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Title="Controllo Certificati" Inherits="CheckCerti._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Pagina senza titolo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
    <thead>
    <th>
    <td>
    Maschera inserimento dati
    </td>
    </th>
    </thead>
    <tr>
    <td>
    <label>Web Service:</label>
    </td>
    <td><asp:TextBox runat="server" ID="txtWebService" Width="300" /></td>
    </tr> 
    <tr>
    <td>
    <label>username:</label>
    </td>
    <td><asp:Label runat="server" ID="txtUserName" Width="150" /></td>
    </tr>   
     <tr>
    <td>
    <label>clientid:</label>
    </td>
    <td><asp:Label runat="server" ID="txtClientId" Width="20" /></td>
     <td>
    <label>Sistema:</label>
    </td>
    <td><asp:Label runat="server" ID="txtSistema"  /></td>
    </tr>      
     <tr>
    <td>
    <label>codice fiscale Intestatario:</label>
    </td>
    <td><asp:TextBox runat="server" ID="txtCodiceFiscale" /></td>  
    <td>
    <label>codice fiscale Richiedente:</label>
    </td>
    <td><asp:TextBox runat="server" ID="txtRichiedente" /></td>
    </tr>   
     <tr>
    <td>
    <label>ID flusso:</label>
    </td>
    <td><asp:TextBox runat="server" ID="txtIDFlusso"  /></td>
    </tr> 
      <tr>
    <td>
    <label>ciu:</label>
    </td>
    <td><asp:Label runat="server" ID="txtCIU" Width="300" /></td>
    </tr>        
     <tr>
    <td>
    <label>Url Controllo pagamenti:</label>
    </td>
    <td><asp:Label runat="server" ID="txtUrlPagamenti" Width="300" /></td>
    </tr>      
    </table>
      <div>
      <asp:Button runat="server" ID="btnCercaPersona" OnClick="btnCercaPersona_OnClick" Text="1 Ricerca Persona" />
      <asp:Button runat="server" ID="btnCercaComponenti" OnClick="btnCercaComponenti_OnClick" Text="2 Ricerca Componenti" />
      <asp:Button runat="server" ID="btnCheckCredenziali" OnClick="btnCheck_Credenziali_OnClick" Text="3 Controlla Credenziali" />
      <asp:Button runat="server" ID="btnCheckEmettibilita" OnClick="btnCheckEmettibilita_OnClick" Text="4 Verifica Emettibilità" />
       <asp:Button runat="server" ID="btnRichiestaCertificati" OnClick="btnRichiesta_OnClick" Text="5 Richiesta Certificato" />
      <asp:Button runat="server" ID="btnCheckPagamenti" OnClick="btnCheckPagamenti_OnClick" Text="Controlla Pagamenti" />  
    </div>  
    </div>  
    <div>
    <asp:TextBox runat="server" ID="Risultato" Width="500" Height="300" TextMode="MultiLine" />
    </div>
    </div>
    </form>
</body>
</html>
