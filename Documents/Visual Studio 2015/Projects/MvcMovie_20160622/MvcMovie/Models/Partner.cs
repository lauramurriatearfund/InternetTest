﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcMovie.Models
{
    public class Partner
    {
        public int ID { get; set; }



        [Required(ErrorMessageResourceType = typeof(Resources.Resources),   ErrorMessageResourceName = "PartnerNameRequired")]
        //[Display(ResourceType= typeof(Resources.Resources), Name= "PartnerName")]
        [Display(Name = "Partner Name")]
        [StringLength(100, ErrorMessage = "Partner Name must be at least 6 characters long.", MinimumLength = 6)]
        public string partnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PartnerRefRequired")]
        [Display(Name = "Partner Reference")]
        //[StringLength(40, ErrorMessage = "Partner Reference must be at least 6 characters long.", MinimumLength = 6)]
        public string partnerRef { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string country { get; set; }

        public DateTime createdDate { get; set; }

        public static readonly List<SelectListItem> COUNTRIES = new List<SelectListItem>()
        {
        new SelectListItem() {Text="Afghanistan", Value="AF"},
        new SelectListItem() { Text="Bangladesh", Value="BD"},
        new SelectListItem() { Text="Cambodia", Value="KH"},
        new SelectListItem() { Text="Denmark", Value="DK"},
        new SelectListItem() { Text="Ethiopia", Value="ET"},
        new SelectListItem() { Text="French Guiana", Value="GF"},
        new SelectListItem() { Text="Gambia", Value="GM"}


        };
    }
}