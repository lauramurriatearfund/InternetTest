using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMovie.Models
{
    public class RentOutSpace
    {
        [HiddenInput(DisplayValue = false)]
        public string latitude { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string longitude { get; set; }
        public string address { get; set; }
    }
}