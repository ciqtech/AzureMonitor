SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[spDeleteEmployee]     
(      
   @EmpId int      
)      
as       
begin      
   Delete from tblEmployee where EmployeeId=@EmpId      
End  
GO


