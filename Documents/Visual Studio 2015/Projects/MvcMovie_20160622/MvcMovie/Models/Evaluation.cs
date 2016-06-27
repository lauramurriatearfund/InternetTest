using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcMovie.Models
{
    public class Session
    {
        public int evaluationID { get; set; }

        public Partner partner { get; set; }

        [Display(Name = "Project Name")]
        public string projectName { get; set; }

        [Display(Name = "Project Reference")]
        public string projectRef { get; set; }

        [Display(Name = "Theme")]
        public string theme { get; set; }

        [Display(Name = "Submission Date")]
        public DateTime submissionDate { get; set; }

        public static readonly List<SelectListItem> THEMES = new List<SelectListItem>()
        {

        new SelectListItem() { Text="Healthcare", Value="Healthcare"},
        new SelectListItem() { Text="Infrastructure", Value="Infrastructure"},
        new SelectListItem() { Text="Education", Value="Education"}
        };

        [Display(Name = "Supporting Information 1")]
        public string supportingText1 { get; set; }

        [Display(Name = "Supporting Information 2")]
        public string supportingText2 { get; set; }

        [Display(Name = "Supporting Information 3")]
        public string supportingText3 { get; set; }

        [Display(Name = "Supporting Information 4")]
        public string supportingText4 { get; set; }
    }
}