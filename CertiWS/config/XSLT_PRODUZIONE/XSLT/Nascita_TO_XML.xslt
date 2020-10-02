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
					<tipo>NASCITA</tipo>
					<txtTipo>CERTIFICATO di</txtTipo>
					<luogoEmissione>Roma</luogoEmissione>
					<ufficioEmittente>UFFICIO DELLO STATO CIVILE</ufficioEmittente>
					<ruoloEmittente>L'UFFICIALE DELLO STATO CIVILE</ruoloEmittente>
					<azione>In base alle risultanze anagrafiche (Art. 108 comma 2 DPR 396/2000)</azione>
					<firmatario>L'UFFICIALE DI STATO CIVILE</firmatario>
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
							<xsl:text>E' NATO</xsl:text>
						</txtNascita>
					</xsl:if>
					<xsl:if test="./SEX = 'F'">
						<txtNascita>
							<xsl:text>E' NATA</xsl:text>
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
							 <comuneRegistrazione>
								<nomeComune>
									<xsl:choose>
										<xsl:when test="./COMRGNAS = ''">
											<xsl:value-of select="./COMNAS"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="./COMRGNAS"/>
										</xsl:otherwise>
									</xsl:choose>
								</nomeComune>
								<xsl:choose>
									<xsl:when test="./TIPRGNAS = 'T'">
										<txtComune>trascritto nel comune di</txtComune>
									</xsl:when>
									<xsl:otherwise>
										<txtComune>del comune di</txtComune>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:if test="./COMRGNAS != ''">
									<siglaProvincia>
										<xsl:value-of select="./PRCRGNAS"/>
									</siglaProvincia>
								</xsl:if>
								<xsl:if test="./COMRGNAS = ''">
									<siglaProvincia>
										<xsl:value-of select="./PRCNAS"/>
									</siglaProvincia>
								</xsl:if>
							</comuneRegistrazione>
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
					 </generalita>
			</xsl:for-each>
		</RispostaCertificato>
	</xsl:template>
</xsl:stylesheet>