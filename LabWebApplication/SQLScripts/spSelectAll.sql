SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[spGetAllEmployees]    
as    
Begin    
    select *    
    from tblEmployee    
End  
GO


