using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Com.Unisys.Logging;
using Com.Unisys.CdR.Certi.WebApp.Business;
using Com.Unisys.CdR.Certi.Objects.Common;
using System.Collections.Generic;
using log4net;
using Com.Unisys.CdR.Certi.WebApp.common;
using System.Globalization;
using Com.Unisys.CdR.Certi.Caching;

namespace Com.Unisys.CdR.Certi.WebApp.controls
{
    public partial class UCRicerca : System.Web.UI.UserControl
    {
        #region Properties

        private bool _BottoneVisualizza = false;
        /// <summary>
        /// 
        /// </summary>
        private enum col
        {

            CodiceFiscale,
            CognomePersona,
            NomePersona,
            SessoPersona,
            DataDiNascitaPersona,
            CodiceIndividuale,
            Descrizione,
            BottoneVisualizza

        }

        #endregion



        private static readonly ILog _log = LogManager.GetLogger(typeof(UCRicerca));

        #region Handlers
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ClickRicerca;

        /// <summary>
        /// 
        /// </summary>
        public event IndividuoHandler SelectIndividuo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persona"></param>
        /// <param name="modifica"></param>
        public delegate void IndividuoHandler(NCRIRICIND.PersonaElencoRow persona, bool modifica);

        /// <summary>
        /// 
        /// </summary>
        public delegate void NoDataHandler();

        /// <summary>
        /// 
        /// </summary>
        public event NoDataHandler NoData;


        #endregion

