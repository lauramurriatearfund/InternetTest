using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcMovie.Models
{
    public class Location
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "Latitude")]
        public long latitude { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "Longitude")]
        public long longitude { get; set; }
    }
}