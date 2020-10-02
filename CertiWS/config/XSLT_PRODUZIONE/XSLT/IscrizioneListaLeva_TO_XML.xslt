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
					<tipo>ISCRIZIONE LISTA DI LEVA</tipo>
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
							<codiceFiscale>
							<xsl:value-of select="./CODFIS"/>
						</codiceFiscale>
						<txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale> 
						</generalita>
			</xsl:for-each>
				<levaMilitare>
					<txtOrdine>di questo comune al N. d'ordine</txtOrdine>
					<numeroOrdine>
						<xsl:value-of select="//OpenTI/LEVA/NLEV"/>
					</numeroOrdine>
					<esitoLeva>
						<xsl:value-of select="//OpenTI/LEVA/ESIT"/>
					</esitoLeva>
					<txtIscritto>trovasi iscritto nella lista di leva</txtIscritto>
					<txtClasse>della classe</txtClasse>
					<classe>
						<xsl:value-of select="//OpenTI/INDIVIDUO/ANAS"/>
					</classe>
				</levaMilitare>
      </RispostaCertificato>
    </xsl:template>
  </xsl:stylesheet>