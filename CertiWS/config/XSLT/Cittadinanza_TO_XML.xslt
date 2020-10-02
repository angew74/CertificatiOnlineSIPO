<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
   <!-- <RispostaCertificato xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">-->
    <request>
      <xsl:if test="//OpenTI/MSGERR != ''">
        <Messaggio>
          <xsl:value-of select="//OpenTI/MSGERR"/>
        </Messaggio>
      </xsl:if>
      <xsl:for-each select="//OpenTI/INDIVIDUO">
        <indice>
          <tipo>CITTADINANZA ITALIANA</tipo>
          <txtTipo>CERTIFICATO di</txtTipo>
          <luogoEmissione>Roma</luogoEmissione>
          <ufficioEmittente></ufficioEmittente>
          <ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
          <azione>Visti gli atti d'ufficio</azione>
          <firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
          <nomeFirmatario>Salvatore Buccola</nomeFirmatario>
          <ufficioRichiesta>Portale Comunale</ufficioRichiesta>
          <txtCertifica>Certifica che</txtCertifica>
          <!--  <txtUrl>E' possibile recuperare il certificato all'indirizzo</txtUrl>-->
          <txtUrl>E' possibile verificare il contenuto del certificato attraverso il tool presente al seguente indirizzo </txtUrl>
          <!--   <url>https://www.comune.roma.it/servizi/certificati/recupero</url>-->
          <url>https://www.comune.roma.it/web/it/scheda-servizi/verifica-validita.page</url>
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
          <!-- <statoCivile>
            <xsl:value-of select="./STATCIV"/>
          </statoCivile>-->
        </generalita>
        <!-- <xsl:if test="./NOMEC != ''">
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
              <xsl:when test="./STATCIV = 'coniugato' or ./STATCIV = 'coniugata'">
                <txtConiuge>
                  <xsl:text>con</xsl:text>
                </txtConiuge>
              </xsl:when>
              <xsl:when test="./STATCIV = 'vedovo' or ./STATCIV = 'vedova'">
                <txtConiuge>
                  <xsl:text>di</xsl:text>
                </txtConiuge>
              </xsl:when>
            </xsl:choose>
          </coniuge>
        </xsl:if>-->
        <xsl:if test="./VIA != ''">
          <indirizzo>
            <toponimo>
              <xsl:value-of select="./VIA"/>
            </toponimo>
            <numeroCivico>
              <numero>
                <xsl:value-of select="./CIV"/>
              </numero>
              <txtNumero>N.</txtNumero>
              <xsl:if test="./LET != ''">
                <lettera>
                  <xsl:value-of select="./LET"/>
                </lettera>
              </xsl:if>
            </numeroCivico>
            <xsl:if test="./LOT != ''">
              <lotto>
                <xsl:value-of select="./LOT"/>
              </lotto>
              <txtLotto>LT.</txtLotto>
            </xsl:if>
            <xsl:if test="./PAL != ''">
              <palazzina>
                <xsl:value-of select="./PAL"/>
              </palazzina>
              <txtPalazzina>PL.</txtPalazzina>
            </xsl:if>
            <xsl:if test="./KM != ''">
              <kilometro>
                <xsl:value-of select="./KM"/>
              </kilometro>
              <txtKilometro>KM.</txtKilometro>
            </xsl:if>
            <xsl:if test="./SCA != ''">
              <scala>
                <xsl:value-of select="./SCA"/>
              </scala>
              <txtScala>SC.</txtScala>
            </xsl:if>
            <xsl:if test="./PIA != ''">
              <piano>
                <xsl:value-of select="./PIA"/>
              </piano>
              <txtPiano>PI.</txtPiano>
            </xsl:if>
            <xsl:if test="./INT != ''">
              <interno>
                <xsl:value-of select="./INT"/>
              </interno>
              <txtInterno>IN.</txtInterno>
            </xsl:if>
          </indirizzo>
        </xsl:if>
        <iscrizioneAPR>
          <residenza>
            <xsl:text>E' RESIDENTE in Roma</xsl:text>
          </residenza>
        </iscrizioneAPR>
      </xsl:for-each>
    </request>
  </xsl:template>
</xsl:stylesheet>