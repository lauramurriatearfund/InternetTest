using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MvcMovie.Models
{
    public class Evaluation
    {
        public Evaluation()
        {
            Connectivities = new List<CheckBoxListItem>();
            Connectivities.Add(new CheckBoxListItem() { ID = 1, Display = Resources.Resources.Dialup, IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 2, Display = Resources.Resources.GPRS, IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 3, Display = Resources.Resources.ThreeG, IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 4, Display = Resources.Resources.FourG, IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 5, Display = Resources.Resources.LowSpeedBroadband, IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 6, Display = Resources.Resources.HighSpeedBroadband, IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 7, Display = Resources.Resources.SatellitePhone, IsChecked = false });
            Connectivities.Add(new CheckBoxListItem() { ID = 8, Display = Resources.Resources.SMS, IsChecked = false });
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Partner partner { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "ProjectName")]
        public string projectName { get; set; }

        [Display(Name = "Project Reference")]
        public string projectRef { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "Theme")]
        public string theme { get; set; }

        [Display(Name = "Submission Date")]
        public DateTime submissionDate { get; set; }

        [Required]
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