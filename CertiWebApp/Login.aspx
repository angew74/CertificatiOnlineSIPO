<%@ Page Language="C#"  autoEventWireup="true" CodeBehind="Login.aspx.cs" Theme="unisysportale" Inherits="Login" Title="accesso area Riservata" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml"> 
<head id="Head1" runat="server">
 <title>Login</title>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="content-type" content="text/html" />
    <meta http-equiv="MSThemeCompatible" content="false" />
    <meta http-equiv="imagetoolbar" content="false" />
    <meta content="true" name="MSSmartTagsPreventParsing" />
    <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
<style type="text/css"> 
BODY
{
	text-align: center;
	padding-bottom: 0px;
	margin: 0px;
	padding-left: 0px;
	padding-right: 0px;
	font-family: Verdana, arial, Geneva, Helvetica, sans-serif;
	height: 100%;
	padding-top: 0px;
}
HTML
{
	height: 100%;
}
BODY
{
	height: 100%;
}
form
{
	border-bottom: medium none;
	border-left: medium none;
	padding-bottom: 0.5em;
	background-color: #f0f0f0;
	margin: 0px;
	padding-left: 0px;
	padding-right: 0px;
	border-top: medium none;
	border-right: medium none;
	padding-top: 0px;
}
p
{
	padding-bottom: 0px;
	margin: 0px;
	padding-left: 0px;
	padding-right: 0px;
	padding-top: 0px;
}
h1
{
	padding-bottom: 0px;
	margin: 0px;
	padding-left: 0px;
	padding-right: 0px;
	padding-top: 0px;
}
h2
{
	padding-bottom: 0px;
	margin: 0px;
	padding-left: 0px;
	padding-right: 0px;
	padding-top: 0px;
}
H3
{
	padding-bottom: 0px;
	margin: 0px;
	padding-left: 0px;
	padding-right: 0px;
	padding-top: 0px;
}
img
{
	border-bottom: medium none;
	border-left: medium none;
	border-top: medium none;
	border-right: medium none;
}
HR
{
	display: none;
}
fieldset
{
	border-bottom: #ccc 1px solid;
	border-left: #ccc 1px solid;
	padding-bottom: 0px;
	margin: 1em 0.5em 0px;
	padding-left: 0.5em;
	padding-right: 0.5em;
	border-top: #ccc 1px solid;
	border-right: #ccc 1px solid;
	padding-top: 0px;
}
legend
{
	text-align: left;
	margin-bottom: 1em;
	color: #b12725;
	font-size: 0.8em;
	font-weight: bold;
}
.hidden
{
	position: absolute;
	width: 2px;
	height: 0px;
	visibility: hidden;
	font-size: 0.1em;
	overflow: hidden;
	left: -1000em;
}
a
{
	color: #b12725;
	text-decoration: none;
}
.frmbtn
{
	border-bottom: medium none;
	border-left: medium none;
	padding-bottom: 0px;
	background-color: #7c7dbe;
	margin: 0.5em 0px 1em;
	padding-left: 0.5em;
	padding-right: 0.5em;
	color: #fff;
	font-size: 0.7em;
	border-top: medium none;
	cursor: pointer;
	border-right: medium none;
	padding-top: 0px;
}
.frmbtn
{
	border-bottom: medium none;
	border-left: medium none;
	padding-bottom: 0.2em;
	background-color: #7c7dbe;
	margin: 0.5em 0px 1em;
	min-height: 16px;
	padding-left: 0.5em;
	padding-right: 0.5em;
	color: #fff;
	font-size: 0.7em;
	border-top: medium none;
	cursor: pointer;
	border-right: medium none;
	padding-top: 0.2em;
}
fieldset p INpUT
{
	width: 20em;
	margin-bottom: 1em;
}
.left
{
	float: left;
}
.right
{
	float: right;
}
.clear
{
	clear: both;
}
.txtright
{
	text-align: right;
}
#layout
{
	border-left: #ccc 1px solid;
	margin: 0px auto;
	width: 768px;
	border-right: #ccc 1px solid;
}
#head
{
	border-left: #ccc 1px solid;
	margin: 0px auto;
	width: 768px;
	border-right: #ccc 1px solid;
}
#layout
{
	border-bottom: #ccc 2px solid;
}
#head
{
	text-align: left;
	background: url(images/testataPcr.jpg) #ff9629 repeat-x;
	padding-top: 120px;
}
div#titoloarea
{
	text-align: right;
	padding-bottom: 0px;
	padding-left: 0px;
	padding-right: 330px;
	font-size: 0.9em;
	font-weight: bold;
	padding-top: 0.2em;
}
#subtestata
{
	border-bottom: #fff 1px solid;
	background-color: #f7f7f7;
	min-height: 36px;
	height: auto !important;
}
#footerhp
{
	text-align: right;
	margin: 0px auto;
	width: 768px;
}
#footerhp ul
{
	padding-bottom: 0px;
	list-style-type: none;
	margin-top: 4px;
	padding-left: 0px;
	padding-right: 0px;
	list-style-image: none;
	padding-top: 0px;
}
#footerhp ul li
{
	display: inline;
}
#quickmenu
{
	position: absolute;
	width: 300px;
	height: 10px;
	overflow: hidden;
	left: -1000em;
}
#quickmenu *
{
	visibility: visible;
}
#formricerca .right
{
	border-bottom: medium none;
	text-align: left;
	border-left: medium none;
	padding-bottom: 4px;
	padding-left: 35px;
	width: 18.5em !important;
	padding-right: 6px;
	max-width: 300px;
	margin-bottom: 0.3em;
	background: url(images/cerca.gif) no-repeat;
	font-size: 0.7em;
	border-top: medium none;
	border-right: medium none;
	padding-top: 0px;
}
#formricerca label
{
	padding-bottom: 2px;
	background-color: #ccc;
	padding-left: 5px;
	padding-right: 5px;
	color: #000;
	font-weight: bold;
	padding-top: 2px;
}
#formricerca INpUT#cerca
{
	border-bottom: #ccc 1px solid;
	border-left: #ccc 1px solid;
	margin: 3px 0px 0px;
	font-size: 1em;
	border-top: #ccc 1px solid;
	border-right: #ccc 1px solid;
}
#formricerca INpUT#submit
{
	border-bottom: #990000 1px solid;
	border-left: #990000 1px solid;
	padding-bottom: 0px;
	background-color: #990000;
	margin: 0px 0px 0px 0.5em;
	padding-left: 0.2em;
	padding-right: 0.2em;
	color: #fff;
	font-size: 95%;
	border-top: #990000 1px solid;
	cursor: pointer;
	font-weight: bold;
	border-right: #990000 1px solid;
	padding-top: 0px;
}
.containersx
{
	position: relative;
}
ul#macro
{
	border-bottom: medium none;
	position: absolute;
	border-left: medium none;
	list-style-type: none;
	min-height: 14px;
	padding-left: 0px;
	width: 28.5em;
	padding-right: 20px;
	background: url(images/titoloGruppo-hr.gif) no-repeat right top;
	color: #fff;
	font-size: 0.65em;
	border-top: medium none;
	top: -36px !important;
	list-style-image: none;
	border-right: medium none;
	left: 0px !important;
}
ul#macro li
{
	background-image: none;
	border-bottom: medium none;
	border-left: medium none;
	padding-bottom: 0.25em;
	margin: 0px;
	padding-left: 0.6em;
	padding-right: 0.7em;
	float: left;
	border-top: medium none;
	border-right: #fe8f23 1px solid;
	padding-top: 1px;
}
ul#macro li.last
{
	border-right: medium none;
}
ul#macro li a
{
	background-color: #8c0029;
	color: #fff;
	font-weight: bold;
}
H3
{
	border-bottom: #f8f8f8 1px solid;
	padding-bottom: 0.2em;
	padding-left: 0.4em;
	padding-right: 65px;
	background:  url(images/titoloServizi.gif) #ccccff no-repeat 100% 0px;
	color: #fff;
	font-size: 0.9em;
	padding-top: 0.2em;
}
#content
{
	text-align: left;
	padding-bottom: 0px;
	margin: auto;
	padding-left: 8px;
	width: 470px;
	padding-right: 8px;
	padding-top: 25px;
}
#content p
{
	font-size: 0.75em;
}
p.content
{
	text-align: left;
	padding-bottom: 70px;
	margin: 8px auto 0px;
	width: 455px;
	font-size: 0.7em;
}
p.content a
{
	font-weight: normal;
	text-decoration: underline;
}
 
