﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.7.3081.0.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.capgemini.it/cdr/io")]
[System.Xml.Serialization.XmlRootAttribute("carrelloPosizioniPagamento", Namespace="http://xml.capgemini.it/cdr/io", IsNullable=false)]
public partial class carrelloPosizioniPagamentoType {
    
    private dettaglioIntermediarioPagamentoType dettaglioIntermediarioPagamentoField;
    
    private posizionePagamentoType[] posizioniPagamentoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
    public dettaglioIntermediarioPagamentoType dettaglioIntermediarioPagamento {
        get {
            return this.dettaglioIntermediarioPagamentoField;
        }
        set {
            this.dettaglioIntermediarioPagamentoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("posizionePagamento", IsNullable=false)]
    public posizionePagamentoType[] posizioniPagamento {
        get {
            return this.posizioniPagamentoField;
        }
        set {
            this.posizioniPagamentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.capgemini.it/cdr/io")]
public partial class dettaglioIntermediarioPagamentoType {
    
    private string backURLField;

    private string codiceSistemaField;

    private string codiceOperazioneField;
    
    private anagraficaDetPosType utenteIdentificatoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
    public string backURL {
        get {
            return this.backURLField;
        }
        set {
            this.backURLField = value;
        }
    }

    [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI")]
    public string codiceSistema
    {
        get
        {
            return this.codiceSistemaField;
        }
        set
        {
            this.codiceSistemaField = value;
        }
    }

    [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI")]
    public string codiceOperazione
    {
        get
        {
            return this.codiceOperazioneField;
        }
        set
        {
            this.codiceOperazioneField = value;
        }
    }



    /// <remarks/>
    public anagraficaDetPosType utenteIdentificato {
        get {
            return this.utenteIdentificatoField;
        }
        set {
            this.utenteIdentificatoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.capgemini.it/cdr/io")]
public partial class anagraficaDetPosType {
    
    private string nomeField;
    
    private string cognomeField;
    
    private string codiceFiscaleField;
    
    private string codiceAnagraficoField;
    
    private string denominazioneField;
    
    private string partitaIVAField;
    
    private string tipoPersonaField;
    
    private string tipoSoggettoField;
    
    private recapitoDetPosType recapitoField;
    
    private string telefonoField;
    
    private string emailField;
    
    /// <remarks/>
    public string nome {
        get {
            return this.nomeField;
        }
        set {
            this.nomeField = value;
        }
    }
    
    /// <remarks/>
    public string cognome {
        get {
            return this.cognomeField;
        }
        set {
            this.cognomeField = value;
        }
    }
    
    /// <remarks/>
    public string codiceFiscale {
        get {
            return this.codiceFiscaleField;
        }
        set {
            this.codiceFiscaleField = value;
        }
    }
    
    /// <remarks/>
    public string codiceAnagrafico {
        get {
            return this.codiceAnagraficoField;
        }
        set {
            this.codiceAnagraficoField = value;
        }
    }
    
    /// <remarks/>
    public string denominazione {
        get {
            return this.denominazioneField;
        }
        set {
            this.denominazioneField = value;
        }
    }
    
    /// <remarks/>
    public string partitaIVA {
        get {
            return this.partitaIVAField;
        }
        set {
            this.partitaIVAField = value;
        }
    }
    
    /// <remarks/>
    public string tipoPersona {
        get {
            return this.tipoPersonaField;
        }
        set {
            this.tipoPersonaField = value;
        }
    }
    
    /// <remarks/>
    public string tipoSoggetto {
        get {
            return this.tipoSoggettoField;
        }
        set {
            this.tipoSoggettoField = value;
        }
    }
    
    /// <remarks/>
    public recapitoDetPosType recapito {
        get {
            return this.recapitoField;
        }
        set {
            this.recapitoField = value;
        }
    }
    
    /// <remarks/>
    public string telefono {
        get {
            return this.telefonoField;
        }
        set {
            this.telefonoField = value;
        }
    }
    
    /// <remarks/>
    public string email {
        get {
            return this.emailField;
        }
        set {
            this.emailField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.capgemini.it/cdr/io")]
public partial class recapitoDetPosType {
    
    private string indirizzoField;
    
    private string capField;
    
    private string comuneField;
    
    private string provinciaField;
    
    private string aggiuntiveEdifField;
    
    private string pressoField;
    
    /// <remarks/>
    public string indirizzo {
        get {
            return this.indirizzoField;
        }
        set {
            this.indirizzoField = value;
        }
    }
    
    /// <remarks/>
    public string cap {
        get {
            return this.capField;
        }
        set {
            this.capField = value;
        }
    }
    
    /// <remarks/>
    public string comune {
        get {
            return this.comuneField;
        }
        set {
            this.comuneField = value;
        }
    }
    
    /// <remarks/>
    public string provincia {
        get {
            return this.provinciaField;
        }
        set {
            this.provinciaField = value;
        }
    }
    
    /// <remarks/>
    public string aggiuntiveEdif {
        get {
            return this.aggiuntiveEdifField;
        }
        set {
            this.aggiuntiveEdifField = value;
        }
    }
    
    /// <remarks/>
    public string presso {
        get {
            return this.pressoField;
        }
        set {
            this.pressoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.7.3081.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://xml.capgemini.it/cdr/io")]
public partial class posizionePagamentoType {
    
    private string codicePosizioneField;
    
    private int annoRataField;
    
    private bool annoRataFieldSpecified;
    
    private int numeroRataField;
    
    private bool numeroRataFieldSpecified;
    
    private string quintocampoSIRField;
    
    private string codiceEsitoElaborazioneField;
    
    /// <remarks/>
    public string codicePosizione {
        get {
            return this.codicePosizioneField;
        }
        set {
            this.codicePosizioneField = value;
        }
    }
    
    /// <remarks/>
    public int annoRata {
        get {
            return this.annoRataField;
        }
        set {
            this.annoRataField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool annoRataSpecified {
        get {
            return this.annoRataFieldSpecified;
        }
        set {
            this.annoRataFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public int numeroRata {
        get {
            return this.numeroRataField;
        }
        set {
            this.numeroRataField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool numeroRataSpecified {
        get {
            return this.numeroRataFieldSpecified;
        }
        set {
            this.numeroRataFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public string quintocampoSIR {
        get {
            return this.quintocampoSIRField;
        }
        set {
            this.quintocampoSIRField = value;
        }
    }
    
    /// <remarks/>
    public string codiceEsitoElaborazione {
        get {
            return this.codiceEsitoElaborazioneField;
        }
        set {
            this.codiceEsitoElaborazioneField = value;
        }
    }
}
