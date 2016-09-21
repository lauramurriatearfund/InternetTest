USE [MvcMovie3]
GO

/****** Object: Table [dbo].[Metric] Script Date: 21/09/2016 11:28:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Metric] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [UserSessionId]         NVARCHAR (MAX) NULL,
    [Timestamp]             DATETIME       NULL,
    [MetricName]            NVARCHAR (MAX) NOT NULL,
    [MetricValue]           NVARCHAR (MAX) NOT NULL,
    [UserSession_SessionId] NVARCHAR (128) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_UserSession_SessionId]
    ON [dbo].[Metric]([UserSession_SessionId] ASC);


GO
ALTER TABLE [dbo].[Metric]
    ADD CONSTRAINT [PK_dbo.Metric] PRIMARY KEY CLUSTERED ([Id] ASC);


GO
ALTER TABLE [dbo].[Metric]
    ADD CONSTRAINT [FK_dbo.Metric_dbo.UserSession_UserSession_SessionId] FOREIGN KEY ([UserSession_SessionId]) REFERENCES [dbo].[UserSession] ([SessionId]);


