using Soumavo.APILayer.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Soumavo.APILayer.Controllers
{
    /// <summary>
    /// Soumavo Controller
    /// </summary>
    public class SoumavoController : ApiController
    {
        /// <summary>
        /// List All Employee Data
        /// </summary>
        /// <returns>List Of Employee</returns>
        [Route("Soumavo/SeeData")]
        [HttpGet]
        public List<Employee> SeeData()
        {
            List<Employee> emp = new List<Employee>();
            using (SoumavoEntities _ent = new SoumavoEntities())
            {
                var employeeDetails = (from a in _ent.Employees select a).ToList();
                foreach (var item in employeeDetails)
                {
                    emp.Add(new Employee()
                    {
                        FromDate = item.FromDate,
                        Name = item.Name,
                        Payment = item.Payment,
                        ToDate = item.ToDate,
                        Vehicle = item.Vehicle,
                        Id = item.Id
                    });
                }
            }
            return emp;
        }
    }
}
