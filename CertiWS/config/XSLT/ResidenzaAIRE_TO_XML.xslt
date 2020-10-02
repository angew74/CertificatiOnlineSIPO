<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <request>
      <xsl:if test="//OpenTI/MSGERR != ''">
        <Messaggio>
          <xsl:value-of select="//OpenTI/MSGERR"/>
        </Messaggio>
      </xsl:if>
      <xsl:for-each select="//OpenTI/INDIVIDUO">
        <indice>
          <tipo>RESIDENZA per gli ITALIANI RESIDENTI all'ESTERO</tipo>
          <txtTipo>CERTIFICATO di</txtTipo>
          <luogoEmissione>Roma</luogoEmissione>
          <ufficioEmittente></ufficioEmittente>
          <ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
          <azione>in base alle risultanze anagrafiche</azione>
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
          <!--  <xsl:choose>
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
          <statoCivile>
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
            <txtConiuge>con</txtConiuge>
          </coniuge>
        </xsl:if>-->
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
          <xsl:choose>
            <xsl:when test="./GAIRE = '' and ./GEMI = ''">
              <txtDataIscrizione>
                <xsl:text>dalla nascita</xsl:text>
              </txtDataIscrizione>
            </xsl:when>
            <xsl:otherwise>
              <txtDataIscrizione>
                <xsl:text>dal</xsl:text>
              </txtDataIscrizione>
              <xsl:if test="./GAIRE != ''">
                <dataIscrizioneAire>
                  <xsl:value-of select="./GAIRE"/>/<xsl:value-of select="./MAIRE"/>/<xsl:value-of select="./AAIRE"/>
                </dataIscrizioneAire>
              </xsl:if>
              <dataIscrizioneAire>
                <xsl:value-of select="./GEMI"/>/<xsl:value-of select="./MEMI"/>/<xsl:value-of select="./AEMI"/>
              </dataIscrizioneAire>
            </xsl:otherwise>
          </xsl:choose>
          <txtResidenzaAire>
            <xsl:text>E' RESIDENTE a</xsl:text>
          </txtResidenzaAire>
          <xsl:if test="./AIREIND != ''">
            <txtIndirizzoAire>in</txtIndirizzoAire>
            <indirizzoAire>
              <xsl:value-of select="./AIREIND"/>
            </indirizzoAire>
          </xsl:if>
          <comuneAire>
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
        </datiAire>
      </xsl:for-each>
    </request>
  </xsl:template>
</xsl:stylesheet>