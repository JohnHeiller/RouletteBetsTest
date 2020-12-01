USE [RouletteBets]
GO

/****** Object:  Table [dbo].[User]    Script Date: 30/11/2020 8:24:54 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](100) NOT NULL,
	[NumberId] [nchar](100) NOT NULL,
	[Email] [nchar](100) NULL,
	[Telephone] [nchar](100) NULL,
	[CreditMoney] [bigint] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
