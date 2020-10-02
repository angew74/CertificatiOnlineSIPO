using System;
using System.Xml.Serialization;

namespace Com.Unisys.CdR.Certi.WS.Dati
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.comune.roma.it/certificati/data")]
    public partial class TransactionRequestType
    {

        private string codiceFiscaleIntestatarioField;

        private string codiceFiscaleRichiedenteField;

        private string idTransazioneField;

        private string nomeIntestarioField;

        private string cognomeIntestarioField;

        private string idPodField;

        private string sistemaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codiceFiscaleIntestatario
        {
            get
            {
                return this.codiceFiscaleIntestatarioField;
            }
            set
            {
                this.codiceFiscaleIntestatarioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cognomeIntestatario
        {
            get
            {
                return this.cognomeIntestarioField;
            }
            set
            {
                this.cognomeIntestarioField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nomeIntestatario
        {
            get
            {
                return this.nomeIntestarioField;
            }
            set
            {
                this.nomeIntestarioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codiceFiscaleRichiedente
        {
            get
            {
                return this.codiceFiscaleRichiedenteField;
            }
            set
            {
                this.codiceFiscaleRichiedenteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idTransazione
        {
            get
            {
                return this.idTransazioneField;
            }
            set
            {
                this.idTransazioneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idPod
        {
            get
            {
                return this.idPodField;
            }
            set
            {
                this.idPodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sistema
        {
            get
            {
                return this.sistemaField;
            }
            set
            {
                this.sistemaField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CertificatoType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.comune.roma.it/certificati/data")]
    public partial class InfoCertificatoType
    {

        private string idNomeCertificatoField;

        private string idUsoField;

        private string idMotivoEsenzioneField;

        private bool emettibileField;

        private bool emettibileFieldSpecified;

        private string dicituraVariabileField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idNomeCertificato
        {
            get
            {
                return this.idNomeCertificatoField;
            }
            set
            {
                this.idNomeCertificatoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IdUso
        {
            get
            {
                return this.idUsoField;
            }
            set
            {
                this.idUsoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idMotivoEsenzione
        {
            get
            {
                return this.idMotivoEsenzioneField;
            }
            set
            {
                this.idMotivoEsenzioneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool emettibile
        {
            get
            {
                return this.emettibileField;
            }
            set
            {
                this.emettibileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool emettibileSpecified
        {
            get
            {
                return this.emettibileFieldSpecified;
            }
            set
            {
                this.emettibileFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dicituraVariabile
        {
            get
            {
                return this.dicituraVariabileField;
            }
            set
            {
                this.dicituraVariabileField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.comune.roma.it/certificati/data")]
    public partial class CertificatoType : InfoCertificatoType
    {

        private string idDocumentoField;

        private byte[] binField;

        private string dicituraVariabileField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IdDocumento
        {
            get
            {
                return this.idDocumentoField;
            }
            set
            {
                this.idDocumentoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] bin
        {
            get
            {
                return this.binField;
            }
            set
            {
                this.binField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dicituraVariabile
        {
            get
            {
                return this.dicituraVariabileField;
            }
            set
            {
                this.dicituraVariabileField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.comune.roma.it/certificati/data")]
    public partial class CredenzialiType
    {

        private string idFlussoField;

        private TransactionRequestType transactionDataField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idFlusso
        {
            get
            {
                return this.idFlussoField;
            }
            set
            {
                this.idFlussoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TransactionRequestType transactionData
        {
            get
            {
                return this.transactionDataField;
            }
            set
            {
                this.transactionDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.comune.roma.it/certificati/data")]
    public partial class ComponenteFamigliaType
    {

        private string codiceIndividualeField;
        private string codiceFiscaleField;
        private string cognomeField;
        private string nomeField;
        private string rapportoParentelaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codiceIndividuale
        {
            get
            {
                return this.codiceIndividualeField;
            }
            set
            {
                this.codiceIndividualeField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codiceFiscale
        {
            get
            {
                return this.codiceFiscaleField;
            }
            set
            {
                this.codiceFiscaleField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cognome
        {
            get
            {
                return this.cognomeField;
            }
            set
            {
                this.cognomeField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nome
        {
            get
            {
                return this.nomeField;
            }
            set
            {
                this.nomeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rapportoParentela
        {
            get
            {
                return this.rapportoParentelaField;
            }
            set
            {
                this.rapportoParentelaField = value;
            }
        }
    }
}
