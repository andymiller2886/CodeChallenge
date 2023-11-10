using System;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class DirectReportsService : IDirectReportsService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<DirectReportsService> _logger;

        public DirectReportsService(ILogger<DirectReportsService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetReportingStructureById(id);
            }

            return null;
        }
    }
}
