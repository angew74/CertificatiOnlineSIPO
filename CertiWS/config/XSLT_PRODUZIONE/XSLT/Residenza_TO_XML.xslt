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
					<tipo>RESIDENZA</tipo>
					<txtTipo>CERTIFICATO di</txtTipo>
					<luogoEmissione>Roma</luogoEmissione>
					<ufficioEmittente></ufficioEmittente>
					<ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
					<azione>in base alle risultanze anagrafiche</azione>
					<firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
				   <nomeFirmatario>Virginia Proverbio</nomeFirmatario>
				  <ufficioRichiesta>Portale Comunale</ufficioRichiesta>
					<txtCertifica>Certifica che</txtCertifica>
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
							<xsl:text>nato</xsl:text>
						</txtNascita>
					</xsl:if>
					<xsl:if test="./SEX = 'F'">
						<txtNascita>
							<xsl:text>nata</xsl:text>
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
					<xsl:if test="./DESCIT != 'ITALIANA'">
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
					 </generalita>				 
				<xsl:if test="./VIA != ''">
					<indirizzo>
						<toponimo>
							<xsl:value-of select="./VIA"/>
						</toponimo>
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