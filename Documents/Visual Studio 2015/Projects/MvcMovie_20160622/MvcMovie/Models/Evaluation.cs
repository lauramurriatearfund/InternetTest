using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcMovie.Models
{
    public class Evaluation
    {
        public Evaluation()
        {
            Connectivities = new List<CheckBoxListItem>();
            Connectivities.Add(new CheckBoxListItem() { ID = 1, Display = "Dialup", IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 2, Display = "GPRS", IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 3, Display = "3G", IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 4, Display = "4G", IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 5, Display = "Low speed broadband", IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 6, Display = "High speed broadband", IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 7, Display = "Satelite phone", IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 8, Display = "SMS", IsChecked = false });
        }

        public int evaluationID { get; set; }

        public Partner partner { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public string projectName { get; set; }

        [Display(Name = "Project Reference")]
        public string projectRef { get; set; }

        [Display(Name = "Theme")]
        public string theme { get; set; }

        [Display(Name = "Submission Date")]
        public DateTime submissionDate { get; set; }

        public List<CheckBoxListItem> Connectivities { get; set; }

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