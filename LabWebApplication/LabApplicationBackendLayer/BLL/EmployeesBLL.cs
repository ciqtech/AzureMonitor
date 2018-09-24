using System;
using System.Collections.Generic;
using System.Text;
using LabApplicationBackendLayer.DAL;
using LabApplicationBackendLayer.Models;
using LabApplicationBackendLayer.Utilities;
using Microsoft.Extensions.Options;

namespace LabApplicationBackendLayer.BLL
{
    public class EmployeesBLL
    {
        ILabBackendDAL backendDal;

        public EmployeesBLL(IOptions<AllConfiguration> configs)
        {
            ConfigReader configurations = new ConfigReader(configs);
            Dictionary<string, string> envArgs = EnvironmentHelper.Arguments;
            string passedEnvironment;

            if (envArgs != null && envArgs.Count > 0)
            {
                passedEnvironment = envArgs[EnvironmentHelper.ENV_BACKEND];
            }
            else
            {
                passedEnvironment = ConfigReader.BackendDBType;
            }

            BackEndDB dbChoice = passedEnvironment == "COSMOS" ? BackEndDB.COSMOS : BackEndDB.SQL;
            this.backendDal = DALClientFactory.GetDBClient(dbChoice, configurations);
        }

        public void AddEmployee(Employee employee)
        {
            backendDal.AddEmployee(employee);
        }

        public void DeleteEmployee(string id)
        {
            backendDal.DeleteEmployee(id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return backendDal.GetAllEmployees();
        }

        public Employee GetEmployeeData(string id)
        {
            return backendDal.GetEmployeeData(id);
        }

        public void UpdateEmployee(Employee employee)
        {
            backendDal.UpdateEmployee(employee);
        }
        public void PerformDeleteUpdate(string employeeId, int trackNumber)
        {
            backendDal.PerformDeleteUpdate(employeeId, trackNumber);
        }
    }
}
