using LabApplicationBackendLayer.DAL;
using LabApplicationBackendLayer.Utilities;

namespace LabApplicationBackendLayer.BLL
{
    public enum BackEndDB
    {
        SQL = 0,
        COSMOS
    }

    internal class DALClientFactory
    {
        public static ILabBackendDAL GetDBClient(BackEndDB dbChoice, ConfigReader configs)
        {
            ILabBackendDAL backendDAL;

            switch (dbChoice)
            {
                case BackEndDB.SQL:
                    backendDAL = new EmployeeSQLDAL();
                    break;
                case BackEndDB.COSMOS:
                    backendDAL = new EmployeeCOSMOSDAL();
                    break;
                default:
                    backendDAL = new EmployeeSQLDAL();
                    break;
            }

            return backendDAL;
        }
    }
}
