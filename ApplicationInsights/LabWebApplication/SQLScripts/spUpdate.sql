SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[spUpdateEmployee]      
(      
   @EmpId INTEGER ,    
   @Name VARCHAR(20),     
   @City VARCHAR(20),    
   @Department VARCHAR(20),    
   @Gender VARCHAR(6)    
)      
as      
begin      
   Update tblEmployee       
   set Name=@Name,      
   City=@City,      
   Department=@Department,    
   Gender=@Gender      
   where EmployeeId=@EmpId      
End 
GO


