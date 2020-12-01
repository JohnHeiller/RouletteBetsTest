USE [RouletteBets]
GO

/****** Object:  Table [dbo].[Bets]    Script Date: 30/11/2020 8:24:08 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Bets](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Number] [bigint] NULL,
	[Color] [bit] NULL,
	[Money] [bigint] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[RouletteId] [bigint] NOT NULL,
	[BetTime] [nchar](100) NOT NULL,
	[WonMoney] [float] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Bets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Bets]  WITH CHECK ADD  CONSTRAINT [FK_Bets_Roulette] FOREIGN KEY([RouletteId])
REFERENCES [dbo].[Roulette] ([Id])
GO

ALTER TABLE [dbo].[Bets] CHECK CONSTRAINT [FK_Bets_Roulette]
GO

ALTER TABLE [dbo].[Bets]  WITH CHECK ADD  CONSTRAINT [FK_Bets_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO

ALTER TABLE [dbo].[Bets] CHECK CONSTRAINT [FK_Bets_User]
GO
