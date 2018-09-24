using LabApplicationBackendLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabApplicationBackendLayer.DAL
{
    public interface ILabBackendDAL
    {
        IEnumerable<Employee> GetAllEmployees();

        //To Add new employee record    
        void AddEmployee(Employee employee);

        //To Update the records of a particluar employee  
        void UpdateEmployee(Employee employee);

        //Get the details of a particular employee  
        Employee GetEmployeeData(string id);

        //To Delete the record on a particular employee  
        void DeleteEmployee(string id);

        void PerformDeleteUpdate(string employeeId, int trackNumber);
    }
}
