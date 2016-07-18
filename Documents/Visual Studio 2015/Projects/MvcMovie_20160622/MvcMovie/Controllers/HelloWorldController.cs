using System.Web.Mvc;
using System;
using MvcMovie.DAL;
using MvcMovie.Models;
using MvcMovie.Helpers;
using System.Web;
using System.Linq;
using Subgurim.Controles;
using System.Threading;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {

        private PartnerDbContext partnerDb = new PartnerDbContext();
        private MetricDbContext metricDb = new MetricDbContext();
        private UserSessionDbContext sessionDb = new UserSessionDbContext();
        private EvaluationDbContext evalDb = new EvaluationDbContext();

        public ActionResult Index()
        {
            // Update Metrics database with users browser/device type
            string strUserAgent = Request.UserAgent.ToString().ToLower();
            if (strUserAgent != null && this.Session.SessionID != null)
            {
                Metric metric = new Metric();
                metric.MetricName = Metric.METRIC_DEVICE_TYPE;
                metric.MetricValue = strUserAgent;
                metric.Timestamp = DateTime.Now;
                metric.SessionID = this.Session.SessionID;

               
                metricDb.Metrics.Add(metric);
                metricDb.SaveChanges();
                
            }
            

            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_HOME;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                metricDb.Metrics.Add(progress);
                metricDb.SaveChanges();
            }


            return View();
        }

        public ActionResult Text()
        {
            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_TEXT;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                metricDb.Metrics.Add(progress);
                metricDb.SaveChanges();

                string addressStr = Request.UserHostAddress;
                string osStr = Request.UserAgent;
                

                if (addressStr != null)
                {
                    Metric addressMetric = new Metric();
                    addressMetric.MetricName = Metric.METRIC_IP_ADDRESS;
                    addressMetric.MetricValue = addressStr;
                    addressMetric.SessionID = this.Session.SessionID;
                    //addressMetric.Timestamp = DateTime.Now;
                    metricDb.Metrics.Add(addressMetric);

                    //use google maps subgurim api against max mind database to find
                    //city and country from ip address
                    //unfound addresses will be null
                    Metric cityMetric = new Metric();
                    cityMetric.MetricName = Metric.METRIC_CITY_FROM_IP;
                    Location loc = LocationHelper.GetCityLocationFromIP(addressStr);
                    if (loc != null) {
                        cityMetric.MetricValue = loc.city;
                    } else
                    {
                        cityMetric.MetricValue = null;
                    }


                    Metric countryMetric = new Metric();
                    countryMetric.MetricName = Metric.METRIC_COUNTRY_FROM_IP;
                    Location country = LocationHelper.GetCountryLocationFromIP(addressStr);
                    if (country != null)
                    {
                        countryMetric.MetricValue = country.countryCode;
                    }
                    else
                    {
                        countryMetric.MetricValue = null;
                    }

                    //save both metrics to database
                    metricDb.Metrics.Add(cityMetric);
                    metricDb.Metrics.Add(countryMetric);
                    metricDb.SaveChanges();
                }

                if (osStr != null) {
                    Metric os = new Metric();
                    os.MetricName = Metric.METRIC_USER_AGENT;
                    
                    os.MetricValue = Request.UserAgent;
                    os.SessionID = this.Session.SessionID;
                    os.Timestamp = DateTime.Now;
                    //use db context to update metrics database
                    metricDb.Metrics.Add(os);
                    metricDb.SaveChanges();

                }

                string[] langArr = Request.UserLanguages;

                if (langArr.Length > 0)
                {
                    
                    for (int i=0; i < langArr.Length;  i++) {
                        Metric lang = new Metric();
                        lang.MetricName = Metric.METRIC_LANGUAGE;
                        lang.MetricValue = langArr[i];
                        lang.SessionID = this.Session.SessionID;
                        lang.Timestamp = DateTime.Now;
                        //use db context to update metrics database
                        metricDb.Metrics.Add(lang);
                        metricDb.SaveChanges();


                    }

                }
                //string geolocStr = Request.U
                //navigator.geolocation.getCurrentPosition

            }
            
            return View();
        }


        public ActionResult Display()
        {
            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_DISPLAY;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.Metrics.Add(progress);
                metricDb.SaveChanges();
            }
            return View();
        }

        [HttpGet]
        public ActionResult Enter()
        {
       
            return View();
        }

        [HttpPost]
        public ActionResult Enter(Partner model)
        { 
            
            if (this.Session.SessionID != null)
            {

                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_ENTER;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.Metrics.Add(progress);
                metricDb.SaveChanges();
            }

            if (ModelState.IsValid)
            {
                //use db context to update partner db
                model.createdDate = DateTime.Now;
                partnerDb.Partners.Add(model);
                partnerDb.SaveChanges();
                ViewBag.message = string.Format("Thank you. Your information has been received");
                //proceed to next tab/screen on user journey
                return RedirectToAction("Submit");
            } else
            {
                //show same page for re-entry of information
                return View();
            }

            
        }

        [HttpGet]
        public ActionResult Submit()
        {
            var model = new Evaluation();
            model.submissionDate = DateTime.Today;

            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_SUBMIT;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.Metrics.Add(progress);
                metricDb.SaveChanges();
            }

            var selectedConnectivities = model.Connectivities.Where(x => x.IsChecked).Select(x => x.ID).ToList();

            return View(model);

        }


        [HttpPost]
        public ActionResult Submit(Evaluation model)
        {

            var selectedConnectivities = model.Connectivities.Where(x => x.IsChecked).Select(x => x.ID).ToList();
            //todo update database with selected connectivities
            //could cheat and put as metrics - not advisable as user entered not auto captured

            if (ModelState.IsValid)
            {
                //use db context to update evaluation db
                evalDb.save(model);
                ViewBag.message = string.Format("Thank you. Your information has been received");
                //proceed to next tab/screen on user journey
                return RedirectToAction("Validate");
            }
            else
            {
                //show same page for re-entry of information
                return View(model);
            }

            
        }

        public ActionResult Validate()
        {
            ViewBag.formSubmitSuccess = "Your form submission has been successfully received";

            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_VALIDATE;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.SaveChanges();
            }
            return View();
        }

        public ActionResult Upload(Partner model)
        {
            Console.WriteLine("Partner details received: " + model.ToString());
            ViewBag.formSubmitSuccess = "Your form submission has been successfully received";

            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_UPLOAD;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.Metrics.Add(progress);
                metricDb.SaveChanges();
            }
            return View();
        }

        public ActionResult Success()
        {
            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_SUCCESS;
                //use db context to update metrics database
                metricDb.Metrics.Add(progress);
                metricDb.SaveChanges();


            }

            return View();

        }

        public ActionResult ViewIt(){

            return View();

        }

 

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
             
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = System.DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);


            // Modify current thread's cultures   - Could be a duplication since this
            // is also done in BaseController         
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return RedirectToAction("Enter");
        }



    }
}  