        DataTable messaggi = new DataTable();
        IList<Messaggio> msg = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                PanelCodFisc.Visible = true;
                PanelDatiAnag.Visible = true;
                SearchCodFisc.Checked = false;
                SearchDatiAnag.Checked = true;
                gridDett.DataSource = null;
                gridDett.DataBind();
                PnlRisultati.Visible = false;
                Conferma.Focus();
                RangeAnno.MaximumValue = DateTime.Now.Year.ToString();
            }
        }

        private void DisableAll()
        {
          
            CodFisc.Enabled = false;
            Cognome.Enabled = false;
            Nome.Enabled = false;
            GGData.Enabled = false;
            MMData.Enabled = false;
            AAData.Enabled = false;
            Sex.Enabled = false;
            RangeAnno.Enabled = false;
            RangeGiorno.Enabled = false;
            RangeMese.Enabled = false;
            //Validators           
            CFValidator.Enabled = false;
            CFRegExpVal.Enabled = false;
            CognomeValidator.Enabled = false;
            NomeValidator.Enabled = false;
            CognomeRegExpVal.Enabled = false;
            NomeRegExpVal.Enabled = false;
            AnnoRegExpVal.Enabled = false;
            GiornoRegExpVal.Enabled = false;
            MeseRegExpVal.Enabled = false;
            AADataValidator.Enabled = false;
            MMDataValidator.Enabled = false;
            GGDataValidator.Enabled = false;
            CodFisc.Text = "";
            Cognome.Text = "";
            Nome.Text = "";
            GGData.Text = "";
            AAData.Text = "";
            MMData.Text = "";
            SearchDatiAnag.Checked = false;
            SearchCodFisc.Checked = false;
            gridDett.DataSource = null;
            gridDett.DataBind();
            PnlRisultati.Visible = false;

        }
        protected void SearchCodFisc_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            DisableAll();
            SearchCodFisc.Checked = true;
            CodFisc.Enabled = true;
            CFValidator.Enabled = true;
            CFRegExpVal.Enabled = true;
            Conferma.Enabled = true;
        }

        protected void SearchDatiAnag_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            DisableAll();
            SearchDatiAnag.Checked = true;
            Cognome.Enabled = true;
            Nome.Enabled = true;
            GGData.Enabled = true;
            MMData.Enabled = true;
            AAData.Enabled = true;
            Sex.Enabled = true;
            CognomeRegExpVal.Enabled = true;
            NomeRegExpVal.Enabled = true;
            NomeValidator.Enabled = true;
            AnnoRegExpVal.Enabled = true;
            MeseRegExpVal.Enabled = true;
            RangeAnno.Enabled = true;
            RangeGiorno.Enabled = true;
            RangeMese.Enabled = true;
            GiornoRegExpVal.Enabled = true;
            CognomeValidator.Enabled = true;
            NomeValidator.Enabled = false;
            AADataValidator.Enabled = true;
            GGDataValidator.Enabled = true;
            MMDataValidator.Enabled = true;
            RangeAnno.MaximumValue = DateTime.Now.Year.ToString();
            Conferma.Enabled = true;
            //RegisterOnSubmitStatement("checkData1", "return(checkData1(document.forms[0].GGData,document.forms[0].MMData,document.forms[0].AAData));")
        }
        protected void Conferma_Click(System.Object sender, System.EventArgs e)
        {
            NCRIRICIND ret = null;
            string cf = null;
            string ci = string.Empty;
            string surname = null;
            string name = null;
            string gg = string.Empty;
            string mm = string.Empty;
            string aa = string.Empty;
            string sesso = null;
            string[] parametri = new string[10];
            if (SearchCodFisc.Checked == true)
            {
                try
                {
                    cf = CodFisc.Text.ToUpper().Trim();
                    // N.R. 09/2020
                   //  ret = BusGestioneRicerche.FindByCodiceFiscale(cf, "0", "5", "Nessuno", string.Empty, string.Empty,string.Empty);
                    ret = BusGestioneRicercheSIPO.FindByCodiceFiscale(cf,"");
                    messaggi = ret.Messaggi;
                    // Save the type of search

                    if (messaggi.Rows.Count > 0)
                    {
                        ListaMessaggi lm = new ListaMessaggi(messaggi);
                        if (lm.getMessage("000276", out msg))
                        {
                            (this.Page as BasePage).info.AddMessage(msg[0].ToString(), LivelloMessaggio.ERROR);
                            // evento
                            if (NoData != null) 
                                NoData();
                        }
                        else
                            (this.Page as BasePage).info.AddMessage(messaggi, LivelloMessaggio.ERROR);
                        gridDett.Visible = false;
                    }
                    else
                    {
                        // salvo ultima ricerca
                        gridDettDataBind(ret);
                        if (ClickRicerca != null)
                            ClickRicerca(sender, e);
                    }
                }
                catch (ManagedException me)
                {
                    ((BasePage)this.Page).info.AddMessage(me.Message, LivelloMessaggio.ERROR);
                }
                catch (System.Exception ex)
                {
                      _log.Error(ex.Message);
                    ((BasePage)this.Page).info.AddMessage(ex.ToString(), LivelloMessaggio.ERROR);
                }


            }
            else if (SearchDatiAnag.Checked == true)
            {

                try
                {
                    surname = Cognome.Text;
                    name = Nome.Text;
                    if (!string.IsNullOrEmpty(GGData.Text))
                        gg = GGData.Text;
                    if (!string.IsNullOrEmpty(MMData.Text))
                        mm = MMData.Text;
                    if (!string.IsNullOrEmpty(AAData.Text))
                        aa = AAData.Text;
                    if (Sex.SelectedIndex == 0)
                        sesso = "M";
                    else
                        sesso = "F";
                    // N.R. 09/2020
                   // ret = BusGestioneRicerche.FindByDatiAnagrafici(name, surname,
                     //                                                   aa, mm, gg,
                       //                                                 sesso, "0", "5", "Nessuno", string.Empty, string.Empty,string.Empty);
                    ret = BusGestioneRicercheSIPO.FindByDatiAnagrafici(name, surname,
                                                                     aa, mm, gg,
                                                                     sesso,"");
                    messaggi = ret.Messaggi;
                    // Save the type of search
                    NCRIRICIND.PersonaElencoDataTable tb = new NCRIRICIND.PersonaElencoDataTable();
                    if (messaggi.Rows.Count > 0)
                    {
                        ListaMessaggi lm = new ListaMessaggi(messaggi);
                        if (lm.getMessage("000276", out msg))
                        {
                            (this.Page as BasePage).info.AddMessage("Nessun ritrovamento", LivelloMessaggio.ERROR);
                            // evento
                            if (NoData != null)
                                NoData();
                        }
                        else
                            (this.Page as BasePage).info.AddMessage(messaggi, LivelloMessaggio.ERROR);
                        gridDett.Visible = false;
                        PnlRisultati.Visible = false;
                    }
                    else
                    {
                        NCRIRICIND.PersonaElencoRow[] rows = (NCRIRICIND.PersonaElencoRow[])ret.PersonaElenco.Select("DataDiNascitaPersona <> '00000000' ","");
                        foreach (NCRIRICIND.PersonaElencoRow row in rows)
                        {
                            tb.ImportRow(row);
                        }
                        NCRIRICIND newret = new NCRIRICIND();
                        newret.PersonaElenco.Merge(tb);
                        newret.Elenco.Merge(ret.Elenco);
                        gridDettDataBind(newret);
                        if (ClickRicerca != null)
                            ClickRicerca(sender, e);

                        // eventi per ricerche specifiche

                    }
                }
                catch (ManagedException me)
                {
                    ((BasePage)this.Page).info.AddMessage(me.Message, LivelloMessaggio.ERROR);
                }
                catch (System.Exception ex)
                {
                    _log.Error(ex.ToString());
                    ((BasePage)this.Page).info.AddMessage(ex.ToString(), LivelloMessaggio.ERROR);
                }

            }         

        }

        protected void Annulla_Click(System.Object sender, System.EventArgs e)
        {
            DisableAll();
            SearchDatiAnag.Checked = true;
            Cognome.Enabled = true;
            Cognome.Text = string.Empty;
            Nome.Text = string.Empty;
            GGData.Text = string.Empty;
            MMData.Text = string.Empty;
            AAData.Text = string.Empty;
            Nome.Enabled = true;
            GGData.Enabled = true;
            MMData.Enabled = true;
            AAData.Enabled = true;
            Sex.Enabled = true;
            RangeAnno.Enabled = true;
            RangeGiorno.Enabled = true;
            RangeMese.Enabled = true;
            CognomeRegExpVal.Enabled = true;
            NomeRegExpVal.Enabled = true;
            AnnoRegExpVal.Enabled = true;
            MeseRegExpVal.Enabled = true;
            GiornoRegExpVal.Enabled = true;
            CognomeValidator.Enabled = true;
            NomeValidator.Enabled = true;
            AADataValidator.Enabled = true;
            GGDataValidator.Enabled = true;
            MMDataValidator.Enabled = true;
            Conferma.Enabled = true;
        }
        protected void OnPagerIndexChanged(string sPaginaRichiesta)
        {
            NCRIRICIND ret = null;
            string cf = null;
            string ci = string.Empty;
            string surname = null;
            string name = null;
            string gg = string.Empty;
            string mm = string.Empty;
            string aa = string.Empty;
            string sesso = null;
            ProfiloUtente richiedente = SessionManager<ProfiloUtente>.get(SessionKeys.RICHIEDENTE_CERTIFICATI);
            if (SearchCodFisc.Checked == true)
            {
                try
                {
                   
                    cf = CodFisc.Text.ToUpper().Trim();
                    ret = BusGestioneRicercheSIPO.FindByCodiceFiscale(cf,richiedente.CodiceFiscale);
                    messaggi = ret.Messaggi;
                    // Save the type of search

                    if (messaggi.Rows.Count > 0)
                    {
                        ListaMessaggi lm = new ListaMessaggi(messaggi);
                        if (lm.getMessage("000276", out msg))
                        {
                            (this.Page as BasePage).info.AddMessage(msg[0].ToString(), LivelloMessaggio.ERROR);
                            // evento
                            if (NoData != null)
                                NoData();
                        }
                        else
                            (this.Page as BasePage).info.AddMessage(messaggi, LivelloMessaggio.ERROR);
                        gridDett.Visible = false;
                    }
                    else
                    {
                        // salvo ultima ricerca
                        gridDettDataBind(ret);
                     
                    }
                }
                catch (ManagedException me)
                {
                    ((BasePage)this.Page).info.AddMessage(me.Message, LivelloMessaggio.ERROR);
                }
                catch (System.Exception ex)
                {                   
                    ((BasePage)this.Page).info.AddMessage(ex.ToString(), LivelloMessaggio.ERROR);
                }


            }
            else if (SearchDatiAnag.Checked == true)
            {

                try
                {
                    surname = Cognome.Text;
                    name = Nome.Text;
                    if (!string.IsNullOrEmpty(GGData.Text))
                        gg = GGData.Text;
                    if (!string.IsNullOrEmpty(MMData.Text))
                        mm = MMData.Text;
                    if (!string.IsNullOrEmpty(AAData.Text))
                        aa = AAData.Text;
                    if (Sex.SelectedIndex == 0)
                        sesso = "M";
                    else
                        sesso = "F";
                    ret = BusGestioneRicercheSIPO.FindByDatiAnagrafici(name, surname,
                                                                        aa, mm, gg,
                                                                        sesso,richiedente.CodiceFiscale);
                    messaggi = ret.Messaggi;
                    // Save the type of search

                    if (messaggi.Rows.Count > 0)
                    {
                        ListaMessaggi lm = new ListaMessaggi(messaggi);
                        if (lm.getMessage("000276", out msg))
                        {
                            (this.Page as BasePage).info.AddMessage(msg[0].ToString(), LivelloMessaggio.ERROR);
                            // evento
                            if (NoData != null)
                                NoData();
                        }
                        else
                            (this.Page as BasePage).info.AddMessage(messaggi, LivelloMessaggio.ERROR);
                        gridDett.Visible = false;
                    }
                    else
                    {
                        // salvo ultima ricerca
                        gridDettDataBind(ret);
                        // eventi per ricerche specifiche

                    }
                }
                catch (ManagedException me)
                {
                    ((BasePage)this.Page).info.AddMessage(me.Message, LivelloMessaggio.ERROR);
                }
                catch (System.Exception ex)
                {
                   _log.Error(ex.Message);
                    ((BasePage)this.Page).info.AddMessage(ex.ToString(), LivelloMessaggio.ERROR);
                }
            }

        }


        public static String FormatDataAMGToGMA(string dataInput, string divisore)
        {

            string ret = dataInput;

            if (dataInput.Length == 8)
            {
                string yy = dataInput.Substring(0, 4);
                string mm = dataInput.Substring(4, 2);
                string gg = dataInput.Substring(6, 2);

                string data = gg + divisore + mm + divisore + yy;
                if (IsDate(data) || ret == "00000000" || mm == "00" || gg == "00")
                    ret = data;
            }

            return ret;

        }

        public static bool IsDate(Object obj)
        {
            //Ale: se obj è null (la data non rispetta l'espressione regolare) fa il botto!
            //spostata nel try con aggiunta if!=null
            //string strDate = obj.ToString();
            try
            {
                if (obj != null)
                {
                    string strDate = obj.ToString();
                    IFormatProvider culture = new CultureInfo("it-IT", true);

                    DateTime dt = DateTime.Parse(strDate, culture);
                    if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                        return true;
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        void gridDettDataBind(NCRIRICIND ds)
        {
            if (ds == null)
                return;
            NCRIRICIND.ElencoRow er;
            er = (NCRIRICIND.ElencoRow)ds.Elenco.Rows[0];
            gridDett.DataSource = ds.PersonaElenco;
            gridDett.DataKeyNames = new string[] { "CodiceFiscale", "CodiceIndiv" };
            gridDett.DataBind();
            if (gridDett.Rows.Count > 0)
            {
                gridDett.Visible = true;
                PnlRisultati.Visible = true;
                gridDett.BottomPagerRow.Visible = false;
                ((UCPaging)gridDett.BottomPagerRow.Controls[0].Controls[1]).configureControl(er.da, "5", er.totale);

            }
            else
            {
                ((BasePage)this.Page).info.AddMessage("Nessun elemento trovato!", LivelloMessaggio.INFO);
            }
            gridDett.Columns[(int)col.BottoneVisualizza].Visible = true;

        }

        private string getCellValue(int riga, int colonna)
        {
            string s = this.gridDett.Rows[riga].Cells[colonna].Text;
            if (s != "&nbsp;") return s;
            else return "";
        }


        private string getCellControlValue(int ARowIndex, int AColumnIndex)
        {
            string sResult = string.Empty;

            if ((ARowIndex > -1) && (ARowIndex < gridDett.Rows.Count) && (AColumnIndex > -1) && (AColumnIndex < gridDett.Columns.Count))
            {
                TableCell tc = gridDett.Rows[ARowIndex].Cells[AColumnIndex];

                Label lbl = null;
                if (tc.Controls.Count > 1)
                    lbl = tc.Controls[1] as Label;

                if (lbl != null)
                    sResult = lbl.Text;
                else
                    sResult = tc.Text;

                if (sResult == "&nbsp;")
                    sResult = string.Empty;
            }

            return sResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDettaglioRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int riga = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Visualizza": OnSelectIndividuo(riga, false); break;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="riga"></param>
        /// <param name="modifica"></param>
        void OnSelectIndividuo(int riga, bool modifica)
        {
            NCRIRICIND.PersonaElencoRow pRow = BusGestioneRicerche.PersonaElenco(string.Empty,
                                                                                                               string.Empty,
                                                                                                               getCellControlValue(riga, (int)col.CodiceIndividuale),
                                                                                                               getCellValue(riga, (int)col.SessoPersona),
                                                                                                               getCellValue(riga, (int)col.CognomePersona),
                                                                                                               getCellValue(riga, (int)col.NomePersona),
                                                                                                               getCellControlValue(riga, (int)col.DataDiNascitaPersona),
                                                                                                               "",
                                                                                                               getCellValue(riga, (int)col.Descrizione),
                                                                                                               getCellValue(riga, (int)col.CodiceFiscale)).PersonaElenco[0];
            SelectIndividuo(pRow, modifica);
        }



    }
}