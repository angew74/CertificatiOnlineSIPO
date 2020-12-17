using Com.Unisys.CdR.Certi.Caching;
using Com.Unisys.CdR.Certi.Objects.Common;
using Com.Unisys.CdR.Certi.Utils;
using Com.Unisys.CdR.Certi.WebApp.Business;
using Com.Unisys.Logging;
using Com.Unisys.Security.Provider;
using log4net;
using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS;

namespace Com.Unisys.CdR.Certi.WebApp.emissione
{
    /// <summary> 
    /// Interfaccia di gestione dell'emissione certificato
    /// Si individuano le seguenti fasi di elaborazione
    /// 0 - inizio
    /// 1 - risposta ricerca richiedente e caricamento familiari
    /// 2 - selezionato l'intestatario del certificato
    /// 3 - selezionato il certificato da richiedere
    /// 4 - selezionata la motivazione dell'esenzione da bollo
    /// 5 - conferma elaborazione richiesta
    /// </summary>
    public partial class Emissione : Com.Unisys.CdR.Certi.WebApp.common.BasePage
    {
        private static readonly ILog log = LogManager.GetLogger("Emissione");

        BUSFlussoRichiesta flu;

        /// <summary>
        /// Verifica la presenza del richiedente in sessione
        /// La prima volta vengono ricaricati i dati del richiedente dal portale comunale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fase.Value = "0";
                log.Debug("partito");
                string cf1 = Request.ServerVariables["HTTP_IV_USER"];
                string cf2 = Request.ServerVariables["iv-user"];
                log.Debug("sto verificando iv_user " + cf2);
                log.Debug("sto verificando http_iv_user " + cf1);
                Com.Unisys.Security.Provider.MyPrincipal upro = null;
                if (bool.Parse(ConfigurationManager.AppSettings["TEST"]))
                {
                    // solo per test   
                    cf1 = ConfigurationManager.AppSettings["TEST_ACCOUNT"];
                    log.Debug("sto dentro test");
                    // cf1 = (string)Session["TEST"];
                }
                if (string.IsNullOrEmpty(cf1))
                {
                    log.Debug("sto facendo logoff");
                    base.LogOff();
                }
                if (SessionManager<ProfiloUtente>.exist(SessionKeys.RICHIEDENTE_CERTIFICATI) &&
                    ((ProfiloUtente)SessionManager<ProfiloUtente>.get(SessionKeys.RICHIEDENTE_CERTIFICATI)).CodiceFiscale.ToUpper().Equals(cf1.ToUpper()))
                    richiedente = SessionManager<ProfiloUtente>.get(SessionKeys.RICHIEDENTE_CERTIFICATI);
                else
                {
                    log.Debug("vado a caricare il richiedente");
                    LoadNewRichiedente();
                    SessionManager<ProfiloUtente>.set(SessionKeys.RICHIEDENTE_CERTIFICATI, richiedente);
                }
                try
                {
                    // se arrivo qui posso fare certificati                   
                    upro = CheckUtentePA(richiedente);
                    // se upro è diverso da null e fa parte dell'applicazione certificati
                    // posso fare i certificati a tutti altrimenti solo ai componenti 
                    // della mia famiglia                  
                    if (upro != null && CheckPortale(upro.getMembership(int.Parse(ConfigurationManager.AppSettings["REFIDAPP"]))))
                    {
                        pnlRicerca.Visible = true;
                        pnlIntestatario.Visible = true;
                        fase.Value = "1";
                        btnIntestatario.Visible = false;
                    }
                    else
                    {
                        LoadListaFamiglia();
                        switch (!string.IsNullOrEmpty(richiedente.CodiceIndividuale))
                        {
                            case true:
                                SessionManager<ProfiloUtente>.set(SessionKeys.RICHIEDENTE_CERTIFICATI, richiedente);
                                fase.Value = "1";
                                break;
                            case false:
                                info.AddMessage("Il cittadino non possiede i requisiti necessari per accedere al servizio",
                                    LivelloMessaggio.WARNING);
                                break;
                        }
                    }
                }
                catch (ManagedException)
                {
                    info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                                LivelloMessaggio.WARNING);
                }
            }
            else
            {
                log.Debug("sono in postback");
                richiedente = SessionManager<ProfiloUtente>.get(SessionKeys.RICHIEDENTE_CERTIFICATI);
                if(richiedente == null)
                { 
                    LoadNewRichiedente();
                    SessionManager<ProfiloUtente>.set(SessionKeys.RICHIEDENTE_CERTIFICATI, richiedente);
                }
                switch (!(string.IsNullOrEmpty(richiedente.CodiceIndividuale)))
                {
                    case true:
                        if (fase.Value.Equals("0")) fase.Value = "1";
                        break;
                    case false:
                        if (richiedente.UtenteUM != "S")
                        {
                            info.AddMessage("Richiedente non presente in DB Anagrafe",
                                LivelloMessaggio.WARNING);
                        }
                        break;
                }
            }
            if (SessionManager<String>.exist(SessionKeys.ESITO_PAGAMENTO) && !SessionManager<String>.get(SessionKeys.ESITO_PAGAMENTO).Equals("OK"))
                info.AddMessage("Il pagamento non andato a buon fine", LivelloMessaggio.WARNING);
            SessionManager<String>.del(SessionKeys.ESITO_PAGAMENTO);

        }

        protected void OnSelectIndividuo(NCRIRICIND.PersonaElencoRow persona, bool modifica)
        {
            if (persona.CodiceFiscale != string.Empty)
            {
                cfIntestatario.Value = persona.CodiceFiscale;
                log.Debug(" ON SELECT "  + persona.CodiceFiscale);
            }
            else
            {
                cfIntestatario.Value = persona.CodiceIndiv;
                log.Debug(" ON SELECT " + persona.CodiceIndiv);
            }
            log.Debug("persona cognome " + persona.CognomePersona);
            log.Debug("persona nome " + persona.NomePersona);
            log.Debug("persona codice fiscale " + persona.CodiceFiscale);
            cognomeIntestatario.Value = persona.CognomePersona;
            nomeIntestatario.Value = persona.NomePersona;
            LoadIntestatarioAvvo(persona);
            if (!(LoadListaFamiglia(persona)))
            {
                LoadCertificatiAttiviAvvocati();
            }
            else
            {
                LoadCertificatiAttivi();
            }
        }

        private void LoadIntestatarioAvvo(NCRIRICIND.PersonaElencoRow persona)
        {
            flu = new BUSFlussoRichiesta();
            string idRic = string.Empty;
            try
            {

                if (richiedente.UtenteUM == "S" && string.IsNullOrEmpty(persona.CodiceFiscale))
                {
                    // N.R. 09/2020 NON DOVREBBE ESSERE TOCCATO
                    idRic = flu.ExecuteInizializza(ConfigurationManager.AppSettings["ClientID"], richiedente.CodiceFiscale,
                       persona.CodiceIndiv, richiedente.IndirizzoIP);
                    log.Debug("id richiesta " + idRic);

                }
                else
                {
                    // N.R. 09/2020 NON DOVREBBE ESSERE TOCCATO
                    idRic = flu.ExecuteInizializza(ConfigurationManager.AppSettings["ClientID"], richiedente.CodiceFiscale,
                       persona.CodiceFiscale, richiedente.IndirizzoIP);
                    log.Debug("id richiesta " + idRic);

                }
                if (!String.IsNullOrEmpty(idRic))
                {
                    idRichiesta.Value = idRic;
                    log.Debug("PERSONA " + persona.NomePersona);
                    lblNomeIntestatario.Text = persona.NomePersona + " ";
                    lblCognomeIntestatario.Text = persona.CognomePersona;
                    lblCodiceFiscaleIntestatario.Text = " (" + persona.CodiceFiscale + ")";
                    fase.Value = "2";
                    pnlRicerca.Visible = false;
                }
                else
                {
                    idRichiesta.Value = String.Empty;
                    info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                        LivelloMessaggio.WARNING);
                }
            }
            catch (ManagedException)
            {
                info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                            LivelloMessaggio.WARNING);
            }
        }

        private void LoadCertificatiAttiviAvvocati()
        {
            MyPrincipal upro = (MyPrincipal)Cache.Get(richiedente.CodiceFiscale);
            List<IGroupIdentity> list = (List<IGroupIdentity>)upro.getMembership(int.Parse(ConfigurationManager.AppSettings["REFIDAPP"]));
            IGroupIdentity group = list[0];
            List<AccessRight> rights = (List<AccessRight>)MySecurityProvider.GetRights(group.Id.ToString());
            ProfiloCertificato.CertificatoDataTable dt = (ProfiloCertificato.CertificatoDataTable)CacheManager<ProfiloCertificato.CertificatoDataTable>.get(
                CacheKeys.CERTIFICATI_ATTIVI_AVVOCATI, VincoloType.NONE);
            ProfiloCertificato.CertificatoDataTable newdt = new ProfiloCertificato.CertificatoDataTable();
            string[] campi = new string[2] { "public_id", "nome" };
            for (int i = 0; i < rights.Count; i++)
            {

                AccessRight right = rights[i];
                ProfiloCertificato.CertificatoRow[] rows = (ProfiloCertificato.CertificatoRow[])dt.Select("ALIAS = '" + right.IdPolicy + "'");
                if (rows != null && rows.Length > 0)
                {

                    foreach (ProfiloCertificato.CertificatoRow certrow in rows)
                    {

                        ProfiloCertificato.CertificatoRow row = (ProfiloCertificato.CertificatoRow)newdt.NewRow();
                        row["public_id"] = certrow.PUBLIC_ID;
                        row["nome"] = certrow.NOME;
                        row["certid"] = certrow.CERTID;
                        row["backend_id"] = certrow.BACKEND_ID;
                        row["tipo_uso_id"] = certrow.TIPO_USO_ID;
                        row["client_id"] = certrow.CLIENT_ID;
                        row["alias"] = certrow.ALIAS;
                        row["attivo"] = certrow.ATTIVO;
                        newdt.Rows.Add(row);
                    }
                }
            }
            rptCertificati.DataSource = newdt.DefaultView.ToTable(true, campi);
            rptCertificati.DataBind();

        }


        private bool CheckPortale(IList<IGroupIdentity> iList)
        {
            if (iList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private MyPrincipal CheckUtentePA(ProfiloUtente richiedente)
        {
            Com.Unisys.Security.Provider.MyPrincipal upro = null;
            MyIdentity identity = null;
            if (richiedente.CodiceFiscale != string.Empty)
            {
                if ((Cache.Get(richiedente.CodiceFiscale) != null))
                {

                    upro = (MyPrincipal)Cache.Get(richiedente.CodiceFiscale);
                    identity = (MyIdentity)upro.Identity;
                    richiedente.UtenteUM = "S";
                    SessionManager<ProfiloUtente>.set(SessionKeys.RICHIEDENTE_CERTIFICATI, richiedente);
                    return upro;
                }
                else
                {
                    identity = MySecurityProvider.BuildNewIdentity(richiedente.CodiceFiscale);
                    upro = MySecurityProvider.BuildPrincipal(identity, ConfigurationManager.AppSettings["REFIDAPP"]);
                    if (upro != null && upro.getMembership(int.Parse(ConfigurationManager.AppSettings["REFIDAPP"])).Count > 0)
                    {

                        Cache.Add(richiedente.CodiceFiscale, upro, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(Double.Parse(ConfigurationManager.AppSettings["CacheExp"])), System.Web.Caching.CacheItemPriority.AboveNormal, null);
                        richiedente.UtenteUM = "S";
                        SessionManager<ProfiloUtente>.set(SessionKeys.RICHIEDENTE_CERTIFICATI, richiedente);
                        return upro;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;

        }



        /// <summary>
        /// Gestisce la visualizzazione dei vari oggetti in pagina 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Page_PreRender(object sender, EventArgs e)
        {
            base.Page_PreRender(sender, e);
            Certificati myMaster = (Certificati)this.Master;
        /*    (myMaster.FindControl("hyp_CON") as HyperLink).Visible = false;
            (myMaster.FindControl("hyp_CON") as HyperLink).Enabled = false;*/

            switch (fase.Value)
            {
                case "0":
                    pnlIntestatario.Visible = false;
                    liIntestatario.Attributes.Add("style", "visibility:hidden");
                    pnlIntestatarioRo.Visible = false;                    
                    pnlCertificati.Visible = false;
                    liCertificati.Attributes.Add("style", "visibility:hidden");
                    pnlCertificatiRo.Visible = false;                   
                    pnlMotivazioni.Visible = false;
                    liMotivazioni.Attributes.Add("style", "visibility:hidden");
                    PnlMotivazioniro.Visible = false;                    
                    pnlConferma.Visible = false;                  
                    break;
                case "1":
                    if (richiedente.UtenteUM != "S")
                    {
                        pnlIntestatario.Visible = true;                     
                        liIntestatario.Attributes.Remove("style");
                        liIntestatario.Attributes.Add("class", "pagination");
                        btnIntestatario.Visible = true;                       
                        pnlRicerca.Visible = false;                       
                        pnlIntestatarioRo.Visible = false;
                        pnlCertificati.Visible = false;
                        liCertificati.Attributes.Add("style", "visibility:hidden");
                        pnlCertificatiRo.Visible = false;
                        pnlMotivazioni.Visible = false;
                        liMotivazioni.Attributes.Add("style", "visibility:hidden");
                        PnlMotivazioniro.Visible = false;
                        pnlConferma.Visible = false;                       
                    }
                    else
                    {
                        pnlRicerca.Visible = true;
                        liIntestatario.Attributes.Remove("style");
                        liIntestatario.Attributes.Add("class", "pagination");
                        pnlIntestatario.Visible = true;
                        pnlIntestatarioRo.Visible = false;
                        btnIntestatario.Visible = false;
                        pnlCertificati.Visible = false;
                        liCertificati.Attributes.Add("style", "visibility:hidden");
                        pnlCertificatiRo.Visible = false;
                        pnlMotivazioni.Visible = false;
                        liMotivazioni.Attributes.Add("style", "visibility:hidden");
                        PnlMotivazioniro.Visible = false;
                        pnlConferma.Visible = false;                       
                    }
                    break;
                case "2":
                    pnlIntestatario.Visible = false;
                    pnlIntestatarioRo.Visible = true;
                    liIntestatario.Attributes.Remove("style");
                    liIntestatario.Attributes.Add("class", "pagination");
                    for (int i = 0; i < rptCertificati.Items.Count; i++)
                    {
                        RadioButton rbi = (rptCertificati.Items[i].FindControl("rbCert")) as RadioButton;
                        CheckBox chbsi = (rptCertificati.Items[i].FindControl("chbSemp")) as CheckBox;
                        CheckBox chbbi = (rptCertificati.Items[i].FindControl("chbBoll")) as CheckBox;
                        if (!rbi.Text.Equals(idPublicCertificato.Value))
                        {
                            rbi.Checked = false;
                            chbsi.Checked = false;
                            chbsi.InputAttributes.Add("disabled", "disabled");
                            chbbi.Checked = false;
                            chbbi.InputAttributes.Add("disabled", "disabled");
                        }
                        else
                        {
                            if (idTipoUso.Value.Equals("1") || idTipoUso.Value.Equals("3"))
                            {
                                chbsi.Checked = true;
                                chbbi.Checked = false;
                            }
                            else if (idTipoUso.Value.Equals("2"))
                            {
                                chbsi.Checked = false;
                                chbbi.Checked = true;
                            }
                            else
                            {
                                chbsi.Checked = false;
                                chbbi.Checked = false;
                            }
                        }
                    }

                    pnlCertificati.Visible = true;
                    liCertificati.Attributes.Clear();
                    pnlCertificatiRo.Visible = false;
                    pnlMotivazioni.Visible = false;
                    liMotivazioni.Attributes.Add("style", "visibility:hidden");
                    PnlMotivazioniro.Visible = false;
                    pnlConferma.Visible = false;                  
                    break;
                case "3":
                    pnlIntestatario.Visible = false;
                    pnlIntestatarioRo.Visible = true;
                    liIntestatario.Attributes.Remove("style");
                    liIntestatario.Attributes.Add("class", "pagination");
                    pnlCertificati.Visible = false;
                    liCertificati.Attributes.Add("style", "visibility:hidden");
                    pnlCertificatiRo.Visible = true;
                    pnlMotivazioni.Visible = true;
                    liMotivazioni.Attributes.Clear();
                    PnlMotivazioniro.Visible = false;
                    pnlConferma.Visible = false;                   
                    break;
                case "4":
                    pnlIntestatario.Visible = false;                    
                    pnlIntestatarioRo.Visible = true;
                    liIntestatario.Attributes.Remove("style");
                    liIntestatario.Attributes.Add("class", "pagination");
                    liCertificati.Attributes.Clear();
                    liCertificati.Attributes.Add("class", "pagination");
                    pnlCertificati.Visible = false;
                    pnlCertificatiRo.Visible = true;
                    pnlMotivazioni.Visible = false;
                    PnlMotivazioniro.Visible = ((idTipoUso.Value == "1" || idTipoUso.Value == "3") ? true : false);
                    if(PnlMotivazioniro.Visible)
                    {
                        liMotivazioni.Attributes.Add("class", "pagination");
                    }
                    else
                    {
                        liMotivazioni.Attributes.Add("style", "visibility:hidden");
                    }
                  //  
                    pnlConferma.Visible = true;
                    if (idTipoUso.Value.Equals("3"))
                    { btnOn.Text = "Avanti"; }
                    else
                    { btnOn.Text = "Effettua pagamento"; }
                    break;
            }


        }

        #region  Region Buttons

        /// <summary>
        /// Conferma selezione intestatario del certificato.
        /// Se si seleziona un componente della famiglia con codice fiscale presente
        /// in DB Anagrafe, avvia un flusso di elaborazione richiesta, 
        /// invia la richiesta alle componenti interne le quali attribuiscono a questo flusso 
        /// un identificativo.
        /// Quindi vengono visualizzati i certificati disponibili da richiedere.
        /// Se l'intestatario selezionato risulta sprovvisto di codice fiscale si  visualizza
        /// un messaggio a video.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIntestatario_Click(object sender, EventArgs e)
        {

            if (rblIntestatario.SelectedIndex < 0)
            {
                info.AddMessage("Selezionare l'intestatario del certificato",
                    LivelloMessaggio.WARNING);
            }
            else if (String.IsNullOrEmpty(rblIntestatario.SelectedItem.Value))
            {
                info.AddMessage("Tra le informazioni anagrafiche dell’intestatario del certificato"
                    + " non figura un codice fiscale valido:"
                    + " non è possibile emettere il certificato anagrafico."
                    + " Si prega di rivolgersi al proprio Municipio o all’Anagrafe"
                    + " per correggere l’anomalia. Si precisa che questa condizione"
                    + " è riferita al solo codice fiscale e non indica necessariamente"
                    + " problemi inerenti l’intera posizione anagrafica.",
                    LivelloMessaggio.WARNING);
            }
            else
            {
                cfIntestatario.Value = rblIntestatario.SelectedItem.Value;
                flu = new BUSFlussoRichiesta();
                try
                {

                    // N.R. 09/2020 NON DOVREBBE ESSERE MODIFICATA
                    string idRic = flu.ExecuteInizializza(ConfigurationManager.AppSettings["ClientID"], richiedente.CodiceFiscale,
                        rblIntestatario.SelectedItem.Value, richiedente.IndirizzoIP);
                    if (!String.IsNullOrEmpty(idRic))
                    {
                        idRichiesta.Value = idRic;
                        lblNomeIntestatario.Text = rblIntestatario.SelectedItem.Text +
                            " (" + rblIntestatario.SelectedItem.Value + ")";
                        LoadCertificatiAttivi();
                        fase.Value = "2";
                    }
                    else
                    {
                        idRichiesta.Value = String.Empty;
                        info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                            LivelloMessaggio.WARNING);
                    }
                }
                catch (ManagedException)
                {
                    info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                                LivelloMessaggio.WARNING);
                }
            }
        }


        /// <summary>
        /// Gestione selezione certificato (simulazione radio button list). 
        /// Acquisizione id certificato selezionato.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbCert_CheckedChanged(object sender, EventArgs e)
        {
            idCertificato.Value = String.Empty;
            idPublicCertificato.Value = String.Empty;
            idCertificato.Value = string.Empty;
            idTipoUso.Value = string.Empty;

            for (int i = 0; i < rptCertificati.Items.Count; i++)
            {
                RadioButton rb = (rptCertificati.Items[i].FindControl("rbCert")) as RadioButton;
                if (rb.GroupName == (sender as RadioButton).GroupName)
                {
                    idPublicCertificato.Value = rb.Text;
                    ProfiloCertificato.CertificatoDataTable tb = CacheManager<ProfiloCertificato.CertificatoDataTable>.
                        get(CacheKeys.CERTIFICATI_ATTIVI, VincoloType.NONE);

                    idCertificato.Value = (tb.Select("PUBLIC_ID = '" + rb.Text + "'") as ProfiloCertificato.CertificatoRow[])[0].CERTID.ToString();

                    string filtroSempl = "PUBLIC_ID='" + rb.Text + "' and "
                        + "TIPO_USO_ID = 1";
                    string filtroBollo = "PUBLIC_ID='" + rb.Text + "' and "
                        + "TIPO_USO_ID = 2";
                    string filtroNoDirSg = "PUBLIC_ID='" + rb.Text + "' and "
                        + "TIPO_USO_ID = 3";

                    bool sempl = tb.Select(filtroSempl).Length > 0 ? true : false;
                    bool bollo = tb.Select(filtroBollo).Length > 0 ? true : false;
                    bool nodir = tb.Select(filtroNoDirSg).Length > 0 ? true : false;

                    if (sempl || nodir) (rptCertificati.Items[i].FindControl("chbSemp") as CheckBox).Enabled = true;
                    (rptCertificati.Items[i].FindControl("chbSemp") as CheckBox).Checked = nodir;
                    if (bollo) (rptCertificati.Items[i].FindControl("chbBoll") as CheckBox).Enabled = true;
                    if (nodir)
                    {
                        idTipoUso.Value = "3";
                        lblCertificato.Text ="Certificato di " +  (rptCertificati.Items[i].FindControl("certNome") as Label).Text;
                    }
                }
                else
                {
                    rb.Checked = false;
                    (rptCertificati.Items[i].FindControl("chbSemp") as CheckBox).InputAttributes.Add("disabled", "disabled");
                    (rptCertificati.Items[i].FindControl("chbSemp") as CheckBox).Checked = false;
                    (rptCertificati.Items[i].FindControl("chbBoll") as CheckBox).InputAttributes.Add("disabled", "disabled");
                    (rptCertificati.Items[i].FindControl("chbBoll") as CheckBox).Checked = false;
                }
            }
        }


        /// <summary>
        /// checkbox in carta semplice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chbSemp_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < rptCertificati.Items.Count; i++)
            {
                CheckBox chb = (rptCertificati.Items[i].FindControl("chbSemp")) as CheckBox;
                CheckBox chbb = (rptCertificati.Items[i].FindControl("chbBoll")) as CheckBox;
                if (chb == (sender as CheckBox))
                {
                    if (chb.Checked)
                    {
                        chbb.Checked = false;
                        lblCertificato.Text ="Certificato di " +  (rptCertificati.Items[i].FindControl("certNome") as Label).Text;

                        string filtro = "PUBLIC_ID='" + idPublicCertificato.Value
                            + "' and TIPO_USO_ID = 3";
                        ProfiloCertificato.CertificatoDataTable tb = CacheManager<ProfiloCertificato.CertificatoDataTable>.
                                get(CacheKeys.CERTIFICATI_ATTIVI, VincoloType.NONE);

                        idTipoUso.Value = tb.Select(filtro).Length == 0 ? "1" : "3";
                    }
                    else
                    {
                        idTipoUso.Value = String.Empty;
                    }
                }
                else
                {
                    chb.InputAttributes.Add("disabled", "disabled");
                    chb.Checked = false;
                    chbb.InputAttributes.Add("disabled", "disabled");
                    chbb.Checked = false;
                }
            }
        }


        /// <summary>
        /// checkbox in carta bollata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chbBoll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < rptCertificati.Items.Count; i++)
            {
                CheckBox chb = (rptCertificati.Items[i].FindControl("chbBoll")) as CheckBox;
                CheckBox chbs = (rptCertificati.Items[i].FindControl("chbSemp")) as CheckBox;
                if (chb == (sender as CheckBox))
                {
                    if (chb.Checked)
                    {
                        chbs.Checked = false;
                        lblCertificato.Text = "Certificato di " +  (rptCertificati.Items[i].FindControl("certNome") as Label).Text;
                        idTipoUso.Value = "2";
                    }
                    else
                    {
                        idTipoUso.Value = String.Empty;
                    }
                }
                else
                {
                    chbs.InputAttributes.Add("disabled", "disabled");
                    chbs.Checked = false;
                    chb.InputAttributes.Add("disabled", "disabled");
                    chb.Checked = false;
                }

            }
        }

        /// <summary>
        /// Conferma selezione certificato. Avvia le funzioni di verifica emettibilità 
        /// del certificato richiesto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCertiOn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(idPublicCertificato.Value))
            {
                info.AddMessage("Selezionare il certificato",
                    LivelloMessaggio.WARNING);
            }
            else if (String.IsNullOrEmpty(idTipoUso.Value))
            {
                info.AddMessage("Selezionare il tipo d'uso del certificato",
                   LivelloMessaggio.WARNING);
            }
            else
            {
                switch (idTipoUso.Value)
                {
                    case "3":
                        //   fase.Value = "4";
                        switch (idCertificato.Value)
                        {
                            case "1":
                            case "2":
                            case "3":
                                fase.Value = "4";
                                break;
                            default:
                                LoadMotivazioniAttivi();
                                fase.Value = "3";
                                break;
                        }
                        break;
                    case "1":
                        LoadMotivazioniAttivi();
                        fase.Value = "3";
                        break;
                    case "2":
                        fase.Value = "4";
                        break;
                }
            }
        }

        /// <summary>
        /// Ritorno alla scelta dell'intestatario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCertiOff_Click(object sender, EventArgs e)
        {
            idCertificato.Value = String.Empty;
            idPublicCertificato.Value = String.Empty;
            idCertificato.Value = string.Empty;
            idTipoUso.Value = String.Empty;
            fase.Value = "1";
        }

        /// <summary>
        /// Conferma selezione motivazione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMotivoOn_Click(object sender, EventArgs e)
        {
            if (rblMotivazioni.SelectedIndex == -1)
            {
                info.AddMessage("Selezionare una motivazione", LivelloMessaggio.WARNING);
            }
            else
            {
                idEsenzione.Value = rblMotivazioni.SelectedValue.ToString();
                lblMotivazioni.Text = rblMotivazioni.SelectedItem.ToString();
                fase.Value = "4";
            }
        }

        /// <summary>
        /// Ritorno alla scelta del certificato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMotivoOff_Click(object sender, EventArgs e)
        {
            lblMotivazioni.Text = string.Empty;
            // nico 2/2011
            idEsenzione.Value = string.Empty;
            fase.Value = "2";
        }


        /// <summary>
        /// Conferma richiesta emissione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOn_Click(object sender, EventArgs e)
        {
            string outMsg = string.Empty;
            Certi.WebApp.Business.ProxyWS.InfoCertificatoType[] resp = new InfoCertificatoType[1];
            flu = new BUSFlussoRichiesta();
            string form = string.Empty;
            log.Debug("btnon_click_" + Request.ServerVariables["HTTP_IV_USER"]);
            int idClient = 0;
            try
            {
                idClient = int.Parse(ConfigurationManager.AppSettings["ClientID"]);
                string publicIDClient = (CacheManager<ProfiloClient.ClientsDataTable>.
                    get(CacheKeys.CLIENTS, VincoloType.NONE).Select("ID = '" + idClient + "'") as ProfiloClient.ClientsRow[])[0].Public_ID;
                log.Debug("ExecuteVerifica_" + Request.ServerVariables["HTTP_IV_USER"]);
                // N.R. 09/2020 DA PROVARE IL NUOVO SERVIZIO
                resp = flu.ExecuteVerifica(publicIDClient, richiedente.CodiceFiscale,
                   richiedente.IndirizzoIP, cfIntestatario.Value, idRichiesta.Value, idPublicCertificato.Value,
                   idTipoUso.Value, idEsenzione.Value, lblCognomeIntestatario.Text, lblNomeIntestatario.Text);
                log.Debug("Dopo_ExecuteVerifica_" + Request.ServerVariables["HTTP_IV_USER"]);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ManagedException))
                {
                    log.Error(ex);
                }
                else
                {
                    ManagedException mex = new ManagedException(ex.Message, "ERR", "Emissione", string.Empty, ex.InnerException);
                    log.Error(mex);
                }
            }
            if (!resp[0].emettibile)
            {
                info.AddMessage("Certificato non rilasciabile per l'intestatario selezionato",
                            LivelloMessaggio.WARNING);
                return;
            }
            else if (idTipoUso.Value == "3")
            {
                fase.Value = "5";
                Response.Redirect("http://www.comune.roma.it/certificati/emissione/ritiro.aspx");

            }
            else
            {
                try
                {
                    log.Debug("Prima di GetRichiestabyId" + Request.ServerVariables["HTTP_IV_USER"]);
                    ProfiloRichiesta pr = flu.GetRichiestabyId(idRichiesta.Value);
                    log.Debug("Dopo di GetRichiestabyId" + Request.ServerVariables["HTTP_IV_USER"]);
                    // dati intestatario
                    string nomeI = pr.Richieste[0].NOME_INTESTATARIO;
                    string cognomeI = pr.Richieste[0].COGNOME_INTESTATARIO;
                    log.Debug("superato intestario");
                    // dati certificato
                    string CIU = string.Empty;
                    DateTime dataEmissione = default(DateTime);

                    ProfiloRichiesta.CertificatiRow row = pr.Certificati[0];
                    if ((int)row.TIPO_CERTIFICATO_ID == int.Parse(idCertificato.Value) &&
                        (int)row.TIPO_USO_ID == int.Parse(idTipoUso.Value) &&
                        (row.ESENZIONE_ID == idEsenzione.Value || (row.ESENZIONE_ID == null && idEsenzione.Value == string.Empty)))
                    {
                        CIU = row.CIU;
                        dataEmissione = row.DATA_EMISSIONE;
                    }

                    if (string.IsNullOrEmpty(CIU))
                    {
                        ManagedException mex = new ManagedException("Errore nel metodo di business." +
                            "Certificato non trovato.",
                            "ERR_011",
                            "Certi.WebApp.emissione.Emissione",
                            "btnOn_Click",
                            "Conferma richiesta di emissione del certificato",
                            "ClientID: " + idClient,
                            "ActiveObjectCF: " + richiedente.CodiceFiscale,
                            null);
                        Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWA", mex);
                        log.Error(error);
                        throw mex;
                    }
                    fase.Value = "5";
                    string backPage = ConfigurationManager.AppSettings["BackPage"];
                    log.Debug("dopo lettura variabili");
                    string filtro = "PUBLIC_ID='" + idPublicCertificato.Value + "' and TIPO_USO_ID =" + idTipoUso.Value;
                    ProfiloCertificato.CertificatoDataTable tb = CacheManager<ProfiloCertificato.CertificatoDataTable>.
                        get(CacheKeys.CERTIFICATI_ATTIVI, VincoloType.NONE);
                    ProfiloCertificato.CertificatoRow r = (ProfiloCertificato.CertificatoRow)(tb.Select(filtro)[0]);
                    string nomeCertificato = r.NOME;
                    bool inBollo = (idTipoUso.Value == "2") ? true : false;
                    bool esitoPrePago = false;
                    BusFlussoPagamento flussoPagamento = new BusFlussoPagamento();
                    // modifica NR 15/05/2020           
                    string juv = "";
                    esitoPrePago = flussoPagamento.invioPosizioneCreditoria(richiedente.CodiceFiscale, "1", idRichiesta.Value, backPage, CIU, out juv, out outMsg);
                    row.CODICE_PAGAMENTO = juv;
                    if (esitoPrePago)
                    {
                        if (flu.SignRichiestaPagamento(CIU, juv) == 0)
                        {
                            ManagedException mex = new ManagedException("Errore nel metodo di business." +
                                "Errore aggiornamento status " + Status.C_RICHIESTA_PAGAMENTO.ToString(),
                                "ERR_011",
                                "Certi.WebApp.emissione.Emissione",
                                "btnOn_Click",
                                "Conferma richiesta di emissione del certificato",
                                "ClientID: " + idClient,
                                "ActiveObjectCF: " + richiedente.CodiceFiscale,
                                null);
                            Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWA", mex);
                            log.Error(error);
                            throw mex;
                        }
                    }
                    else
                    {
                        info.AddMessage(outMsg.ToString(),
                                    LivelloMessaggio.WARNING);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(ManagedException))
                    {
                        log.Error(ex);
                    }
                    else
                    {
                        ManagedException mex = new ManagedException(ex.Message, "ERR", "Emissione", string.Empty, ex.InnerException);
                        log.Error(mex);
                    }

                    info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                                    LivelloMessaggio.WARNING);
                    return;
                }

            }

            log.Debug("sto per fare la response redirect_" + Request.ServerVariables["HTTP_IV_USER"]);

            string url = string.Format(ConfigurationManager.AppSettings.Get("urlPagoWeb") + ConfigurationManager.AppSettings.Get("COD_APP") + "/" + ConfigurationManager.AppSettings.Get("COD_ENTE") + "/" + "ticket/" + outMsg.ToString() + "?idSistema=" + ConfigurationManager.AppSettings.Get("ID_SISTEMA") + "&bridgeMode=" + ConfigurationManager.AppSettings.Get("BRIDGE_MODE"));
            Response.Redirect(url);
            // Response.Redirect(string.Format(ConfigurationManager.AppSettings.Get("EntryPoint") + "?codEnte=" + ConfigurationManager.AppSettings.Get("enteCreditore") + "&idSistema=" + ConfigurationManager.AppSettings.Get("sistemaArea") + "&ticket={0}", outMsg.ToString()));
            log.Debug("dopo response redirect_" + Request.ServerVariables["HTTP_IV_USER"]);

        }

        /// <summary>
        /// Ritorno dalla conferma richiesta emissione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOff_Click(object sender, EventArgs e)
        {
            switch (idTipoUso.Value)
            {
                case "3":
                    //   fase.Value = "2";
                    fase.Value = "3";
                    break;
                case "1":
                    fase.Value = "3";
                    break;
                case "2":
                    fase.Value = "2";
                    break;
            }
        }

        #endregion


        /// <summary>
        /// gestione checkbox in bollo non visibili per i certificati gratuiti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptCertificati_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CheckBox b1 = (CheckBox)e.Item.FindControl("chbBoll");
                b1.InputAttributes.Add("disabled", "disabled");
                CheckBox b2 = (CheckBox)e.Item.FindControl("chbSemp");
                b2.InputAttributes.Add("disabled", "disabled");

                string pp = ((DataRowView)e.Item.DataItem).Row["PUBLIC_ID"].ToString();

                //string filtro = "PUBLIC_ID='" + pp
                //    + "' and TIPO_USO_ID = 3";
                //ProfiloCertificato.CertificatoDataTable tb = CacheManager<ProfiloCertificato.CertificatoDataTable>.
                //        get(CacheKeys.CERTIFICATI_ATTIVI, VincoloType.NONE);
                //b1.Visible = tb.Select(filtro).Length > 0 ? false : true;
                if (pp == "C0001" || pp == "C0002" || pp == "C0003")
                {
                    string filtro = "PUBLIC_ID='" + pp
                       + "' and TIPO_USO_ID = 3";
                    ProfiloCertificato.CertificatoDataTable tb = CacheManager<ProfiloCertificato.CertificatoDataTable>.
                            get(CacheKeys.CERTIFICATI_ATTIVI, VincoloType.NONE);
                    b1.Visible = tb.Select(filtro).Length > 0 ? false : true;
                }

            }
        }

        #region Region metodi privati

        /// <summary>
        /// Caricamento dati del richiedente inviati dal portale comunale
        /// </summary>
        private void LoadNewRichiedente()
        {
            richiedente = new ProfiloUtente();
            string cf = Request.ServerVariables["HTTP_IV_USER"];
            string ip = Request.ServerVariables["HTTP_IV_REMOTE_ADDRESS"];
            log.Debug("cf " + cf);
            log.Debug("ip " + ip);
            if (bool.Parse(ConfigurationManager.AppSettings["TEST"]))
            {
                log.Debug(bool.Parse(ConfigurationManager.AppSettings["TEST"]));
                log.Debug("sto dentro test 2");
                cf = ConfigurationManager.AppSettings["TEST_ACCOUNT"];
                ip = ConfigurationManager.AppSettings["TEST_IP"];
            }

            if (string.IsNullOrEmpty(cf))
            {
                log.Debug("sto facendo logoff 2");
                base.LogOff();
            }
            else
            {
                log.Debug("sono dentro set codice fiscale e richiedete");
                richiedente.CodiceFiscale = cf.Trim().ToUpper();
                if (string.IsNullOrEmpty(ip))
                {
                    richiedente.IndirizzoIP = "127.0.0.1";
                }
                else { richiedente.IndirizzoIP = ip.Trim(); }
                log.Debug("caricato codice fiscale ed ip");
            }
            if (!bool.Parse(ConfigurationManager.AppSettings["TEST"]))
            {
                try
                {
                    log.Debug("carico il profilo utente " + Request.ServerVariables["HTTP_IV_USER"]);
                    richiedente.Nome = Com.Unisys.CdR.Certi.WebApp.Business.Utility.Utils.ConvertToUTF8(Request.ServerVariables["HTTP_IV_NOME"]);
                    richiedente.CodiceIndividuale = Request.ServerVariables["HTTP_IV_USER"];
                    richiedente.Cognome = Com.Unisys.CdR.Certi.WebApp.Business.Utility.Utils.ConvertToUTF8(Request.ServerVariables["HTTP_IV_COGNOME"]);
                    log.Debug(" cognome " + Request.ServerVariables["HTTP_IV_COGNOME"]);                  
                    richiedente.Sesso = Request.ServerVariables["HTTP_IV_SEX"];
                    richiedente.ComuneNascita = Com.Unisys.CdR.Certi.WebApp.Business.Utility.Utils.ConvertToUTF8(Request.ServerVariables["HTTP_IV_NASCITA_COMUNE"]);
                    log.Debug("carico il comune di nascita utente " + Request.ServerVariables["HTTP_IV_NASCITA_COMUNE"]);
                    if (!string.IsNullOrEmpty(Request.ServerVariables["HTTP_IV_NASCITA_NAZIONE"]))
                    { richiedente.NazioneNascita = Com.Unisys.CdR.Certi.WebApp.Business.Utility.Utils.ConvertToUTF8(Request.ServerVariables["HTTP_IV_NASCITA_NAZIONE"]); }
                    else { richiedente.NazioneNascita = ""; }
                    log.Debug("carico NAZIONE di nascita utente " + Request.ServerVariables["HTTP_IV_NASCITA_NAZIONE"]);
                    richiedente.ProvinciaNascita = Request.ServerVariables["HTTP_IV_NASCITA_PROV"];
                    richiedente.DataNascita = Request.ServerVariables["HTTP_IV_NASCITA_DATA"];
                    richiedente.Indirizzo = Com.Unisys.CdR.Certi.WebApp.Business.Utility.Utils.ConvertToUTF8(Request.ServerVariables["HTTP_IV_RES_VIA"]);
                    log.Debug("carico VIA RESIDENZA " + Request.ServerVariables["HTTP_IV_RES_VIA"]);
                    richiedente.Civico = Request.ServerVariables["HTTP_RES_CIVICO"];
                    richiedente.Cap = Request.ServerVariables["HTTP_IV_RES_CAP"];
                    log.Debug("carico il comune di residenza utente " + Request.ServerVariables["HTTP_IV_RES_COMUNE"]);
                    richiedente.ComuneResidenza = Com.Unisys.CdR.Certi.WebApp.Business.Utility.Utils.ConvertToUTF8(Request.ServerVariables["HTTP_IV_RES_COMUNE"]);
                    richiedente.ProvinciaResidenza = Request.ServerVariables["HTTP_IV_RES_PROV"];
                    richiedente.Email = Request.ServerVariables["HTTP_IV_EMAIL"];
                }
                catch (ManagedException em)
                {
                    info.AddMessage("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                                LivelloMessaggio.WARNING);
                    log.Error(em.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    ManagedException mex = new ManagedException("Non è stato possibile interrogare i sistemi centrali. Riprovare in un secondo momento",
                        "ERR_011",
                        "Certi.WebApp.emissione.Emissione",
                        "LoadNewRichiedente",
                        "Ricerca del richiedente",
                        "ClientID: 1",
                        "ActiveObjectCF: " + cf,
                        ex.InnerException);
                    Unisys.Logging.Errors.ErrorLog error = new Com.Unisys.Logging.Errors.ErrorLog("CWA", mex);
                    log.Error(error);
                    info.AddMessage(mex.Message, LivelloMessaggio.WARNING);
                    throw mex;
                }
            }
            else
            {
                richiedente.Nome = Com.Unisys.CdR.Certi.WebApp.Business.Utility.Utils.ConvertToUTF8("=?UTF-8?B?TklDT0zDkg==?=");
                richiedente.CodiceIndividuale = "";
                richiedente.Cognome = Com.Unisys.CdR.Certi.WebApp.Business.Utility.Utils.ConvertToUTF8("COGNOME");
                richiedente.CodiceFiscale = "RBRNCL74P16H501C";
                richiedente.Sesso = "F";
                richiedente.ProvinciaNascita = "";
                richiedente.ComuneNascita = "ROMA";
                richiedente.NazioneNascita = "ITALIA";
                richiedente.DataNascita = "08/05/1983";
                richiedente.Indirizzo = "VIA DEI CERCHI";
                richiedente.Civico = "6";
                richiedente.Cap = "00154";
                richiedente.ComuneResidenza = "ROMA";
                richiedente.ProvinciaResidenza = "";
            }

            Com.Unisys.Logging.Certi.CertiLogInfo inf = new Com.Unisys.Logging.Certi.CertiLogInfo();
            inf.logCode = "LON";
            inf.loggingAppCode = "CWA";
            inf.flussoID = "";
            inf.clientID = ConfigurationManager.AppSettings["ClientID"];
            inf.activeObjectCF = richiedente.CodiceFiscale;
            inf.activeObjectIP = richiedente.IndirizzoIP;
            inf.passiveObjectCF = "";
            log.Info(inf);
        }



        /// <summary>
        /// caricamento familiari del richiedente
        /// </summary>
        private void LoadListaFamiglia()
        {
            Certi.WebApp.Business.ProxyWS.ComponenteFamigliaType[] famiglia = null; ;
            flu = new BUSFlussoRichiesta();
            try
            {
                famiglia = flu.CallRicercaComponenti(ConfigurationManager.AppSettings["ClientID"], richiedente.CodiceFiscale,
                richiedente.IndirizzoIP);
            }
            catch (ManagedException m)
            {
                info.AddMessage("Errore nel caricamento dei componenti della famiglia. Riprovare in un secondo momento.", LivelloMessaggio.WARNING);
                log.Error(m);
                throw;
            }
            if (famiglia != null)
            {
                ProfiloFamiglia fam = new ProfiloFamiglia();
                for (int i = 0; i < famiglia.Length; i++)
                {
                    ProfiloFamiglia.ComponentiRow row = fam.Componenti.NewComponentiRow();
                    row.CodiceIndividuale = famiglia[i].codiceIndividuale;
                    row.CodiceFiscale = famiglia[i].codiceFiscale;
                    if (row.CodiceFiscale == richiedente.CodiceFiscale)
                    { richiedente.CodiceIndividuale = row.CodiceIndividuale; }
                    row.NomeIndiv = String.Concat(famiglia[i].cognome, " ", famiglia[i].nome);
                    fam.Componenti.Rows.Add(row);


                }
                DataView dw = new DataView(fam.Componenti);
                dw.Sort = "NomeIndiv";
                rblIntestatario.DataSource = dw;
                rblIntestatario.DataBind();
            }
            else
            {
                richiedente.CodiceIndividuale ="";
            }
        }

        private bool LoadListaFamiglia(NCRIRICIND.PersonaElencoRow persona)
        {
            Certi.WebApp.Business.ProxyWS.ComponenteFamigliaType[] famiglia = null; ;
            flu = new BUSFlussoRichiesta();
            try
            {              
                richiedente.CodiceIndividuale = persona.CodiceIndiv;
                famiglia = flu.GetComponenteFamiglias(richiedente.CodiceFiscale);               

            }
            catch (ManagedException)
            {
                info.AddMessage("Errore nel caricamento dei componenti della famiglia. Riprovare in un secondo momento.", LivelloMessaggio.WARNING);
                throw;
            }
            if (famiglia != null && CheckComponente(famiglia, persona.CodiceIndiv))
            {
                return true;
            }
            else
            {

                return false;
            }

        }

        private bool CheckComponente(Com.Unisys.CdR.Certi.WebApp.Business.ProxyWS.ComponenteFamigliaType[] famiglia, string codiceind)
        {
            foreach (ComponenteFamigliaType familiare in famiglia)
            {
                if (codiceind == familiare.codiceIndividuale)
                {
                    log.Debug("ok codiceind " + codiceind + " familiare " + familiare.codiceIndividuale);
                    return true;
                }
                else
                {
                    log.Debug("ko codiceind " + codiceind + " familiare " + familiare.codiceIndividuale);
                }
            }          
            return false;

        }

        /// <summary>
        /// Caricamento lista certificati disponibili
        /// </summary>
        private void LoadCertificatiAttivi()
        {
            DataView dw = new DataView(CacheManager<ProfiloCertificato.CertificatoDataTable>.get(
                CacheKeys.CERTIFICATI_ATTIVI, VincoloType.NONE));
            string[] campi = new string[2] { "public_id", "nome" };
            rptCertificati.DataSource = dw.ToTable(true, campi);
            rptCertificati.DataBind();
        }


        /// <summary>
        /// Caricamento elenco motivazioni
        /// </summary>
        private void LoadMotivazioniAttivi()
        {
            ProfiloMotivazione motivazioni = new ProfiloMotivazione();
            rblMotivazioni.DataSource = CacheManager<ProfiloMotivazione.MotivazioneDataTable>.get(
                CacheKeys.MOTIVAZIONI_ATTIVI, VincoloType.NONE);
            rblMotivazioni.DataBind();
        }

        /// <summary>
        /// Caricamento elenco motivazioni
        /// </summary>
        //private ArrayList LoadMotivazioniSelezionate()
        //{
        //    listMotivazioni.Items.Clear();
        //    ArrayList mots = new ArrayList();
        //    for (int i = 0; i < chlMotivazioni.Items.Count; i++)
        //    {
        //        if (chlMotivazioni.Items[i].Selected)
        //        {
        //            listMotivazioni.Items.Add(new ListItem(chlMotivazioni.Items[i].Text));
        //            mots.Add(int.Parse(chlMotivazioni.Items[i].Value));
        //        }
        //    }
        //    return mots;
        //}
        #endregion


    }
}