using System.Web.Mvc;
using System;
using MvcMovie.DAL;
using MvcMovie.Models;

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
            }
            //TODO - use db context to update metrics database

            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_HOME;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //TODO - use db context to update metrics database
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
                //TODO - use db context to update metrics database


                string addressStr = Request.UserHostAddress;
                string osStr = Request.UserAgent;
                string[] langArr = Request.UserLanguages;

                if (addressStr != null)
                {
                    Metric addressMetric = new Metric();
                    addressMetric.MetricName = Metric.METRIC_IP_ADDRESS;
                    addressMetric.MetricValue = addressStr;
                    addressMetric.SessionID = this.Session.SessionID;
                    addressMetric.Timestamp = DateTime.Now;
                    //TODO - use db context to update metrics database
                }

                if (osStr != null) {
                    Metric os = new Metric();
                    os.MetricName = Metric.METRIC_USER_AGENT;
                    os.MetricValue = Metric.PROGRESS_SCREEN_SUBMIT;
                    os.SessionID = this.Session.SessionID;
                    os.Timestamp = DateTime.Now;
                    //use db context to update metrics database
                    metricDb.save(os);

                }

                if (langArr.Length > 0)
                {
                    
                    for (int i=0; i < langArr.Length;  i++) {
                        Metric lang = new Metric();
                        lang.MetricName = Metric.METRIC_LANGUAGE;
                        lang.MetricValue = langArr[i];
                        lang.SessionID = this.Session.SessionID;
                        lang.Timestamp = DateTime.Now;
                        //use db context to update metrics database
                        metricDb.save(lang);
                        

                    }

                }
                //string geolocStr = Request.U
                //navigator.geolocation.getCurrentPosition

            }
            
            return View();
        }

        // 
        // GET: /HelloWorld/Display/ 

        public ActionResult Display()
        {
            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_DISPLAY;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //TODO - use db context to update metrics database
            }
            return View();
        }



        public ActionResult Enter(Partner model)
        {
            //use db context to update partner db
            partnerDb.save(model);

            if (this.Session.SessionID != null)
            {

                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_ENTER;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.save(progress);
            }
            return View();
        }

        // 
        // GET: /HelloWorld/Submit/ 

        public ActionResult Submit(Session model)
        {
            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_SUBMIT;
                progress.SessionID = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.save(progress);

                
            }
            model.submissionDate = DateTime.Today;

            return View(model);
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
                metricDb.save(progress);
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
                metricDb.save(progress);
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
                metricDb.save(progress);


            }

            return View();

        }

    }
}  