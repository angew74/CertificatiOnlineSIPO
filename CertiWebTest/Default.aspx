<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CertiWebTest._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button" runat="server" OnClick="Button1_Click" Text="NASCITA" />
        &nbsp;
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="MORTE" />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="MATRIMONIO" />&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="CITTADINANZA" />
        <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="RESIDENZA" />
        <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="APpl" Width="187px" /></div>
        <br />
        <asp:Button ID="Button6" runat="server" Text="STATO LIBERO" Width="117px" OnClick="Button6_Click" />
        <asp:Button ID="Button7" runat="server" Text="CITTADINANZA AIRE" Width="142px" OnClick="Button7_Click" />
        <asp:Button ID="Button8" runat="server" OnClick="Button8_Click" Text="RESIDENZAeCITTADINANZAAIRE"
            Width="229px" />
        <asp:Button ID="Button9" runat="server" Height="23px" OnClick="Button9_Click" Text="RESIDENZA_AIRE"
            Width="158px" />
        <asp:Button ID="Button10" runat="server" OnClick="Button10_Click" Text="RESIDENZA_E STATO LIBERO"
            Width="216px" />
        <asp:Button ID="Button11" runat="server" Text="RESIDENZA_CITTADINANZA_STATOLIBERO"
            Width="296px" OnClick="Button11_Click" /><div>
                &nbsp;
                <asp:TextBox ID="TextBox1" runat="server" Height="129px" TextMode="MultiLine" Width="834px"></asp:TextBox>
                <asp:Button ID="Button12" runat="server" Text="RESIDENZA_CITTADINANZA_STATOCIVILE_NASCITA_DIRITTIPOLITICI" OnClick="Button12_Click" />
                <asp:Button ID="Button13" runat="server" OnClick="Button13_Click" Text="RESIDENZAeCITTADINANZA"
                    Width="223px" />
                <asp:Button ID="Button14" runat="server" OnClick="Button14_Click" Text="STATOdiFAMIGLIA"
                    Width="154px" />
                <asp:Button ID="Button16" runat="server" Text="STATO DI FAMIGLIA AIRE" Width="222px" OnClick="Button16_Click" />
                <asp:Button ID="Button15" runat="server" OnClick="Button15_Click" Text="GodimentoDirittiPolitici"
                    Width="197px" />
                <asp:Button ID="Button17" runat="server" Text="RESIDENZA_CITTAD_STATCIV_NASCITA_STFAM_DIRITTIPOLITICI"
                    Width="549px" OnClick="Button17_Click" /></div>
    </form>
</body>
</html>
