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
					<tipo>Iscrizione lista generale</tipo>
					<txtTipo>CERTIFICATO di</txtTipo>
					<luogoEmissione>Roma</luogoEmissione>
					<ufficioEmittente>SERVIZI ELETTORALI</ufficioEmittente>
					<ruoloEmittente>L'UFFICIALE DI ANAGRAFE</ruoloEmittente>
					<azione>Consultate le liste elettorali</azione>
					<firmatario>L'UFFICIALE DI ANAGRAFE</firmatario>
                			<nomeFirmatario>Virginia Proverbio</nomeFirmatario>
       				      <ufficioRichiesta>Portale Comunale</ufficioRichiesta>
					<txtCertifica>Certifica che</txtCertifica>
          <txtUrl>E' possibile recuperare il certificato all'indirizzo</txtUrl>
          <url>https://www.comune.roma.it/servizi/certificati/recupero</url>
          <validitaCertificato>Questo certificato ha valore legale per 6 mesi a partire dalla data di emissione</validitaCertificato>
        </indice>	
				<generalita>
					<xsl:for-each select="//OpenTI/INDIVIDUO">
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
						  <txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale> 
					<codiceFiscale>
							<xsl:value-of select="./CODFIS"/>
						</codiceFiscale>
						</xsl:for-each>
						<xsl:for-each select="//OpenTI/ELETTORE">
							<xsl:if test="//OpenTI/INDIVIDUO/SEX = 'F'">
				 		<listaElettoraleGenerale>è iscritta al N. <xsl:value-of select="./LGEN"/> della lista elettorale generale</listaElettoraleGenerale>
				 		</xsl:if>
				 			<xsl:if test="//OpenTI/INDIVIDUO/SEX = 'M'">
				 				<listaElettoraleGenerale>è iscritto al N. <xsl:value-of select="./LGEN"/> della lista elettorale generale</listaElettoraleGenerale>
				 			</xsl:if>
				 	</xsl:for-each>
					 </generalita>			
		</RispostaCertificato>
	</xsl:template>
</xsl:stylesheet>