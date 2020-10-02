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
          <tipo>STATO LIBERO</tipo>
          <txtTipo>CERTIFICATO di</txtTipo>
          <luogoEmissione>Roma</luogoEmissione>
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
        <xsl:if test="./STATCIV = 'celibe' or ./STATCIV = 'nubile'">
          <matrimonio>
            <txtStatoLibero>
              <xsl:text>NON HA CONTRATTO MATRIMONIO O UNIONE CIVILE</xsl:text>
            </txtStatoLibero>
          </matrimonio>
        </xsl:if>
        <xsl:if test="./STACIV != 'celibe' or ./STATCIV != 'nubile'">
          <matrimonio>
            <txtStatoLibero>
              <xsl:text>NON HA CONTRATTO ALTRO MATRIMONIO O UNIONE CIVILE</xsl:text>
            </txtStatoLibero>
          </matrimonio>
          </xsl:if>
        <generalita>
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
          <txtDataNascita>il</txtDataNascita>
          <luogoNascita>
            <nomeComune>
              <xsl:value-of select="./COMNAS"/>
            </nomeComune>
            <siglaProvincia>
              <xsl:value-of select="./PRCNAS"/>
            </siglaProvincia>
            <xsl:if test="./DNAZCNAS = 'ITALIA'">
              <nomeStato></nomeStato>
            </xsl:if>
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
            <anno>
              <xsl:value-of select="./AANAS"/>
            </anno>
          </attoNascita>
          <txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale>
          <codiceFiscale>
            <xsl:value-of select="./CODFIS"/>
          </codiceFiscale>
          <cittadinanza>
            <xsl:value-of select="./DESCIT"/>
          </cittadinanza>
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
              <xsl:when test="./STATCIV = 'coniugato' or ./STATCIV = 'unito civilmente'">
                <txtConiuge>
                  <xsl:text>con</xsl:text>
                </txtConiuge>
              </xsl:when>
              <xsl:when test="./STATCIV = 'coniugata' or ./STATCIV = 'unita civilmente'">
                <txtConiuge>
                  <xsl:text>con</xsl:text>
                </txtConiuge>
              </xsl:when>  
              <xsl:when test="./STATCIV = 'vedovo' or ./STATCIV = 'vedovo di unione'">
                <txtConiuge>
                  <xsl:text>di</xsl:text>
                </txtConiuge>
              </xsl:when>
              <xsl:when test="./STATCIV = 'vedova' or ./STATCIV = 'vedovoa di unione'">
                <txtConiuge>
                  <xsl:text>di</xsl:text>
                </txtConiuge>
              </xsl:when>
            </xsl:choose>
          </coniuge>
        </xsl:if>
        <xsl:if test="./VIA != ''">
          <indirizzo>
            <toponimo>
              <xsl:value-of select="./VIA"/>
            </toponimo>
            <txtToponimo>abit. in</txtToponimo>
            <xsl:if test="./CIV != ''">
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
            </xsl:if>
          </indirizzo>
        </xsl:if>
        <iscrizioneAPR>
          <residenza>RESIDENTE in Roma</residenza>
        </iscrizioneAPR>
      </xsl:for-each>
    </RispostaCertificato>
  </xsl:template>
</xsl:stylesheet>
