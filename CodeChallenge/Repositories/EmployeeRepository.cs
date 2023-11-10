using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRepository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        public ReportingStructure GetReportingStructureById(string id)
        {
            // Find Employee
            Employee employee = _employeeContext.Employees.Include("DirectReports").SingleOrDefault(e => e.EmployeeId == id);
            
            if (employee != null)
            {
                // Send to helper method to count Direct Reports
                return new ReportingStructure()
                {
                    Employee = employee,
                    NumberOfReports = CountDirectReports(employee),
                };
            }

            return null;
        }



        #region Helper Methods

        private int CountDirectReports(Employee employee)
        {
            // Initialize Counter
            int directReportsCount = 0;

            // If there are no direct reports, it'll return 0
            if (employee.DirectReports != null)
            {
                List<Employee> directReports = employee.DirectReports;

                foreach (Employee emp in directReports)
                {
                    // Increment Counter
                    directReportsCount++;

                    // Check if there are additional Direct Reports
                    Employee directReportEmployee = _employeeContext.Employees.Include("DirectReports").SingleOrDefault(e => e.EmployeeId == emp.EmployeeId);

                    // If there are, continue to count nested employees
                    if (directReportEmployee.DirectReports != null && directReportEmployee.DirectReports.Count > 0)
                    {
                        directReportsCount += CountDirectReports(directReportEmployee);
                    }
                }

            }

            return directReportsCount;
        }

        # endregion
    }
}
