using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoumavoWebApplication.Models
{
    public class Search
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> FromDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ToDate { get; set; }
        public string VehicleName { get; set; }
        public string PaymentName { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<Payment> Payment { get; set; }
    }

    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string Name { get; set; }
    }

    public class Payment
    {
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
    }
}