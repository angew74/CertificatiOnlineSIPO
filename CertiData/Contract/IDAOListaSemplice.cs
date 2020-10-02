using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Objects.Common;

namespace Com.Unisys.CdR.Certi.DataLayer.Contract
{
    public interface IDAOListaSemplice
    {
        /// Restituisce un datatable con una lista dei certificati attivi presenti in base dati e le informazioni 
        /// generali che le riguardano.
        /// </summary>
        /// <returns><c>DataTable</c> lista applicazioni</returns>
        ProfiloCertificato.CertificatoDataTable LoadCertificatiAttivi(int clientID);

        /// Restituisce un datatable con una lista dei certificati attivi presenti in base dati e le informazioni 
        /// generali che le riguardano.
        /// </summary>
        /// <returns><c>DataTable</c> lista applicazioni</returns>
        ProfiloCertificato.CertificatoDataTable LoadCertificatiAttivi();

        /// Restituisce un datatable con una lista completa dei certificati presenti in base dati e le informazioni 
        /// generali che le riguardano.
        /// </summary>
        /// <returns><c>DataTable</c> lista applicazioni</returns>
        ProfiloCertificato.CertificatoDataTable LoadCertificatiCompleta(int clientID);

        /// Restituisce un datatable con una lista completa dei certificati presenti in base dati e le informazioni 
        /// generali che le riguardano.
        /// </summary>
        /// <returns><c>DataTable</c> lista applicazioni</returns>
        ProfiloCertificato.CertificatoDataTable LoadCertificatiCompleta();

        /// <summary>
        /// Restituisce un datatable con la lista dei certificati richiesti dal codice fiscale dato
        /// pronti per download
        /// </summary>
        /// <param name="cf"></param>
        /// <returns>DataTable</returns>
        ProfiloDownload.CertificatiDataTable LoadCertificati4Download(string cf, int tipoRitiro, int clientID);


        /// Restituisce un datatable con una lista delle motivazioni di esenzione dal bollo 
        /// 
        /// </summary>
        /// <returns><c>DataTable</c> lista applicazioni</returns>
        ProfiloMotivazione.MotivazioneDataTable LoadMotivazioni();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ProfiloTipoUso.TipoUsoDataTable LoadTipiUso();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        ProfiloClient.ClientsDataTable LoadClients();

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciu"></param>
        /// <returns></returns>
        ProfiloDownload.CertificatiDataTable LoadCertificato4DownloadByCIU(string ciu);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ProfiloDiciturePagamenti.DiciturePagamentiDataTable LoadDiciturePagamenti();
    }
}
