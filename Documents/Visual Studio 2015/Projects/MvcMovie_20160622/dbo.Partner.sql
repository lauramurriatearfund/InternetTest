USE [MvcMovie3]
GO

/****** Object: Table [dbo].[Partner] Script Date: 21/09/2016 11:29:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Partner] (
    [PartnerId]   INT            NOT NULL,
    [PartnerName] NVARCHAR (100) NOT NULL,
    [PartnerRef]  NVARCHAR (40)  NOT NULL,
    [Country]     NVARCHAR (MAX) NOT NULL,
    [CreatedDate] DATETIME       NOT NULL
);


