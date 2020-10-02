//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice � stato generato da uno strumento.
//     Versione runtime:2.0.50727.3053
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------
// 
// This source code was auto-generated by wsdl, Version=2.0.50727.42.
// 
namespace Com.Unisys.Certi.WS
{
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    using Com.Unisys.CdR.Certi.WS.Dati;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ComuneCertificatiBinding", Namespace = "http://www.comune.roma.it/certificati")]
    public interface ICertificatiBinding
    {
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comune.roma.it/certificati/ricercaPersona", RequestNamespace = "http://www.comune.roma.it/certificati/data", ResponseNamespace = "http://www.comune.roma.it/certificati/data", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("persona", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        string ricercaPersona([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] int idClient, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string codiceFiscale);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comune.roma.it/certificati/ricercaComponentiFamiglia", RequestNamespace = "http://www.comune.roma.it/certificati/data", ResponseNamespace = "http://www.comune.roma.it/certificati/data", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("componentiFamiglia", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("componente", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        ComponenteFamigliaType[] ricercaComponentiFamiglia([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] int idClient, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string codiceFiscaleRichiedente);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comune.roma.it/certificati/recuperaDocumento", RequestNamespace = "http://www.comune.roma.it/certificati/data", ResponseNamespace = "http://www.comune.roma.it/certificati/data", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        byte[][] recuperaDocumento([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] int idClient, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string codiceFiscale, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string CIU);


        ///// <remarks/>
        //[System.Web.Services.WebMethodAttribute()]
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comune.roma.it/certificati/richiestaCredenziali", RequestNamespace = "http://www.comune.roma.it/certificati/data", ResponseNamespace = "http://www.comune.roma.it/certificati/data", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[return: System.Xml.Serialization.XmlElementAttribute("intestatarioTrovato", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        //bool richiestaCredenziali([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] TransactionRequestType transactionRequest, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] out CredenzialiType credenziali);


        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comune.roma.it/certificati/richiestaCredenziali", RequestNamespace = "http://www.comune.roma.it/certificati/data", ResponseNamespace = "http://www.comune.roma.it/certificati/data", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("credenziali", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        CredenzialiType richiestaCredenziali([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] TransactionRequestType transactionRequest, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] out bool intestatarioTrovato);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comune.roma.it/certificati/verificaEmettibilita", RequestNamespace = "http://www.comune.roma.it/certificati/data", ResponseNamespace = "http://www.comune.roma.it/certificati/data", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("certificati", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("certificato", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        InfoCertificatoType[] verificaEmettibilita([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] CredenzialiType credenziali, [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] [System.Xml.Serialization.XmlArrayItemAttribute("certificato", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)] InfoCertificatoType[] certificati);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comune.roma.it/certificati/richiestaCertificati", RequestNamespace = "http://www.comune.roma.it/certificati/data", ResponseNamespace = "http://www.comune.roma.it/certificati/data", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayAttribute("certificati", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("certificato", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        CertificatoType[] richiestaCertificati([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] CredenzialiType credenziali, [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] [System.Xml.Serialization.XmlArrayItemAttribute("certificato", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)] InfoCertificatoType[] certificati);

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comune.roma.it/certificati/confermaStampa", RequestNamespace = "http://www.comune.roma.it/certificati/data", ResponseNamespace = "http://www.comune.roma.it/certificati/data", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("esitoConferma", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        bool confermaStampa([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] CredenzialiType credenziali, [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] [System.Xml.Serialization.XmlArrayItemAttribute("idDocumento", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)] string[] certificati);
    }
}
