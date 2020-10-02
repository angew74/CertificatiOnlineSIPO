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
          <tipo>GODIMENTO DEI DIRITTI POLITICI</tipo>
          <txtTipo>CERTIFICATO di</txtTipo>
          <luogoEmissione>Roma</luogoEmissione>
          <ufficioEmittente>SERVIZIO ELETTORALE</ufficioEmittente>
          <txtComune>COMUNE DI ROMA</txtComune>
          <ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
          <azione>Consultate le liste elettorali</azione>
          <firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
          <nomeFirmatario>Salvatore Buccola</nomeFirmatario>
          <ufficioRichiesta>Portale Comunale</ufficioRichiesta>
          <txtCertifica>Certifica che</txtCertifica>
          <txtServizio>SERVIZIO ELETTORALE</txtServizio>
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
          <codiceFiscale>
            <xsl:value-of select="./CODFIS"/>
          </codiceFiscale>
          <txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale>
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
            <txtConiuge>con</txtConiuge>
          </coniuge>
        </xsl:if>
        <datiElettorali>
          <xsl:if test="./SEX = 'M'">
            <txtIscrizione>e' iscritto al N.</txtIscrizione>
          </xsl:if>
          <xsl:if test="./SEX = 'F'">
            <txtIscrizione>e' iscritta al N.</txtIscrizione>
          </xsl:if>
          <listaGenerale>
            <xsl:value-of select="../ELETTORE/LGEN"/>
          </listaGenerale>
          <txtDirittiPolitici>GODE DEI DIRITTI POLITICI</txtDirittiPolitici>
          <tesseraElettorale>
            <txtTesseraElettorale>tessera elettorale n.</txtTesseraElettorale>
            <numeroTessera>
              <xsl:value-of select="../ELETTORE/NTES"/>
            </numeroTessera>
          </tesseraElettorale>
        </datiElettorali>
      </xsl:for-each>
    </request>
  </xsl:template>
</xsl:stylesheet>