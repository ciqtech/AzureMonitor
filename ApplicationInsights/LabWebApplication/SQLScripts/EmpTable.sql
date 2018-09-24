SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblEmployee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NULL,
	[City] [varchar](20) NULL,
	[Department] [varchar](20) NULL,
	[Gender] [varchar](6) NULL
) ON [PRIMARY]
GO