</style>
</head>
 
 
<body>
<!-- menu rapido -->
<p id="quickmenu" class="hidden">
<a name="quickmenu"><strong>menu di scelta rapida</strong></a><br /><a accessKey="4" href="https://www.comune.roma.it/servizi/certificati/emissione/emissione.aspx#content">vai ai contenuti [4]</a> &nbsp;|&nbsp; <a accessKey="5" href="https://www.comune.roma.it/servizi/certificati/emissione/emissione.aspx#macro">navigazione del sito [5]</a> &nbsp;|&nbsp; <a href="https://www.comune.roma.it/servizi/certificati/emissione/emissione.aspx#cerca">cerca nel sito [6]</a> 
</p>
<hr/>
<!-- testata -->
<div id="head">
<h1 class="hidden">
portale del Comune di Roma
</h1>
<hr/>
<h2 class="hidden">
<a name="wpsportletsx">Navigazione del sito</a>
</h2>
<div class="containersx">
<ul id="macro">
<li class="first">
<a accessKey="4" href="http://www.comune.roma.it/was/wps/portal/pcr" name="macro">Home</a>
</li>
<li>
<a accessKey="5" href="http://www.comune.roma.it/was/wps/portal/!ut/p/_s.7_0_a/7_0_21L?menupage=/&targetpage=/Menu_Orizzontale/Mappa/index.jsp">Mappa</a>
</li>
<li class="last">
<a accessKey="6" href="http://www.comune.roma.it/was/wps/portal/!ut/p/_s.7_0_a/7_0_21L?menupage=/&targetpage=/Menu_Orizzontale/Guida_alla_navigazione/index.jsp">Guida alla navigazione</a>
</li>
</ul>
</div>
<!-- subtestata -->
<div id="subtestata">
<h2 class="hidden">
Ricerca
</h2> 
<div class="right">
<label for="cerca">Ricerca</label><SpaN class="hidden"> &nbsp;</SpaN><INpUT accessKey="3" id="cerca" size="17" name="q" value="" /> <INpUT type="hidden" name="filter" value="0" /><INpUT id="submit" title="avvia la ricerca" alt="avvia la ricerca" type="submit" value="Vai" /> 
</div>
 
