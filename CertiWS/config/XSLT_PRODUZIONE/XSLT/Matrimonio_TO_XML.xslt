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
					<tipo>MATRIMONIO</tipo>
					<txtTipo>CERTIFICATO di </txtTipo>
					<luogoEmissione>Roma</luogoEmissione>
					<ufficioEmittente>UFFICIO DELLO STATO CIVILE</ufficioEmittente>
					<ruoloEmittente>L'UFFICIALE DELLO STATO CIVILE</ruoloEmittente>
					<azione>sulle risultanze dei registri di stato civile</azione>
					<firmatario>L'UFFICIALE DI STATO CIVILE</firmatario>
			    <nomeFirmatario>Virginia Proverbio</nomeFirmatario>
					<!--<ufficioRichiesta>Portale Comunale</ufficioRichiesta>-->
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
					<xsl:if test="./SEX ='F'">
						<txtNascita>nata</txtNascita>
					</xsl:if>
					<xsl:if test="./SEX ='M'">
						<txtNascita>nato</txtNascita>
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
						<siglaStato>
							<xsl:value-of select="./NAZCNAS"/>
						</siglaStato>
						<nomeStato>
							<xsl:value-of select="./DNAZCNAS"/>
						</nomeStato>
					</luogoNascita>
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
					<codiceFiscale>
						<xsl:value-of select="./CODFIS"/>
					</codiceFiscale>
					<txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale>
					<cittadinanza>
						<xsl:value-of select="./DESCIT"/>
					</cittadinanza>
					<xsl:if test="./STATCIV != ''">
						<statoCivile>
							<xsl:value-of select="./STATCIV"/>
						</statoCivile>
					</xsl:if>
				</generalita>
				<xsl:if test="./CCNG != ''">
					<coniuge>
						<cognome>
							<xsl:value-of select="./COGNC"/>
						</cognome>
						<nome>
							<xsl:value-of select="./NOMEC"/>
						</nome>
						<sesso>
							<xsl:value-of select="./SEXC"/>
						</sesso>
						<xsl:if test="./SEXC ='F'">
							<txtNascita>nata</txtNascita>
						</xsl:if>
						<xsl:if test="./SEXC ='M'">
							<txtNascita>nato</txtNascita>
						</xsl:if>
						<codiceFiscale>
							<xsl:value-of select="./CODFISC"/>
						</codiceFiscale>
						<txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale>
						<dataNascita>
							<xsl:value-of select="./GNASC"/>/<xsl:value-of select="./MNASC"/>/<xsl:value-of select="./ANASC"/>
						</dataNascita>
						<txtDataNascita>il</txtDataNascita>
						<xsl:if test="./DESCOMNASC != ''">
							<luogoNascita>
								<nomeComune>
									<xsl:value-of select="./DESCOMNASC"/>
								</nomeComune>
								<txtComune>a</txtComune>
								<siglaProvincia>
									<xsl:value-of select="./PROVNASC"/>
								</siglaProvincia>
								<siglaStato>
									<xsl:value-of select="./NAZNASC"/>
								</siglaStato>
								<nomeStato>
									<xsl:value-of select="./DESNAZNASC"/>
								</nomeStato>
							</luogoNascita>
						</xsl:if>
						<xsl:if test="./NATTNASC != ''">
							<attoNascita>
								<xsl:if test="./NATTNASC != ''">
									<numero>
										<xsl:value-of select="./NATTNASC"/>
									</numero>
									<txtNumero>atto N.</txtNumero>
								</xsl:if>
								<xsl:if test="./PATTNASC != ''">
									<parte>
										<xsl:value-of select="./PATTNASC"/>
									</parte>
									<txtParte>parte</txtParte>
								</xsl:if>
								<xsl:if test="./SATTNASC != ''">
									<serie>
										<xsl:value-of select="./SATTNASC"/>
									</serie>
									<txtSerie>serie</txtSerie>
								</xsl:if>
								<xsl:if test="./EATTNASC != ''">
									<esponente>
										<xsl:value-of select="./EATTNASC"/>
									</esponente>
								</xsl:if>
							</attoNascita>
						</xsl:if>
					</coniuge>
					<matrimonio>
						<txtMatrimonio>HANNO CONTRATTO MATRIMONIO</txtMatrimonio>
						<dataMatrimonio>
							<xsl:value-of select="./GMAT"/>/<xsl:value-of select="./MMAT"/>/<xsl:value-of select="./AMAT"/>
						</dataMatrimonio>
						<txtDataMatrimonio>il</txtDataMatrimonio>
						<txtComune>a</txtComune>
						<luogoMatrimonio>
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
							<txtComune>a</txtComune>
						</luogoMatrimonio>
						<attoMatrimonio>
							<txtAnno>dell'anno</txtAnno>
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
							<txtAnno>dell'anno</txtAnno>
							<anno>
								<xsl:value-of select="./AAMAT"/>
							</anno>
						</attoMatrimonio>
					</matrimonio>
				</xsl:if>
			</xsl:for-each>
		</RispostaCertificato>
	</xsl:template>
</xsl:stylesheet>