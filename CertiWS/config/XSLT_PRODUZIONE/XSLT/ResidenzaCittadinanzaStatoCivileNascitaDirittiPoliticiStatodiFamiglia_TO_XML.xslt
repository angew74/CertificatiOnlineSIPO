<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <xsl:variable name="apos" select='"&apos;"'/>
    <RispostaCertificato xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
      <xsl:if test="//OpenTI/MSGERR != ''">
        <Messaggio>
          <xsl:value-of select="//OpenTI/MSGERR"/>
        </Messaggio>
      </xsl:if>
      <xsl:for-each select="//OpenTI/INDIVIDUO">
        <indice>
          <xsl:choose>
            <xsl:when test ="../ELETTORE/FELE != '1' and (./COMNAS != 'ROMA' or ./PRCNAS != 'RM') and ./FCERTNAS != '1'">
              <tipo>RESIDENZA, di CITTADINANZA ITALIANA, di STATO CIVILE di STATO DI FAMIGLIA</tipo>
            </xsl:when>
            <xsl:when test="(./COMNAS != 'ROMA' or ./PRCNAS != 'RM') and ./FCERTNAS != '1' and ../ELETTORE/FELE = '1'">
              <tipo>RESIDENZA, di CITTADINANZA ITALIANA, di STATO CIVILE di DIRITTI POLITICI, di STATO DI FAMIGLIA</tipo>
            </xsl:when>
            <xsl:when test="((./COMNAS = 'ROMA' and ./PRCNAS = 'RM') or ./FCERTNAS = '1') and ../ELETTORE/FELE != '1'">
              <tipo>RESIDENZA, di CITTADINANZA ITALIANA, di STATO CIVILE, di NASCITA, di STATO DI FAMIGLIA</tipo>
            </xsl:when>
            <xsl:otherwise>
              <tipo>RESIDENZA,di CITTADINANZA ITALIANA, di STATO CIVILE di NASCITA, di DIRITTI POLITICI, di STATO DI FAMIGLIA</tipo>
            </xsl:otherwise>
          </xsl:choose>
          <txtTipo>CERTIFICATO CONTESTUALE di</txtTipo>
          <luogoEmissione>Roma</luogoEmissione>
          <ufficioEmittente></ufficioEmittente>
          <ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
          <azione>Visti gli atti d'ufficio</azione>
          <azione2>nell'Anagrafe della popolazione residente risulta iscritta la seguente famiglia</azione2>
          <azioneArticolo>Visto l'art. 40 D.P.R.445 del 28/12/2000</azioneArticolo>
          <firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
          <nomeFirmatario>Virginia Proverbio</nomeFirmatario>
          <ufficioRichiesta>Portale Comunale</ufficioRichiesta>
          <txtCertifica>Certifica che</txtCertifica>
          <txtSegue>segue certificato contestuale di</txtSegue>
          <txtUrl>E' possibile recuperare il certificato all'indirizzo</txtUrl>
           <url>https://www.comune.roma.it/servizi/certificati/recupero</url>
          <validitaCertificato>Questo certificato ha valore legale per 6 mesi a partire dalla data di emissione</validitaCertificato>
        </indice>
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
              <xsl:text>E' nato</xsl:text>
            </txtNascita>
          </xsl:if>
          <xsl:if test="./SEX = 'F'">
            <txtNascita>
              <xsl:text>E' nata</xsl:text>
            </txtNascita>
          </xsl:if>
          <txtDataNascita>il</txtDataNascita>
          <dataNascita>
            <xsl:value-of select="./GNAS"/>/<xsl:value-of select="./MNAS"/>/<xsl:value-of select="./ANAS"/>
          </dataNascita>
          <luogoNascita>
            <txtComune>a</txtComune>
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
          </luogoNascita>
          <xsl:if test="./NATTNAS != ''">
            <attoNascita>
              <numero>
                <xsl:value-of select="./NATTNAS"/>
              </numero>
              <txtNumero>atto N.</txtNumero>
              <txtParte>parte</txtParte>
              <parte>
                <xsl:value-of select="./PATTNAS"/>
              </parte>
              <txtSerie>serie</txtSerie>
              <serie>
                <xsl:value-of select="./SATTNAS"/>
              </serie>
              <esponente>
                <xsl:value-of select="./EATTNAS"/>
              </esponente>
            </attoNascita>
          </xsl:if>
          <xsl:if test="./NATTNAS = '' or ./DESSTATCIT != 'ITALIA'">
            <xsl:if test="./SEX = 'F'">
              <txtStraniero>STRANIERA</txtStraniero>
            </xsl:if>
            <xsl:if test="./SEX ='M'">
              <txtStraniero>STRANIERO</txtStraniero>
            </xsl:if>
          </xsl:if>
          <xsl:if test="./CODFIS != ''">
            <codiceFiscale>
              <xsl:value-of select="./CODFIS"/>
            </codiceFiscale>
            <txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale>
          </xsl:if>
          <xsl:if test="./DESCIT != ''  !=''">
            <txtStraniero>
              <xsl:value-of select="./DESCIT"/>
            </txtStraniero>
          </xsl:if>
          <statoCivile>
            <xsl:value-of select="./STATCIV"/>
          </statoCivile>
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
          <xsl:if test="(./COMNAS = 'ROMA' and ./PRCNAS = 'RM') or ./FCERTNAS = '1'">
            <xsl:choose>
              <xsl:when test="./SEX = 'M'">
                <txtNascita2>
                  <xsl:text>E' NATO</xsl:text>
                </txtNascita2>
              </xsl:when>
              <xsl:when test="./SEX = 'F'">
                <txtNascita2>
                  <xsl:text>E' NATA</xsl:text>
                </txtNascita2>
              </xsl:when>
            </xsl:choose>
          </xsl:if>
          <xsl:choose>
            <xsl:when test="./STATCIV = 'celibe'">
              <txtStatoCivile>E' CELIBE</txtStatoCivile>
            </xsl:when>
            <xsl:when test="./STATCIV = 'nubile'">
              <txtStatoCivile>E' NUBILE</txtStatoCivile>
            </xsl:when>
            <xsl:when test ="./STATCIV = 'vedovo'">
              <txtStatoCivile>E' VEDOVO</txtStatoCivile>
            </xsl:when>
            <xsl:when test ="./STATCIV = 'vedova'">
              <txtStatoCivile>E' VEDOVA</txtStatoCivile>
            </xsl:when>
            <xsl:when test ="./STATCIV = 'vedovo di unione'">
              <txtStatoCivile>E' VEDOVO DI UNIONE</txtStatoCivile>
            </xsl:when>
            <xsl:when test ="./STATCIV = 'vedova di unione'">
              <txtStatoCivile>E' VEDOVA DI UNIONE</txtStatoCivile>
            </xsl:when>
            <xsl:when test ="./STATCIV = concat('gia', $apos, ' unita civilmente')">
              <txtStatoCivile>E' GIA' UNITA</txtStatoCivile>
            </xsl:when>
            <xsl:when test ="./STATCIV = concat('gia', $apos, ' unito civilmente')">
              <txtStatoCivile>E' GIA' UNITO</txtStatoCivile>
            </xsl:when>           
            <xsl:when test ="./STATCIV = 'coniugato' or ./STATCIV = 'coniugata' and ./COGNC != ''">
              <txtStatoCivile>HA CONTRATTO MATRIMONIO</txtStatoCivile>
            </xsl:when>
            <xsl:when test ="./STATCIV = 'unito civilmente' or ./STATCIV = 'unita civilmente' and ./COGNC != ''">
              <txtStatoCivile>HA CONTRATTO UNIONE CIVILE</txtStatoCivile>
            </xsl:when>
            <xsl:when test="./STATCIV = 'coniugato' and ./COGNC = ''">
              <txtStatoCivile>E' CONIUGATO</txtStatoCivile>
            </xsl:when>
            <xsl:when test="./STATCIV = 'coniugata' and ./COGNC = ''">
              <txtStatoCivile>E' CONIUGATA</txtStatoCivile>
            </xsl:when>
            <xsl:when test="./STATCIV = 'unito civilmente' and ./COGNC = ''">
              <txtStatoCivile>E' UNITO CIVILMENTE</txtStatoCivile>
            </xsl:when>
            <xsl:when test="./STATCIV = 'unita civilmente' and ./COGNC = ''">
              <txtStatoCivile>E' UNITA CIVILMENTE</txtStatoCivile>
            </xsl:when>            
            <xsl:otherwise>
              <xsl:if test="SEX = 'M'">
                <txtStatoCivile>E' GIA' CONIUGATO</txtStatoCivile>
              </xsl:if>
              <xsl:if test="SEX = 'F'">
                <txtStatoCivile>E' GIA' CONIUGATA</txtStatoCivile>
              </xsl:if>
            </xsl:otherwise>
          </xsl:choose>
        </generalita>
        <xsl:choose>
          <xsl:when test ="./STATCIV = 'vedovo'">
            <xsl:if test="./COGNC != ''">
              <coniuge>
                <cognome>
                  <xsl:value-of select="./COGNC"/>
                </cognome>
                <nome>
                  <xsl:value-of select="./NOMEC"/>
                </nome>
                <txtConiuge>di</txtConiuge>
              </coniuge>
            </xsl:if>
          </xsl:when>
          <xsl:when test ="./STATCIV = 'vedovo di unione'">
            <xsl:if test="./COGNC != ''">
              <coniuge>
                <cognome>
                  <xsl:value-of select="./COGNC"/>
                </cognome>
                <nome>
                  <xsl:value-of select="./NOMEC"/>
                </nome>
                <txtConiuge>di</txtConiuge>
              </coniuge>
            </xsl:if>
          </xsl:when>
          <xsl:when test ="./STATCIV = 'vedova'">
            <xsl:if test="./COGNC != ''">
            <coniuge>
              <cognome>
                <xsl:value-of select="./COGNC"/>
              </cognome>
              <nome>
                <xsl:value-of select="./NOMEC"/>
              </nome>
              <txtConiuge>di</txtConiuge>
            </coniuge>
            </xsl:if>
          </xsl:when>
          <xsl:when test ="./STATCIV = 'vedova di unione'">
            <xsl:if test="./COGNC != ''">
              <coniuge>
                <cognome>
                  <xsl:value-of select="./COGNC"/>
                </cognome>
                <nome>
                  <xsl:value-of select="./NOMEC"/>
                </nome>
                <txtConiuge>di</txtConiuge>
              </coniuge>
            </xsl:if>
          </xsl:when>
          <xsl:when test ="./STATCIV = 'coniugato' or ./STATCIV = 'coniugata' and ./COGNC != ''">
            <coniuge>
              <cognome>
                <xsl:value-of select="./COGNC"/>
              </cognome>
              <nome>
                <xsl:value-of select="./NOMEC"/>
              </nome>
              <txtConiuge>con</txtConiuge>
            </coniuge>
          </xsl:when>
          <xsl:when test ="./STATCIV = 'unito civilmente' or ./STATCIV = 'unita civilmente' and ./COGNC != ''">
            <coniuge>
              <cognome>
                <xsl:value-of select="./COGNC"/>
              </cognome>
              <nome>
                <xsl:value-of select="./NOMEC"/>
              </nome>
              <txtConiuge>con</txtConiuge>
            </coniuge>
          </xsl:when>
        </xsl:choose>
        <xsl:if test="./STATCIV != 'celibe' or ./STATCIV != 'nubile' or ./STATCIV != 'vedovo' or ./STATCIV != 'vedovo di unione' or ./STATCIV != 'vedova di unione' or ./STATCIV != 'vedova' or ./STATCIV != 'già coniugato' or ./STATCIV != 'già coniugata' or ./STATCIV != 'già unito' or ./STATCIV != 'già unita'">
        <matrimonio>
          <dataMatrimonio>
            <xsl:value-of select="./GMAT"/>/<xsl:value-of select="./MMAT"/>/<xsl:value-of select="./AMAT"/>
          </dataMatrimonio>
          <txtDataMatrimonio>il</txtDataMatrimonio>
          <luogoMatrimonio>
            <txtComune>a</txtComune>
            <nomeComune>
              <xsl:value-of select="./COMMAT"/>
            </nomeComune>
            <siglaProvincia>
              <xsl:value-of select="./PRCMAT"/>
            </siglaProvincia>
            <siglaStato>
              <xsl:value-of select="./NAZCMAT"/>
            </siglaStato>
            <nomeStato>
              <xsl:value-of select="./DNAZCMAT"/>
            </nomeStato>
          </luogoMatrimonio>
          <attoMatrimonio>
            <txtNumero>atto N.</txtNumero>
            <numero>
              <xsl:value-of select="./NATTMAT"/>
            </numero>
            <txtParte>parte</txtParte>
            <parte>
              <xsl:value-of select="./PATTMAT"/>
            </parte>
            <txtSerie>serie</txtSerie>
            <serie>
              <xsl:value-of select="./SATTMAT"/>
            </serie>
            <esponente>
              <xsl:value-of select="./EATTMAT"/>
            </esponente>
          </attoMatrimonio>
        </matrimonio>
        </xsl:if>
        <xsl:if test="../ELETTORE/FELE = '1'">
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
          </datiElettorali>
        </xsl:if>
        <xsl:if test="./VIA != ''">
          <indirizzo>
            <toponimo>
              <xsl:value-of select="./VIA"/>
            </toponimo>
            <!--<txtToponimo>abit. in</txtToponimo>-->
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
          <residenza>
            <xsl:text>E' RESIDENTE in Roma</xsl:text>
          </residenza>
          </iscrizioneAPR>
      </xsl:for-each>
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
                <txtComune>a</txtComune>
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
              <!-- <xsl:if test="./STATCIV != ''">
                <statoCivile>
                  <xsl:value-of select="./STATCIV"/>
                </statoCivile>
              </xsl:if>-->
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
          </xsl:if>
        </componenteFamiglia>
      </xsl:for-each>
    </RispostaCertificato>
  </xsl:template>
</xsl:stylesheet>