<!-- titolo area -->
<div id="titoloarea">
<hr/>
<p>
area riservata
</p>
</div>
<hr/>
<div class="clear">
</div>
</div>
</div>
<!-- fine testata -->
<!-- layout -->
<div id="layout">
<div id="areacontenuti" class="hidden">
<hr/>
<h2>
Contenuti
</h2>
<p>
<a href="https://www.comune.roma.it/servizi/certificati/emissione/emissione.aspx#quickmenu">torna al menu di scelta rapida</a>
</p>
</div>
<div id="content" style="margin-left:210px">
<form id="autenticazione" runat="server">
<H3>
accesso utente
</H3>
<fieldset id="loginfield">
<legend>Inserimento dati di accesso</legend>
<p class="left">
<strong></strong>
</p>
<p>
&nbsp;
</p>
<p>
&nbsp;
</p>
<p class="left">
<label for="username">Identificativo:</label>
</p>
<p class="txtright">
<asp:TextBox ID="username" MaxLength="40"  runat="server"  />
</p>
<p class="left">
<label for="password">password:</label>
</p>
<p class="txtright">
<asp:TextBox ID="password" runat="server"  TextMode="Password" MaxLength="40"  />
</p>
<div class="txtright">
<asp:Button runat="server" ID="accedi" CssClass="frmbtn" OnClick="Accedi_Click" Text="accedi" ToolTip="Entra nell'area riservata" />
   
</div>
<p>
<br /><strong>per effettuare l'accesso tramite Smart Card CNS clicca <a title="accesso tramite Smart Card" href="https://www.comune.roma.it/cert/smartcard/auth_SmartCard">qui</a>.</strong>
</p>
<br />
</fieldset>
 </form>
</div>
<p class="content" style="margin-left:220px;margin-top:20px">
<strong>Importante:</strong><br />per effettuare l'accesso all'area riservata occorre essere in possesso di un <em>identificativo utente</em> e di una <EM>password</EM> valida. Se non ne sei in possesso puoi richiederli alla pagina di <a title="vai alla Identificazione al portale" href="http://www.comune.roma.it/was/wps/portal/!ut/p/_s.7_0_a/7_0_TD2?menupage=/area_di_navigazione/area_identificazione/Identificazione_al_portale/">Identificazione al portale</a>.
</p>
</div>
<!-- fine layout -->
<!-- footer -->
<div id="footerhp">
<hr/>
<p class="hidden">
Collegamenti al sito del <a title="vai al sito del W3C" href="http://www.w3.org/">W3C</a> (World Wide Web Consortium) per la validazione di questa pagina:
</p>
<ul>
<li>
<a title="Verifica la validit� del codice XHTML 1.0" href="http://validator.w3.org/check?uri=http://www.comune.roma.it/was/wps/portal/pcr"><img alt="XHTML 1.0 Valido!" src="App_Themes/unisysPortale/images/w3cXHTML.gif" width="88" height="31" /></a>
</li>
<li>
<a title="Verifica la validit� dei fogli di stile" href="http://jigsaw.w3.org/css-validator/validator?uri=http://www.comune.roma.it/was/wps/portal/pcr"><img alt="CSS Valido!" src="App_Themes/unisysPortale/images/w3cCSS.gif" width="88" height="31" /></a>
</li>
</ul>
</div>
</body>
 
</html>
