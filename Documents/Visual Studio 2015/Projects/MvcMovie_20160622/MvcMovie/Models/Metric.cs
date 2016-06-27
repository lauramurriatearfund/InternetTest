using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{

    public class Metric
    {
        //Constant definitions: 
        //Attribute name is name used within the application
        //Attribute string value is name or value used within the database

        public static readonly string PROGRESS_SCREEN_HOME = "HOME";
        public static readonly string PROGRESS_SCREEN_TEXT = "TEXT";
        public static readonly string PROGRESS_SCREEN_DISPLAY = "DISPLAY";
        public static readonly string PROGRESS_SCREEN_ENTER = "ENTER";
        public static readonly string PROGRESS_SCREEN_SUBMIT = "SUBMIT";
        public static readonly string PROGRESS_SCREEN_VALIDATE = "VALIDATE";
        public static readonly string PROGRESS_SCREEN_UPLOAD = "UPLOAD";
        public static readonly string PROGRESS_SCREEN_SUCCESS = "SUCCESS";

        public static readonly string METRIC_PAGE_LOAD_TIME = "PAGE_LOAD";
        public static readonly string METRIC_PAGE_REACHED = "PAGE_REACHED";
        public static readonly string METRIC_SPEED_TEST_RESULT = "SPEED_TEST";
        public static readonly string METRIC_GEO_LOCATION = "LOCATION";
        public static readonly string METRIC_DEVICE_TYPE = "DEVICE_TYPE";
        public static readonly string METRIC_IP_ADDRESS = "IP_ADDRESS";

        [Required]
        public int ID { get; set; }

        [Required]
        public string SessionID { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public String MetricName { get; set; }

        [Required]
        public String MetricValue { get; set; }
    }
} 