using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabApplicationBackendLayer.BLL;
using LabApplicationBackendLayer.DAL;
using LabApplicationBackendLayer.Models;
using LabApplicationBackendLayer.Utilities;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LabWebApplication.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeesBLL objemployee;
        TelemetryClient tlmtryClnt;
        static List<Employee> lstEmployee = new List<Employee>();

        public EmployeeController(IOptions<AllConfiguration> configs, IOptions<ApplicationInsightsConfiguration> appConfig)
        {
            objemployee = new EmployeesBLL(configs);
            tlmtryClnt = new TelemetryClient(new TelemetryConfiguration
            {
                InstrumentationKey = appConfig.Value.InstrumentationKey
            });
        }

        public IActionResult Index()
        {
            lstEmployee = objemployee.GetAllEmployees().ToList();
            return View(lstEmployee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Employee employee)
        {
            if (ModelState.IsValid)
            {
                tlmtryClnt.TrackException(new ExceptionTelemetry
                {
                    Exception = new Exception("Exception in Create Post")
                });
                
                tlmtryClnt.TrackEvent(new EventTelemetry
                {
                    Name = "Starting Create"
                    
                });

                DateTime startTime = DateTime.Now;

                objemployee.AddEmployee(employee);

                DateTime endTime = DateTime.Now;

                tlmtryClnt.TrackDependency("CreationDependency", "Create Post", DateTimeOffset.MaxValue, endTime - startTime, true);

                return RedirectToAction("Index");
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = objemployee.GetEmployeeData(id);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind]Employee employee)
        {
            if (id != employee.EmpId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                objemployee.UpdateEmployee(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = objemployee.GetEmployeeData(id);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = objemployee.GetEmployeeData(id);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            objemployee.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GenerateLock(string id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateLock()
        {
            RedirectToActionResult rslt = RedirectToAction("Index");

            TelemetryClient telemetryClient = GetTelemetryClient();

            var requestTelemetry = new RequestTelemetry
            {
                //Name = $"{HttpContext.Request.Method} {HttpContext.Request .GetLeftPart(UriPartial.Path)}"
                Name = "TelemetryException"
            };

            // If there is a Request-Id received from the upstream service, set the telemetry context accordingly.
            if (HttpContext.Request.Headers.ContainsKey("Request-Id"))
            {
                var requestId = HttpContext.Request.Headers["Request-Id"];
                // Get the operation ID from the Request-Id (if you follow the HTTP Protocol for Correlation).
                requestTelemetry.Context.Operation.Id = GetOperationId(requestId);
                requestTelemetry.Context.Operation.ParentId = requestId;
            }

            // StartOperation is a helper method that allows correlation of 
            // current operations with nested operations/telemetry
            // and initializes start time and duration on telemetry items.
            var operation = telemetryClient.StartOperation(requestTelemetry);

            try
            {
                int numberOfThreads = 35;

                List<int> lstCounter = Enumerable.Range(1, numberOfThreads).ToList();

                Parallel.ForEach(lstCounter, (trackNumber) =>
                {
                    objemployee.PerformDeleteUpdate(lstEmployee[trackNumber].EmpId, trackNumber);
                });
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
            }
            finally
            {
                telemetryClient.StopOperation(operation);
                RedirectToPage("Index");
            }

            return rslt;
        }

        [HttpGet]
        public IActionResult CreateURLError()
        {
            return RedirectToAction("/Index400");
        }

        private TelemetryClient GetTelemetryClient()
        {
            TelemetryClient telemetryClient = new TelemetryClient(TelemetryConfiguration.Active);

            var requestTelemetry = new RequestTelemetry
            {
                Name = "TelemetryException"
            };

            // If there is a Request-Id received from the upstream service, set the telemetry context accordingly.
            if (HttpContext.Request.Headers.ContainsKey("Request-Id"))
            {
                var requestId = HttpContext.Request.Headers["Request-Id"];
                // Get the operation ID from the Request-Id (if you follow the HTTP Protocol for Correlation).
                requestTelemetry.Context.Operation.Id = GetOperationId(requestId);
                requestTelemetry.Context.Operation.ParentId = requestId;
            }

            // StartOperation is a helper method that allows correlation of 
            // current operations with nested operations/telemetry
            // and initializes start time and duration on telemetry items.
            var operation = telemetryClient.StartOperation(requestTelemetry);

            return telemetryClient;
        }

        private static string GetOperationId(string id)
        {
            // Returns the root ID from the '|' to the first '.' if any.
            int rootEnd = id.IndexOf('.');
            if (rootEnd < 0)
                rootEnd = id.Length;

            int rootStart = id[0] == '|' ? 1 : 0;
            return id.Substring(rootStart, rootEnd - rootStart);
        }
    }
}