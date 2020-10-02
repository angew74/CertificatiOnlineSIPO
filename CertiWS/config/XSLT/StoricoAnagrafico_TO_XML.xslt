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
      <indice>
        <tipo>STORICO ANAGRAFICO</tipo>
        <txtTipo>CERTIFICATO</txtTipo>
        <luogoEmissione>Roma</luogoEmissione>
        <ufficioEmittente></ufficioEmittente>
        <ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
        <azione>in base alle risultanze anagrafiche</azione>
        <firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
        <nomeFirmatario>Salvatore Buccola</nomeFirmatario>
        <ufficioRichiesta>Portale Comunale</ufficioRichiesta>
        <txtCertifica>Certifica che</txtCertifica>
        <txtVariazioniAnagrafiche>HA AVUTO LE SEGUENTI VARIAZIONI ANAGRAFICHE:</txtVariazioniAnagrafiche>       
        <txtSegue>segue storico anagrafico di</txtSegue>
        <!--  <txtUrl>E' possibile recuperare il certificato all'indirizzo</txtUrl>-->
        <txtUrl>E' possibile verificare il contenuto del certificato attraverso il tool presente al seguente indirizzo </txtUrl>
        <!--   <url>https://www.comune.roma.it/servizi/certificati/recupero</url>-->
        <url>https://www.comune.roma.it/web/it/scheda-servizi/verifica-validita.page</url>
        <validitaCertificato>Questo certificato ha valore legale per 6 mesi a partire dalla data di emissione</validitaCertificato>
      </indice>
      <generalita>
        <nome>
          <xsl:value-of select="//NOME"/>
        </nome>
        <cognome>
          <xsl:value-of select="//COGN"/>
        </cognome>
        <sesso>
          <xsl:value-of select="//SEX"/>
        </sesso>
        <xsl:if test="//SEX = 'M'">
          <txtNascita>
            <xsl:text>E' NATO</xsl:text>
          </txtNascita>
        </xsl:if>
        <txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale>
        <codiceFiscale>
          <xsl:value-of select="//CODFIS"/>
        </codiceFiscale>
        <xsl:if test="//SEX = 'F'">
          <txtNascita>
            <xsl:text>E' NATA</xsl:text>
          </txtNascita>
        </xsl:if>
        <dataNascita>
          <xsl:value-of select="//GNAS"/>/<xsl:value-of select="//MNAS"/>/<xsl:value-of select="//ANAS"/>
        </dataNascita>
        <txtDataNascita>il</txtDataNascita>
        <luogoNascita>
          <nomeComune>
            <xsl:value-of select="//COMNAS"/>
          </nomeComune>
          <siglaProvincia>
            <xsl:value-of select="//PRCNAS"/>
          </siglaProvincia>
          <xsl:if test="//DNAZCNAS = 'ITALIA'">
            <nomeStato></nomeStato>
          </xsl:if>
          <xsl:if test="//DNAZCNAS != 'ITALIA'">
            <nomeStato>
              <xsl:value-of select="//DNAZCNAS"/>
            </nomeStato>
          </xsl:if>
          <xsl:if test="//NAZCPRO != 'I'">
            <siglaStatoProvenienza>
              <xsl:value-of select="//NAZCPRO"/>
            </siglaStatoProvenienza>
          </xsl:if>
          <txtComune>a</txtComune>
        </luogoNascita>
        <StatoCivile>
          <xsl:value-of select="//STATCIV"/>
        </StatoCivile>
        <xsl:if test="//GRES = //GNAS  
						  and  //MRES = //MNAS 
						  and  //ANAS = //ARES">
          <txtIscritto>residente dalla nascita</txtIscritto>
          <dataIscrizione></dataIscrizione>
          <comuneProvenienza></comuneProvenienza>
          <provinciaProvenienza></provinciaProvenienza>
          <statoProvenienza></statoProvenienza>
          <txtProveniente></txtProveniente>
        </xsl:if>
        <xsl:if test="//GRES != //GNAS or //MRES != //MNAS or  //ANAS != //ARES">
          <txtIscritto>iscritto dal</txtIscritto>
          <dataIscrizione>
            <xsl:value-of select="//GRES"/>/<xsl:value-of select="//MRES"/>/<xsl:value-of select="//ARES"/>
          </dataIscrizione>
          <txtProveniente>proveniente da</txtProveniente>
          <xsl:if test="//COMPRO != ''">
            <comuneProvenienza>
              <xsl:value-of select="//COMPRO"/>
            </comuneProvenienza>
            <provinciaProvenienza>
              <xsl:value-of select="//PRCPRO"/>
            </provinciaProvenienza>
            <xsl:if test="//DNAZCPRO = 'ITALIA'">
              <statoProvenienza></statoProvenienza>
            </xsl:if>
            <xsl:if test="//DNAZCPRO != 'ITALIA'">
              <statoProvenienza>
                <xsl:value-of select="//DNAZCPRO"/>
              </statoProvenienza>
            </xsl:if>
          </xsl:if>
          <xsl:if test="//COMPRO = ''">
            <comuneProvenienza></comuneProvenienza>
            <provinciaProvenienza></provinciaProvenienza>
            <statoProvenienza></statoProvenienza>
          </xsl:if>
        </xsl:if>
      </generalita>
      <xsl:for-each select="//VARIAZIONE">
        <Variazione>
          <DataVariazione>
            dal&#160;<xsl:value-of select="./GVAR"/>/<xsl:value-of select="./MVAR"/>/<xsl:value-of select="./AVAR"/>
          </DataVariazione>
          <LuogoVariazione>
            <xsl:value-of select="./EVENTO"/>
          </LuogoVariazione>
        </Variazione>
      </xsl:for-each>
    </request>
  </xsl:template>
</xsl:stylesheet>