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
					<tipo>MORTE</tipo>
					<txtTipo>CERTIFICATO di</txtTipo>
					<luogoEmissione>Roma</luogoEmissione>
          <ufficioEmittente>UFFICIO DELLO STATO CIVILE</ufficioEmittente>
          <ruoloEmittente>L'UFFICIALE DELLO STATO CIVILE</ruoloEmittente>
          <azione>In base alle risultanze anagrafiche (Art. 108 comma 2 DPR 396/2000)</azione>
          <firmatario>L'UFFICIALE DELLO STATO CIVILE</firmatario>
          <nomeFirmatario>Virginia Proverbio</nomeFirmatario>
					<!--<ufficioRichiesta>Portale Comunale</ufficioRichiesta>-->
					<txtCertifica>Certifica che</txtCertifica>
					<txtUrl>E' possibile recuperare il certificato all'indirizzo</txtUrl>
					<url>https://www.comune.roma.it/servizi/certificati/recupero</url>
					<validitaCertificato>Questo certificato ha valore legale per 6 mesi a partire dalla data di emissione</validitaCertificato>
				</indice>
				<generalita>
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
					<xsl:if test="./SEX != ''">
						<sesso>
							<xsl:value-of select="./SEX"/>
						</sesso>
					</xsl:if>
					<xsl:if test="./SEX = 'M'">
						<txtNascita>
							<xsl:text>NATO IL</xsl:text>
						</txtNascita>
					</xsl:if>
					<xsl:if test="./SEX = 'F'">
						<txtNascita>NATA IL</txtNascita>
					</xsl:if>
					<xsl:if test="./GNAS != '' or ./MNAS != '' or ./ANAS != ''">
						<dataNascita>
							<xsl:value-of select="./GNAS"/> /
							<xsl:value-of select="./MNAS"/> /
							<xsl:value-of select="./ANAS"/>
						</dataNascita>
					</xsl:if>
					<xsl:if test="./COMNAS != ''">
						<luogoNascita>
							<txtComune>A</txtComune>
							<nomeComune>
								<xsl:value-of select="./COMNAS"/>
							</nomeComune>
							<xsl:if test="./PRCNAS != ''">
								<siglaProvincia>
									<xsl:value-of select="./PRCNAS"/>
								</siglaProvincia>
							</xsl:if>
							<xsl:if test="./NAZCNAS != ''">
								<siglaStato>
									<xsl:value-of select="./NAZCNAS"/>
								</siglaStato>
							</xsl:if>
							<xsl:if test="./DNAZCNAS != ''">
								<nomeStato>
									<xsl:value-of select="./DNAZCNAS"/>
								</nomeStato>
							</xsl:if>
						</luogoNascita>
					</xsl:if>
					<xsl:if test="./NATTNAS != '' or ./PATTNAS != '' or ./SATTNAS != '' or ./AANAS != '' or ./TIPRGNAS !='' or ./COMRGNAS !=''">
						<attoNascita>
							<txtNumero>atto N.</txtNumero>
							<txtParte>parte</txtParte>
							<txtSerie>serie</txtSerie>
							<xsl:if test="./TIPRGNAS != ''">
								<tipoRegistrazione>
									<xsl:value-of select="./TIPRGNAS"/>
								</tipoRegistrazione>
							</xsl:if>
							<xsl:if test="./COMRGNAS != ''">
								<comuneRegistrazione>
									<nomeComune>
										<xsl:value-of select="./COMRGNAS"/>
									</nomeComune>
									<xsl:if test="./PRCRGNAS != ''">
										<siglaProvincia>
											<xsl:value-of select="./PRCRGNAS"/>
										</siglaProvincia>
									</xsl:if>
									<xsl:if test="./NAZCRGNAS != ''">
										<siglaStato>
											<xsl:value-of select="./NAZCRGNAS"/>
										</siglaStato>
									</xsl:if>
									<xsl:if test="./DNAZCRGNAS != ''">
										<nomeStato>
											<xsl:value-of select="./DNAZCRGNAS"/>
										</nomeStato>
									</xsl:if>
								</comuneRegistrazione>
							</xsl:if>
							<xsl:if test="./NATTNAS != ''">
								<numero>
									<xsl:value-of select="./NATTNAS"/>
								</numero>
							</xsl:if>
							<xsl:if test="./PATTNAS != ''">
								<parte>
									<xsl:value-of select="./PATTNAS"/>
								</parte>
							</xsl:if>
							<serie>
								<xsl:value-of select="./SATTNAS"/>
							</serie>
							<xsl:if test="./EATTNAS != ''">
								<esponente>
									<xsl:value-of select="./EATTNAS"/>
								</esponente>
							</xsl:if>
							<xsl:if test="./AANAS != ''">
								<anno>
									<xsl:value-of select="./AANAS"/>
								</anno>
							</xsl:if>
						</attoNascita>
					</xsl:if>
					<xsl:if test="./CODFIS != ''">
						<txtCodiceFiscale>Cod.Fis.</txtCodiceFiscale>
						<codiceFiscale>
							<xsl:value-of select="./CODFIS"/>
						</codiceFiscale>
					</xsl:if>
					<xsl:if test="./DESCIT != '' or ./ISTNAZ !=''">
						<cittadinanza>
							<xsl:value-of select="./DESCIT"/>
						</cittadinanza>
					</xsl:if>
					<xsl:if test="./STATCIV != ''">
						<statoCivile>
							<xsl:value-of select="./STATCIV"/>
						</statoCivile>
					</xsl:if>
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
						<xsl:choose>
							<xsl:when test="./STATCIV = 'coniugato' or ./STATCIV = 'coniugata' or ./STATCIV = 'unito civilmente' or ./STATCIV = 'unita civilmente'">
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
				<decesso>
					<txtDecesso>È MORTO</txtDecesso>
					<xsl:if test="./GDEC != '' or ./MDEC != '' or ./ADEC != ''">
						<txtDataDecesso>il</txtDataDecesso>
						<dataDecesso>
							<xsl:value-of select="./GDEC"/>/<xsl:value-of select="./MDEC"/>/
							<xsl:value-of select="./ADEC"/>
						</dataDecesso>
					</xsl:if>
					<xsl:if test="./COMDEC != ''">
						<luogoDecesso>
							<txtComune>a</txtComune>
							<nomeComune>
								<xsl:value-of select="./COMDEC"/>
							</nomeComune>
							<xsl:if test="./PRCDEC != ''">
								<siglaProvincia>
									<xsl:value-of select="./PRCDEC"/>
								</siglaProvincia>
							</xsl:if>
							<xsl:if test="./NAZCDEC != ''">
								<siglaStato>
									<xsl:value-of select="./NAZCDEC"/>
								</siglaStato>
							</xsl:if>
							<xsl:if test="./DNAZCDEC != ''">
								<nomeStato>
									<xsl:value-of select="./DNAZCDEC"/>
								</nomeStato>
							</xsl:if>
						</luogoDecesso>
					</xsl:if>
					<xsl:if test="./NATTDEC != '' or ./PATTDEC != '' or ./SATTDEC != '' or ./AADEC != '' or ./TIPRGDEC != '' or ./COMRGDEC != ''">
						<attoDecesso>
							<txtNumero>Atto N.</txtNumero>
							<txtParte>Parte</txtParte>
							<txtSerie>Serie</txtSerie>
							<xsl:if test="./COMRGDEC != ''">
								<comuneRegistrazione>
									<xsl:if test="./TIPRGDEC != ''">
										<xsl:choose>
											<xsl:when test="./TIPRGDEC = 'T'">
												<txtComune>trascritto nel comune di</txtComune>
											</xsl:when>
											<xsl:otherwise>
												<txtComune>del comune di</txtComune>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:if>
									<nomeComune>
										<xsl:value-of select="./COMRGDEC"/>
									</nomeComune>
									<xsl:if test="./PRCRGDEC != ''">
										<siglaProvincia>
											<xsl:value-of select="./PRCRGDEC"/>
										</siglaProvincia>
									</xsl:if>
									<xsl:if test="./NAZCRGDEC != ''">
										<siglaStato>
											<xsl:value-of select="./NAZCRGDEC"/>
										</siglaStato>
									</xsl:if>
									<xsl:if test="./DNAZCRGDEC != ''">
										<nomeStato>
											<xsl:value-of select="./DNAZCRGDEC"/>
										</nomeStato>
									</xsl:if>
								</comuneRegistrazione>
							</xsl:if>
							<xsl:if test="./NATTDEC != ''">
								<numero>
									<xsl:value-of select="./NATTDEC"/>
								</numero>
							</xsl:if>
							<xsl:if test="./PATTDEC != ''">
								<parte>
									<xsl:value-of select="./PATTDEC"/>
								</parte>
							</xsl:if>
							<serie>
								<xsl:value-of select="./SATTDEC"/>
							</serie>
							<xsl:if test="./EATTDEC != ''">
								<esponente>
									<xsl:value-of select="./EATTDEC"/>
								</esponente>
							</xsl:if>
							<xsl:if test="./AADEC != ''">
								<anno>
									<xsl:value-of select="./AADEC"/>
								</anno>
							</xsl:if>
						</attoDecesso>
					</xsl:if>
					<xsl:if test="./COMRESDEC != ''">
						<residenzaAllaDataDecesso>
							<txtComune>RESIDENTE A</txtComune>
							<nomeComune>
								<xsl:value-of select="./COMRESDEC"/>
							</nomeComune>
						</residenzaAllaDataDecesso>
					</xsl:if>
				</decesso>
			</xsl:for-each>
		</RispostaCertificato>
	</xsl:template>
</xsl:stylesheet>