using System;
using System.Collections.Generic;
using System.Text;
using Com.Unisys.CdR.Certi.Objects;
using Com.Unisys.CdR.Certi.Objects.Common;
using System.Collections;

namespace Com.Unisys.CdR.Certi.DataLayer.Contract
{
    public interface IDAORichiesta
    {
        ProfiloRichiesta LoadBaseRichiesta(int idFlusso);
        ProfiloRichiesta LoadRichiesta(int idFlusso);
        ProfiloRichiesta LoadRichiestaByTransazione(string idTransazione);
        ProfiloRichiesta LoadRichiestaByCIU(string CIU);
        int InsertNuovaRichiesta(ProfiloRichiesta pr);
        void UpdateRichiesta(ProfiloRichiesta pr);
        bool UpdateStatusRichiesta(ProfiloRichiesta.RichiesteRow[] rows);  
        void InsertCertificati(ProfiloRichiesta pr);
        void UpdateCertificati(ProfiloRichiesta.CertificatiRow[] rows);
        void UpdatePagamento(ProfiloDownload.CertificatiDataTable tb);
        void UpdatePagamentoExt(int statusId, string xmlPagamento, decimal id, string ciu);
        int UpdateCertificatoByCIU(string ciu, string descrizione, int status,string juv);
        int GetStatusCertificato(string ciu);
        byte[] ReadHandle(string id);
    }
}
