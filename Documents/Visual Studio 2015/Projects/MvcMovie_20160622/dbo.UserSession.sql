USE [MvcMovie3]
GO

/****** Object: Table [dbo].[UserSession] Script Date: 21/09/2016 11:30:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserSession] (
    [SessionId] NVARCHAR (128) NOT NULL,
    [PartnerId] INT            NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_PartnerId]
    ON [dbo].[UserSession]([PartnerId] ASC);


GO
ALTER TABLE [dbo].[UserSession]
    ADD CONSTRAINT [PK_dbo.UserSession] PRIMARY KEY CLUSTERED ([SessionId] ASC);


GO
ALTER TABLE [dbo].[UserSession]
    ADD CONSTRAINT [FK_dbo.UserSession_dbo.Partner_PartnerId] FOREIGN KEY ([PartnerId]) REFERENCES [dbo].[Partner] ([PartnerId]);


