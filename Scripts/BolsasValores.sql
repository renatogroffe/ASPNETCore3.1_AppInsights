USE BaseIndicadores
GO

CREATE TABLE dbo.BolsasValores(
	Sigla VARCHAR(10) NOT NULL,
	NomeBolsa VARCHAR(60) NOT NULL,
	DataReferencia DATETIME NOT NULL,
	Variacao NUMERIC (10,4) NOT NULL,
	CONSTRAINT PK_BolsasValores PRIMARY KEY (Sigla)
)
GO


INSERT INTO dbo.BolsasValores
           (Sigla
           ,NomeBolsa
           ,DataReferencia
           ,Variacao)
     VALUES
           ('BOVESPA'
           ,'Brasil | Bovespa'
           ,'02/15/2020'
           ,-0.0111)

INSERT INTO dbo.BolsasValores
           (Sigla
           ,NomeBolsa
           ,DataReferencia
           ,Variacao)
     VALUES
           ('NASDAQ'
           ,'EUA | NASDAQ'
           ,'02/15/2020'
           ,0.0020)

INSERT INTO dbo.BolsasValores
           (Sigla
           ,NomeBolsa
           ,DataReferencia
           ,Variacao)
     VALUES
           ('NIKKEI'
           ,'Japão | Nikkei'
           ,'02/15/2020'
           ,-0.0059)