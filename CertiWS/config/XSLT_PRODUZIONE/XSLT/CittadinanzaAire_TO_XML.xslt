<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <RispostaCertificato xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
      <xsl:if test="//OpenTI/MSGERR != ''">
        <Messaggio>
          <xsl:value-of select="//OpenTI/MSGERR"/>
        </Messaggio>
      </xsl:if>
      <xsl:for-each select="//OpenTI/INDIVIDUO">
        <indice>
          <tipo>CITTADINANZA ITALIANA all'epoca dell'iscrizione A.I.R.E. per gli ITALIANI RESIDENTI all'ESTERO</tipo>
          <txtTipo>CERTIFICATO di</txtTipo>
          <luogoEmissione>Roma</luogoEmissione>
          <ufficioEmittente></ufficioEmittente>
          <ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
          <azione>Visti gli atti d'ufficio</azione>
          <firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
          <nomeFirmatario>Virginia Proverbio</nomeFirmatario>
          <ufficioRichiesta>Portale Comunale</ufficioRichiesta>
          <txtCertifica>Certifica che</txtCertifica>
          <txtUrl>E' possibile recuperare il certificato all'indirizzo</txtUrl>
           <url>https://www.comune.roma.it/servizi/certificati/recupero</url>
          <validitaCertificato>Questo certificato ha valore legale per 6 mesi a partire dalla data di emissione</validitaCertificato>
        </indice>
        <generalita>
          <txtDataNascita>il</txtDataNascita>
          <nome>
            <xsl:value-of select="./NOME"/>
          </nome>
          <cognome>
            <xsl:value-of select="./COGN"/>
          </cognome>
          <sesso>
            <xsl:value-of select="./SEX"/>
          </sesso>
          <xsl:if test="./SEX = 'M'">
            <txtNascita>
              <xsl:text>nato</xsl:text>
            </txtNascita>
          </xsl:if>
          <xsl:if test="./SEX = 'F'">
            <txtNascita>
              <xsl:text>nata</xsl:text>
            </txtNascita>
          </xsl:if>
          <dataNascita>
            <xsl:value-of select="./GNAS"/>/<xsl:value-of select="./MNAS"/>/<xsl:value-of select="./ANAS"/>
          </dataNascita>
          <luogoNascita>
            <nomeComune>
              <xsl:value-of select="./COMNAS"/>
            </nomeComune>
            <siglaProvincia>
              <xsl:value-of select="./PRCNAS"/>
            </siglaProvincia>
            <xsl:if test="./DNAZCNAS != 'ITALIA'">
              <nomeStato>
                <xsl:value-of select="./DNAZCNAS"/>
              </nomeStato>
            </xsl:if>
            <txtComune>a</txtComune>
          </luogoNascita>
          <attoNascita>
            <txtNumero>atto N.</txtNumero>
            <txtParte>parte</txtParte>
            <txtSerie>serie</txtSerie>
            <numero>
              <xsl:value-of select="./NATTNAS"/>
            </numero>
            <parte>
              <xsl:value-of select="./PATTNAS"/>
            </parte>
            <serie>
              <xsl:value-of select="./SATTNAS"/>
            </serie>
            <esponente>
              <xsl:value-of select="./EATTNAS"/>
            </esponente>
          </attoNascita>
          <codiceFiscale>
            <xsl:value-of select="./CODFIS"/>
          </codiceFiscale>
          <txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale>
          <cittadinanza>
            <xsl:value-of select="./DESCIT"/>
          </cittadinanza>
          <xsl:choose>
            <xsl:when test="./SEX = 'M'">
              <statusCittadinanza>
                <xsl:text>E' CITTADINO ITALIANO</xsl:text>
              </statusCittadinanza>
            </xsl:when>
            <xsl:otherwise>
              <statusCittadinanza>
                <xsl:text>E' CITTADINA ITALIANA</xsl:text>
              </statusCittadinanza>
            </xsl:otherwise>
          </xsl:choose>
          <statoCivile>
            <xsl:value-of select="./STATCIV"/>
          </statoCivile>
        </generalita>
        <xsl:if test="./NOMEC != ''">
          <coniuge>
            <xsl:if test="./COGNC != ''">
              <cognome>
                <xsl:value-of select="./COGNC"/>
              </cognome>
            </xsl:if>
            <nome>
              <xsl:value-of select="./NOMEC"/>
            </nome>
            <xsl:choose>
              <xsl:when test="./STATCIV = 'coniugato' or ./STATCIV = 'coniugata' or ./STATCIV = 'unito civilmente' or ./STATCIV = 'unita civilmente'">
                <txtConiuge>
                  <xsl:text>con</xsl:text>
                </txtConiuge>
              </xsl:when>
              <xsl:when test="./STATCIV = 'vedovo' or ./STATCIV = 'vedova' or ./STATCIV = 'vedovo di unione' or ./STATCIV = 'vedova di unione'">
                <txtConiuge>
                  <xsl:text>di</xsl:text>
                </txtConiuge>
              </xsl:when>
            </xsl:choose>
          </coniuge>
        </xsl:if>
        <datiAire>
          <xsl:choose>
            <xsl:when test="./SEX = 'M'">
              <txtIscrizione>
                <xsl:text>iscritto all'A.I.R.E.</xsl:text>
              </txtIscrizione>
            </xsl:when>
            <xsl:otherwise>
              <txtIscrizione>
                <xsl:text>iscritta all'A.I.R.E.</xsl:text>
              </txtIscrizione>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="./GAIRE != '' and GEMI != ''">
          <txtDataIscrizione>dal</txtDataIscrizione>
            <xsl:if test="./GAIRE != ''">
            <dataIscrizioneAIRE>
              <xsl:value-of select="./GAIRE"/>/<xsl:value-of select="./MAIRE"/>/<xsl:value-of select="./AAIRE"/>
            </dataIscrizioneAIRE>
            </xsl:if>
            <dataIscrizioneAIRE>
              <xsl:value-of select="./GEMI"/>/<xsl:value-of select="./MEMI"/>/<xsl:value-of select="./AEMI"/>
            </dataIscrizioneAIRE>
          </xsl:if>
          <xsl:if test ="./PATTNAS = '2' and ./SATTNAS = 'B' and ./TIPRGNAS = 'T' and ./DNAZCNAS != 'ITALIA' and ./ANAS > '1941'">
            <txtTrascrizione>per trascrizione atto di nascita</txtTrascrizione> 
          </xsl:if>
        </datiAire>
      </xsl:for-each>
    </RispostaCertificato>
  </xsl:template>
</xsl:stylesheet>