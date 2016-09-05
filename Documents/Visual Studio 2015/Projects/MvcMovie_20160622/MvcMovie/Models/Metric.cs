using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


        public static readonly string PAGE_LOAD_TIME_TEXT = "TEXT";
        public static readonly string PAGE_LOAD_TIME_DISPLAY = "DISPLAY";
        public static readonly string PAGE_LOAD_TIME_ENTER = "ENTER";
        public static readonly string PAGE_LOAD_TIME_SUBMIT = "SUBMIT";
        public static readonly string PAGE_LOAD_TIME_VALIDATE = "VALIDATE";
        public static readonly string PAGE_LOAD_TIME_UPLOAD = "UPLOAD";
        public static readonly string PAGE_LOAD_TIME_SUCCESS = "SUCCESS";


        public static readonly string METRIC_PAGE_REACHED = "PAGE_REACHED";
        public static readonly string METRIC_SPEED_TEST_RESULT = "SPEED_TEST";
        public static readonly string METRIC_GEO_LOCATION = "LOCATION";
        public static readonly string METRIC_DEVICE_TYPE = "DEVICE_TYPE";
        public static readonly string METRIC_IP_ADDRESS = "IP_ADDRESS";
        public static readonly string METRIC_USER_AGENT = "USER_AGENT";
        public static readonly string METRIC_LANGUAGE = "USER_LANGUAGE";
        public static readonly string METRIC_COUNTRY_FROM_IP = "COUNTRY_FROM_IP";
        public static readonly string METRIC_CITY_FROM_IP = "CITY_FROM_IP";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserSessionId  { get; set;}

        public virtual UserSession UserSession { get; set; }

        
        public DateTime? Timestamp { get; set; }

        [Required]
        public String MetricName { get; set; }

        [Required]
        public String MetricValue { get; set; }
    }
} 