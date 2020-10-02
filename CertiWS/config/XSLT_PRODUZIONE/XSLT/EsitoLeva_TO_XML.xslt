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
					<tipo>COMPROVANTE L'ESITO AVUTO NELLA LEVA SUI GIOVANI NATI NELL'ANNO</tipo>
					<txtTipo>CERTIFICATO</txtTipo>
					<txtDa>da</txtDa>
					<luogoEmissione>Roma</luogoEmissione>
					<ufficioEmittente></ufficioEmittente>
					<ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
					<azione>Dichiara che dalle liste di leva esistenti negli archivi comunali risulta che:</azione>
					<firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
					 <nomeFirmatario>Virginia Proverbio</nomeFirmatario>
					<ufficioRichiesta>Portale Comunale</ufficioRichiesta>
					<txtCertifica></txtCertifica>
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
				<numeroOrdine>
					<xsl:value-of select="//OpenTI/LEVA/NLEV"/>
				</numeroOrdine>
				<esitoLeva>
					<xsl:value-of select="//OpenTI/LEVA/ESIT"/>
				</esitoLeva>
				<txtIscritto>ha fatto parte della Leva sui nati nell'anno</txtIscritto>
				<txtClasse>della classe</txtClasse>
				<classe>
					<xsl:value-of select="//OpenTI/INDIVIDUO/ANAS"/>
				</classe>
				<txtOrdine>al N. d'ordine</txtOrdine>
				<txtConsiglio>e fu dal Consiglio di Leva in occasione dell'esame in data </txtConsiglio>
				<dataVisita>
					<xsl:value-of select="//OpenTI/LEVA/GVIS"/>/<xsl:value-of select="//OpenTI/LEVA/MVIS"/>/<xsl:value-of select="//OpenTI/LEVA/AVIS"/>
				</dataVisita>
			</levaMilitare>
      </RispostaCertificato>
    </xsl:template>
  </xsl:stylesheet>