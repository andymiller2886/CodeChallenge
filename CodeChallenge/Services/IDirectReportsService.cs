using CodeChallenge.Models;
using System;

namespace CodeChallenge.Services
{
    public interface IDirectReportsService
    {
        ReportingStructure GetById(String id);
    }
}
