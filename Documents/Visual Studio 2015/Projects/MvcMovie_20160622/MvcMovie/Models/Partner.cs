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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PartnerId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources),   ErrorMessageResourceName = "PartnerNameRequired")]
        [Display(ResourceType= typeof(Resources.Resources), Name= "PartnerName")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PartnerNameLength" , MinimumLength = 6)]
        public string PartnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PartnerRefRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "PartnerRef")]
        [StringLength(40, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PartnerRefLength", MinimumLength = 6)]
        public string PartnerRef { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "CountryRequired")]
        [Display(ResourceType = typeof(Resources.Resources), Name = "Country")]
        public string Country { get; set; }

        public DateTime CreatedDate { get; set; }

        public static readonly List<SelectListItem> COUNTRIES = new List<SelectListItem>()
        {
        new SelectListItem() { Text="Afghanistan", Value="AF"},
        new SelectListItem() {Text="Angola", Value="AO"},
        new SelectListItem() { Text="Andean Region", Value="001"},
        new SelectListItem() { Text="Bangladesh", Value="BD"},
        new SelectListItem() { Text="Brazil", Value="BR"},
        new SelectListItem() { Text="Burkina Faso", Value="BF"},
        new SelectListItem() { Text="Cambodia", Value="KH"},
        new SelectListItem() { Text="Central America", Value="013"},
        new SelectListItem() { Text="Chad", Value="TD"},
        new SelectListItem() { Text="Cote d'Ivoire", Value="CI"},
        new SelectListItem() { Text="DRC", Value="CG"},
        new SelectListItem() { Text="Ethiopia", Value="ET"},
        new SelectListItem() { Text="French Guiana", Value="GY"},
        new SelectListItem() { Text="Gambia", Value="GM"},
        new SelectListItem() { Text="Haiti", Value="HT"},
        new SelectListItem() { Text="Liberia", Value="LR"},
        new SelectListItem() { Text="Mekong Sub Region", Value="GMS"},
        new SelectListItem() { Text="Malawi", Value="MW"},
        new SelectListItem() { Text="Mali", Value="ML"},
        new SelectListItem() { Text="Mozambique", Value="MZ"},
        new SelectListItem() { Text="Myanmar", Value="MM"},
        new SelectListItem() { Text="Niger", Value="NE"},
        new SelectListItem() { Text="Nigeria", Value="NG"},
        new SelectListItem() { Text="Nepal", Value="NP"},
        new SelectListItem() { Text="Pakistan", Value="PK"},
        new SelectListItem() { Text="Russia", Value="RU"},
        new SelectListItem() { Text="Rwanda", Value="RW"},
        new SelectListItem() { Text="Sierra Leone", Value="SL"},
        new SelectListItem() { Text="Somalia", Value="SO"},
        new SelectListItem() { Text="South Sudan", Value="SS"},
        new SelectListItem() { Text="Syria", Value="SY"},
        new SelectListItem() { Text="Tanzania", Value="TZ"},
        new SelectListItem() { Text="Uganda", Value="UG"},
        new SelectListItem() { Text="Zambia", Value="ZM"},
        new SelectListItem() { Text="Zimbabwe", Value="ZW"},
        new SelectListItem() { Text="Other", Value="--"},


        };

    }
}