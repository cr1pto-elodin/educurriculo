--Problemas na execução durante o comando de start do container. Havaliar depois.

CREATE DATABASE PORTAL;
GO

USE PORTAL;
GO

CREATE TABLE ACESSO(
    ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    USER_NAME VARCHAR(100),
    SENHA VARCHAR(32),
    ATIVO BIT DEFAULT(1),
    RECCREATEDON DATETIME DEFAULT(GETDATE())
);
GO

CREATE TABLE CADASTRO(
    CPF VARCHAR(11) NOT NULL PRIMARY KEY,
    ID_ACESSO INT NOT NULL FOREIGN KEY REFERENCES ACESSO(ID),
    NOME VARCHAR(200),
    DATANASCIMENTO DATETIME,
    SEXO CHAR(1),
    ESTADO_CIVIL CHAR(1),
    ESCOLARIDADE CHAR(2),
    PRETENSAO_SAL DECIMAL(18,2),
    RECCREATEDON DATETIME DEFAULT(GETDATE())
);
GO

CREATE TABLE CERTIFICACOES(
    CPF VARCHAR(11) NOT NULL FOREIGN KEY REFERENCES CADASTRO(CPF),
    ID_CERTIFICACOES INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    INSTITUICAO VARCHAR(100),
    CURSO VARCHAR(50),
    INICIO INT,
    FIM INT,
    RECCREATEDON DATETIME DEFAULT(GETDATE())
);
GO

CREATE TABLE EXPERIENCIA(
    CPF VARCHAR(11) NOT NULL FOREIGN KEY REFERENCES CADASTRO(CPF),
    ID_EXPERIENCIA INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    EMPRESA VARCHAR(150),
    INICIOEX DATETIME,
    FIMEX DATETIME,
    RECCREATEDON DATETIME DEFAULT(GETDATE())
);
GO

INSERT INTO ACESSO (USER_NAME,SENHA) 
VALUES ('adm','81dc9bdb52d04dc20036dbd8313ed055');
GO

INSERT INTO CADASTRO (CPF,ID_ACESSO,NOME,SEXO,DATANASCIMENTO,ESTADO_CIVIL,ESCOLARIDADE)
VALUES ('16495167709',1,'Victor Polck','M','2000-03-11 00:00:000','S','MC');
GO

-- CREATE PROCEDURE SP_CADASTRO(
--     @NOME VARCHAR(200),
--     @CPF VARCHAR(11),
--     @DATANASCIMENTO DATETIME,
--     @SENHA VARCHAR(32)
-- )
-- AS(
--     INSERT INTO USUARIO (NOME,CPF,DATANASCIMENTO,SENHA)
--     VALUES (@NOME,@CPF,@DATANASCIMENTO,@SENHA)
-- )

CREATE OR ALTER PROCEDURE SP_CONSULTAR_USUARIO(
    @CPF VARCHAR(11)
)
AS(
    SELECT 1 FROM CADASTRO WHERE CPF = @CPF
)
GO

CREATE OR ALTER PROCEDURE SP_CADASTRO_ACESSO(
    @USER_NAME VARCHAR(100),
    @SENHA VARCHAR(32)
)
AS
BEGIN
    INSERT INTO ACESSO ([USER_NAME],SENHA)
    VALUES (@USER_NAME, @SENHA)

    SELECT SCOPE_IDENTITY()
END
GO

CREATE OR ALTER PROCEDURE SP_CADASTRO_DADOS(
    @CPF VARCHAR(11),
    @ID INT,
    @NOME VARCHAR(200),
    @DATANASCIMENTO DATETIME,
    @SEXO CHAR(1),
    @ESTADO_CIVIL CHAR(1),
    @ESCOLARIDADE CHAR(2),
    @PRETENSAO_SAL DECIMAL(18,2)
)
AS
BEGIN
    INSERT INTO CADASTRO(CPF, ID_ACESSO, NOME, DATANASCIMENTO, SEXO, ESTADO_CIVIL, ESCOLARIDADE, PRETENSAO_SAL)
    VALUES (@CPF,@ID,@NOME,@DATANASCIMENTO,@SEXO,@ESTADO_CIVIL,@ESCOLARIDADE,@PRETENSAO_SAL)
END
GO

CREATE OR ALTER PROCEDURE SP_DELETAR_EXPERIENCIA(
    @CPF VARCHAR(11)
)
AS
BEGIN
    DELETE EXPERIENCIA WHERE CPF = @CPF 
END
GO

CREATE OR ALTER PROCEDURE SP_CADASTRO_EXPERIENCIA(
    @CPF VARCHAR(11),
    @EMPRESA VARCHAR(150),
    @INICIOEX DATETIME,
    @FIMEX DATETIME
)
AS
BEGIN
    INSERT INTO EXPERIENCIA (CPF, EMPRESA, INICIOEX, FIMEX)
    VALUES (@CPF, @EMPRESA, @INICIOEX, @FIMEX)
END
GO

CREATE OR ALTER PROCEDURE SP_DELETA_CERTIFICACOES(
    @CPF VARCHAR(11)
)
AS
BEGIN
    DELETE CERTIFICACOES WHERE CPF = @CPF
END
GO

CREATE OR ALTER PROCEDURE SP_CADASTRO_CERTIFICACOES(
    @CPF VARCHAR(11),
    @INSTITUICAO(100),
    @CURSO VARCHAR(50),
    @INICIO INT,
    @FIM INT
)
AS
BEGIN
    INSERT INTO CERTIFICACOES (CPF,CURSO)
    VALUES (@CPF,@CURSO)
END
GO
