using Newtonsoft.Json;
using SoumavoWebApplication.Entity_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SoumavoWebApplication.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client;
        string url = "http://localhost:8477/";
        public HomeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static Models.Search _search = new Models.Search();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Search()
        {
            _search.Payment = new List<Models.Payment>();
            _search.Vehicles = new List<Models.Vehicle>();
            //Fetch data from DB
            using (SoumavoEntities _ent = new SoumavoEntities())
            {
                var paymentDetails = (from a in _ent.Payments select a).ToList();
                foreach (var item in paymentDetails)
                {
                    _search.Payment.Add(new Models.Payment() { Currency = item.Payment_Curr });
                }

                var vehicleDetails = (from a in _ent.Vehicles select a).ToList();
                foreach (var item in vehicleDetails)
                {
                    _search.Vehicles.Add(new Models.Vehicle() { Name = item.Vehicle_Name });
                }
            }

            return View(_search);
        }

        [HttpPost]
        public ActionResult Search(Models.Search _search)
        {
            //Save the data to Database
            if (_search != null)
            {
                using (SoumavoEntities _ent = new SoumavoEntities())
                {
                    Employee _emp = new Employee()
                    {
                        Name = _search.Name,
                        FromDate = Convert.ToDateTime(Convert.ToDateTime(_search.FromDate).ToShortDateString()),
                        ToDate = Convert.ToDateTime(Convert.ToDateTime(_search.ToDate).ToShortDateString()),
                        Payment = _search.PaymentName,
                        Vehicle = _search.VehicleName
                    };
                    _ent.Employees.Add(_emp);
                    _ent.SaveChanges();
                    TempData["Message"] = "Saved";
                }
            }
            return RedirectToAction("Search", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> SeeData()
        {
            List<Employee> _emp = new List<Employee>();
            HttpResponseMessage responseMessage = await client.GetAsync(url + "Soumavo/SeeData");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                _emp = JsonConvert.DeserializeObject<List<Employee>>(responseData);
            }
            return View(_emp);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<Models.Vehicle> _veh = new List<Models.Vehicle>();
            List<Models.Payment> _pay = new List<Models.Payment>();
            //new object for every request
            Models.Search _s = new Models.Search();
            using (SoumavoEntities _ent = new SoumavoEntities())
            {
                //populate the vehicle dropdown
                var _vehicleList = (from a in _ent.Vehicles select a).ToList();
                foreach (var item in _vehicleList)
                {
                    _veh.Add(new Models.Vehicle()
                    {
                        Name = item.Vehicle_Name,
                        VehicleId = item.Id
                    });
                }
                //populate the Payment Dropdown
                var _paymentList = (from a in _ent.Payments select a).ToList();
                foreach (var item in _paymentList)
                {
                    _pay.Add(new Models.Payment()
                    {
                        Currency = item.Payment_Curr,
                        CurrencyId = item.Id
                    });
                }


                var result = (from a in _ent.Employees where a.Id.Equals(id) select a).FirstOrDefault();
                _s.Name = result.Name;
                _s.FromDate = result.FromDate;
                _s.ToDate = result.ToDate;
                _s.VehicleName = result.Vehicle;
                _s.PaymentName = result.Payment;
                _s.Id = result.Id;
                _s.Payment = _pay;
                _s.Vehicles = _veh;
            }
            return PartialView(_s);
        }

        [HttpPost]
        public ActionResult Update(Models.Search _search)
        {
            string retval = string.Empty;
            using (SoumavoEntities _ent = new SoumavoEntities())
            {
                var result = (from a in _ent.Employees where a.Id.Equals(_search.Id) select a).FirstOrDefault();

                if (result != null)
                {

                    Employee _emp = new Employee()
                    {
                        FromDate = _search.FromDate,
                        Name = _search.Name,
                        Payment = _search.PaymentName,
                        ToDate = _search.ToDate,
                        Vehicle = _search.VehicleName,
                        Id = _search.Id,

                    };
                    _ent.Entry(result).CurrentValues.SetValues(_emp);
                    _ent.SaveChanges();
                    TempData["Message"] = "Updated Successfully";
                }
                else
                {
                    TempData["Message"] = "Error Occured";
                }
            }
            return RedirectToAction("SeeData");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            bool res = false;
            using (SoumavoEntities _ent = new SoumavoEntities())
            {
                var result = (from a in _ent.Employees where a.Id.Equals(id) select a).FirstOrDefault();
                if (result != null)
                {
                    _ent.Employees.Remove(result);
                    _ent.SaveChanges();
                    res = true;
                }
            }
            return Json(res);
        }
    }
}