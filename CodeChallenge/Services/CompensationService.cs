using System;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;
using CodeChallenge.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DirectReportsService> _logger;

        public CompensationService(ILogger<DirectReportsService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            // Load existing salary data
            var compensationData = SerializerHelper.DeSerializeObject<List<Compensation>>("CompensationData.xml");

            //Find existing compensation data if available, returning the active one that isn't end dated
            var existingCompensation = compensationData.FirstOrDefault(c => c.Employee.EmployeeId == compensation.Employee.EmployeeId && c.EndDate == null);

            // End date old record so new record is used
            if (existingCompensation != null)
            {
                existingCompensation.EndDate = compensation.EffectiveDate;
            }

            else
            {
                // With no current salary data, verify the employee exists
                bool employeeExists = _employeeRepository.GetById(compensation.Employee.EmployeeId) != null;

                // If they do, add the salary data
                if (employeeExists)
                {
                    compensationData.Add(compensation);
                }

                // If they don't, let the client know it was a bad request
                else
                {
                    return null;
                }
            }

            // Save data to file and return.  Ideally this would be in a database!
            SerializerHelper.SerializeObject(@"CompensationData.xml", compensationData);

            return compensation;
        }

        public Compensation GetById(string id)
        {
            // Load existing salary data
            var compensationData = SerializerHelper.DeSerializeObject<List<Compensation>>("CompensationData.xml");

            //Find existing compensation data if available, returning the current one that isn't end dated
            var compensation = compensationData.FirstOrDefault(c => c.Employee.EmployeeId == id && c.EndDate == null);

            if (compensation != null)
            {
                return compensation;
            }

            return null;
        }
    }
}
