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
      <indice>
        <tipo>STATO DI FAMIGLIA PER GLI ITALIANI RESIDENTI ALL'ESTERO</tipo>
        <txtTipo>CERTIFICATO DI</txtTipo>
        <luogoEmissione>Roma</luogoEmissione>
        <ufficioEmittente></ufficioEmittente>
        <ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
        <azione>nell'Anagrafe della popolazione residente risulta iscritta la seguente famiglia di</azione>
        <firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
        <nomeFirmatario>Virginia Proverbio</nomeFirmatario>
        <ufficioRichiesta>Portale Comunale</ufficioRichiesta>
        <txtCertifica>Certifica che</txtCertifica>
        <txtUrl>E' possibile recuperare il certificato all'indirizzo</txtUrl>
         <url>https://www.comune.roma.it/servizi/certificati/recupero</url>
        <validitaCertificato>Questo certificato ha valore legale per 6 mesi a partire dalla data di emissione</validitaCertificato>
        <txtSegue>segue stato di famiglia di</txtSegue>
      </indice>
      <xsl:for-each select="//FAMIGLIA">
        <generalita>
          <nome>
            <xsl:value-of select="./NOMERIC"/>
          </nome>
          <cognome>
            <xsl:value-of select="./COGNRIC"/>
          </cognome>
        </generalita>
      </xsl:for-each>
      <!-- componenteFamiglia -->
      <xsl:for-each select="//COMPONENTE">
        <componenteFamiglia>
          <generalita>
            <xsl:if test="./CIND != ''">
              <xsl:if test="./SEX != ''">
                <sesso>
                  <xsl:value-of select="./SEX"/>
                </sesso>
              </xsl:if>
              <xsl:if test="./NOME != ''">
                <nome>
                  <xsl:value-of select="./NOME"/>
                </nome>
              </xsl:if>
              <xsl:if test="./COGN != ''">
                <cognome>
                  <xsl:value-of select="./COGN"/>
                </cognome>
              </xsl:if>
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
              <xsl:if test="./CODFIS != ''">
                <codiceFiscale>
                  <xsl:value-of select="./CODFIS"/>
                </codiceFiscale>
                <txtCodiceFiscale>
                  <xsl:text>Cod.Fis.</xsl:text>
                </txtCodiceFiscale>
              </xsl:if>
              <xsl:if test="./DESCIT != '' or ./ISTNAZ !=''">
                <cittadinanza>
                  <xsl:if test="./DESCIT !=''">
                    <descrizioneCittadinanza>
                      <xsl:value-of select="./DESCIT"/>
                    </descrizioneCittadinanza>
                  </xsl:if>
                </cittadinanza>
              </xsl:if>
              <xsl:if test="./STATCIV != ''">
                <statoCivile>
                  <xsl:value-of select="./STATCIV"/>
                </statoCivile>
              </xsl:if>
              <xsl:if test="./NATTNAS != '' or ./PATTNAS != '' or ./SATTNAS != '' or ./AANAS != '' or ./TIPRGNAS !='' or ./COMRGNAS !=''">
                <attoNascita>
                  <numero>
                    <xsl:value-of select="./NATTNAS"/>
                  </numero>
                  <txtNumero>atto N.</txtNumero>
                  <parte>
                    <xsl:value-of select="./PATTNAS"/>
                  </parte>
                  <txtParte>parte</txtParte>
                  <txtSerie>serie</txtSerie>
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
              </xsl:if>
            </xsl:if>
            <xsl:if test="./RAPPAR != ''">
              <rapportoParentela>
                <xsl:value-of select="./RAPPAR"/>
              </rapportoParentela>
            </xsl:if>
            <!--</xsl:if>-->
          </generalita>
          <xsl:if test="./CCNG != ''">
            <coniuge>
              <xsl:if test="./SEXC != ''">
                <sesso>
                  <xsl:value-of select="./SEXC"/>
                </sesso>
              </xsl:if>
              <xsl:if test="./COGNC != ''">
                <cognome>
                  <xsl:value-of select="./COGNC"/>
                </cognome>
              </xsl:if>
              <xsl:if test="./NOMEC != ''">
                <nome>
                  <xsl:value-of select="./NOMEC"/>
                </nome>
              </xsl:if>
              <xsl:choose>
                <xsl:when test="./STATCIV = 'coniugato' or ./STATCIV = 'coniugata' or ./STATCIV = 'unito civilmente' or ./STATCIV = 'unita civilmente'">
                  <txtConiuge>
                    <xsl:text>con</xsl:text>
                  </txtConiuge>
                </xsl:when>
                <xsl:when test="./STATCIV = 'vedovo' or ./STATCIV = 'vedova' or ./STATCIV = 'vedova di unione' or ./STATCIV = 'vedovo di unione'">
                  <txtConiuge>
                    <xsl:text>di</xsl:text>
                  </txtConiuge>
                </xsl:when>
              </xsl:choose>
            </coniuge>
          </xsl:if>
          <datiAire>
            <txtResidenzaAire>residente all'estero</txtResidenzaAire>
            <txtDataIscrizione>dal</txtDataIscrizione>
            <xsl:if test="../AIREIND != ''">
              <txtIndirizzoAire>in</txtIndirizzoAire>
              <indirizzoAire>
                <xsl:value-of select="../AIREIND"/>
              </indirizzoAire>
            </xsl:if>
            <comuneAire>
              <txtComune>a</txtComune>
              <nomeComune>
                <xsl:value-of select="../COMAIRE"/>
              </nomeComune>
              <xsl:if test="../PRCAIRE != ''">
                <siglaProvincia>
                  <xsl:value-of select="../PRCAIRE"/>
                </siglaProvincia>
              </xsl:if>
              <xsl:if test="../NAZCAIRE != ''">
                <siglaStato>
                  <xsl:value-of select="../NAZCAIRE"/>
                </siglaStato>
              </xsl:if>
              <xsl:if test="../DNAZCAIRE != ''">
                <nomeStato>
                  <xsl:value-of select="../DNAZCAIRE"/>
                </nomeStato>
              </xsl:if>
            </comuneAire>
            <xsl:if test="./GAIRE != ''">
              <dataIscrizioneAire>
                <xsl:value-of select="./GAIRE"/>/<xsl:value-of select="./MAIRE"/>/<xsl:value-of select="./AAIRE"/>
              </dataIscrizioneAire>
            </xsl:if>
            <dataIscrizioneAire>
              <xsl:value-of select="./GEMI"/>/<xsl:value-of select="./MEMI"/>/<xsl:value-of select="./AEMI"/>
            </dataIscrizioneAire>
          </datiAire>
        </componenteFamiglia>
      </xsl:for-each>
      <!--<xsl:for-each select="//FAMIGLIA">
            <datiAire>
              <txtResidenzaAire>residente all'estero</txtResidenzaAire>
              <txtDataIscrizione>dal</txtDataIscrizione>
              <xsl:if test="./AIREIND != ''">
                <txtIndirizzoAire>in</txtIndirizzoAire>
                <indirizzoAire>
                  <xsl:value-of select="./AIREIND"/>
                </indirizzoAire>
              </xsl:if>
              <comuneAire>
                <txtComune>a</txtComune>
                <nomeComune>
                  <xsl:value-of select="./COMAIRE"/>
                </nomeComune>
                <xsl:if test="./PRCAIRE != ''">
                  <siglaProvincia>
                    <xsl:value-of select="./PRCAIRE"/>
                  </siglaProvincia>
                </xsl:if>
                <xsl:if test="./NAZCAIRE != ''">
                  <siglaStato>
                    <xsl:value-of select="./NAZCAIRE"/>
                  </siglaStato>
                </xsl:if>
                <xsl:if test="./DNAZCAIRE != ''">
                  <nomeStato>
                    <xsl:value-of select="./DNAZCAIRE"/>
                  </nomeStato>
                </xsl:if>
              </comuneAire>
              <xsl:for-each select="//COMPONENTE">
            <dataIscrizioneAire>
              <xsl:value-of select="./GAIRE"/>/<xsl:value-of select="./MAIRE"/>/<xsl:value-of select="./AAIRE"/>
            </dataIscrizioneAire>
          </xsl:for-each>
            </datiAire>
          </xsl:for-each>-->
    </RispostaCertificato>
  </xsl:template>
</xsl:stylesheet>