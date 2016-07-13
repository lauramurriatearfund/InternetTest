using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MvcMovie.Models
{
    public class Partner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }



        [Required(ErrorMessageResourceType = typeof(Resources.Resources),   ErrorMessageResourceName = "PartnerNameRequired")]
        [Display(ResourceType= typeof(Resources.Resources), Name= "PartnerName")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PartnerNameLength" , MinimumLength = 6)]
        public string partnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PartnerRefRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "PartnerRef")]
        [StringLength(40, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PartnerRefLength", MinimumLength = 6)]
        public string partnerRef { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CountryRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "Country")]
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