USE [RouletteBets]
GO

/****** Object:  Table [dbo].[Roulette]    Script Date: 30/11/2020 8:28:16 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Roulette](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nchar](50) NOT NULL,
	[IsOpen] [bit] NOT NULL,
 CONSTRAINT [PK_Roulette] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Roulette] ADD  CONSTRAINT [DF_Roulette_IsOpen]  DEFAULT ((0)) FOR [IsOpen]
GO
