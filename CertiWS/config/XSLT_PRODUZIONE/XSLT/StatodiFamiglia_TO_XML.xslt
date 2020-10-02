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
        <tipo>STATO DI FAMIGLIA</tipo>
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
        <txtSegue>segue stato di famiglia di</txtSegue>
         <url>https://www.comune.roma.it/servizi/certificati/recupero</url>
        <validitaCertificato>Questo certificato ha valore legale per 6 mesi a partire dalla data di emissione</validitaCertificato>
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
      <xsl:for-each select="//CONTRATTO">
        <DicituraContratto>
          <testoContratto1>
            <xsl:value-of select="./CONTR1"/>
          </testoContratto1>
          <testoContratto2>
            <xsl:value-of select="./CONTR2"/>
          </testoContratto2>
          <testoContratto3>
            <xsl:value-of select="./CONTR3"/>
          </testoContratto3>
          <testoContratto4>
            <xsl:value-of select="./CONTR4"/>
          </testoContratto4>         
        </DicituraContratto>
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
            <xsl:if test="./DESCIT != 'ITALIANA'">
              <xsl:if test="./SEX = 'F'">
                <txtStraniero>STRANIERA</txtStraniero>
              </xsl:if>
              <xsl:if test="./SEX ='M'">
                <txtStraniero>STRANIERO</txtStraniero>
              </xsl:if>
            </xsl:if>
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
                <xsl:when test="./STATCIV = 'coniugato' or ./STATCIV = 'coniugata' or ./STATCIV = 'unita civilmente' or ./STATCIV = 'unito civilmente'">
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
        </componenteFamiglia>
      </xsl:for-each>
      <xsl:for-each select="//FAMIGLIA">
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
      </xsl:for-each>
    </RispostaCertificato>
  </xsl:template>
</xsl:stylesheet>