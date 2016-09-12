using System.Web.Mvc;
using System;
using MvcMovie.DAL;
using MvcMovie.Models;
using MvcMovie.Helpers;
using System.Web;
using System.Linq;
using Subgurim.Controles;
using System.Threading;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using MvcMovie.Resources;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        private Logger logger = Logger.Get();
        private PartnerDbContext partnerDb = new PartnerDbContext();
        private MetricDbContext metricDb = new MetricDbContext();
        //private UserSessionDbContext sessionDb = new UserSessionDbContext();
        private EvaluationDbContext evalDb = new EvaluationDbContext();

        public ActionResult Index()
        {
            //call user session db helper to update UserSession with the current session ID
            Boolean success = (new UserSessionDbHelper()).UpdateUserSession(this.Session.SessionID);

            // Update Metrics database with user's browser/device type
            string strUserAgent = Request.UserAgent.ToString().ToLower();
            if (strUserAgent != null && this.Session.SessionID != null)
            {
                Metric metric = new Metric();
                metric.MetricName = Metric.METRIC_DEVICE_TYPE;
                metric.MetricValue = strUserAgent;
                metric.Timestamp = DateTime.Now;
                metric.UserSessionId = this.Session.SessionID;

                try
                {
                    metricDb.Metrics.Add(metric);
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }
            }


            if (this.Session.SessionID != null)
            {
                Metric metric = new Metric();
                metric.MetricName = Metric.METRIC_PAGE_REACHED;
                metric.MetricValue = Metric.PROGRESS_SCREEN_HOME;
                metric.UserSessionId = this.Session.SessionID;
                metric.Timestamp = DateTime.Now;
                metricDb.Metrics.Add(metric);
                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }

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
                progress.UserSessionId = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                metricDb.Metrics.Add(progress);
                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }

                }

                //call user session db helper to update UserSession with the current session ID
                Boolean success = (new UserSessionDbHelper()).UpdateUserSession(this.Session.SessionID);

                string addressStr = Request.UserHostAddress;
                string osStr = Request.UserAgent;

                if (addressStr != null)
                {
                    try
                    {
                        Metric addressMetric = new Metric();
                        addressMetric.MetricName = Metric.METRIC_IP_ADDRESS;
                        addressMetric.MetricValue = addressStr;
                        addressMetric.UserSessionId = this.Session.SessionID;
                        addressMetric.Timestamp = DateTime.Now;
                        metricDb.Metrics.Add(addressMetric);
                        metricDb.SaveChanges();

                        //use google maps subgurim api against max mind database to find
                        //city and country from ip address
                        //unfound addresses will be null or -- these are screened out
                        Metric cityMetric = new Metric();
                        cityMetric.MetricName = Metric.METRIC_CITY_FROM_IP;
                        cityMetric.UserSessionId = this.Session.SessionID;
                        cityMetric.Timestamp = DateTime.Now;
                        Location loc = LocationHelper.GetCityLocationFromIP(addressStr);
                        logger.Log(Logger.DEBUG, "City returned from subgurim api was: " + loc.city, null);
                        if (loc != null && loc.city != null && loc.city != "  ")
                        {
                            cityMetric.MetricValue = loc.city;
                            metricDb.Metrics.Add(cityMetric);
                            metricDb.SaveChanges();
                        }

                        Metric countryMetric = new Metric();
                        countryMetric.MetricName = Metric.METRIC_COUNTRY_FROM_IP;
                        countryMetric.UserSessionId = this.Session.SessionID;
                        countryMetric.Timestamp = DateTime.Now;
                        Location country = LocationHelper.GetCountryLocationFromIP(addressStr);
                        logger.Log(Logger.DEBUG, "Country returned from subgurim api was: " + country.countryCode, null);
                        if (loc != null && loc.city != null && loc.city != "--")
                            if (country != null && country.countryCode != null && country.city != "--")
                            {
                                countryMetric.MetricValue = country.countryCode;
                                metricDb.Metrics.Add(countryMetric);
                                metricDb.SaveChanges();
                            }


                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                        {
                            // Get entry
                            DbEntityEntry entry = item.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Log error messages
                            foreach (DbValidationError subItem in item.ValidationErrors)
                            {
                                string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                logger.Log(Logger.ERROR, message, ex);
                            }

                            // Rollback changes
                            switch (entry.State)
                            {
                                case EntityState.Added:
                                    entry.State = EntityState.Detached;
                                    break;
                                case EntityState.Modified:
                                    entry.CurrentValues.SetValues(entry.OriginalValues);
                                    entry.State = EntityState.Unchanged;
                                    break;
                                case EntityState.Deleted:
                                    entry.State = EntityState.Unchanged;
                                    break;
                            }
                        }
                    }
                }

                if (osStr != null)
                {
                    Metric os = new Metric();
                    os.MetricName = Metric.METRIC_USER_AGENT;

                    os.MetricValue = Request.UserAgent;
                    os.UserSessionId = this.Session.SessionID;
                    os.Timestamp = DateTime.Now;
                    //use db context to update metrics database
                    metricDb.Metrics.Add(os);


                    try
                    {
                        metricDb.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                        {
                            // Get entry
                            DbEntityEntry entry = item.Entry;
                            string entityTypeName = entry.Entity.GetType().Name;

                            // Log error messages
                            foreach (DbValidationError subItem in item.ValidationErrors)
                            {
                                string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                         subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                logger.Log(Logger.ERROR, message, ex);
                            }

                            // Rollback changes
                            switch (entry.State)
                            {
                                case EntityState.Added:
                                    entry.State = EntityState.Detached;
                                    break;
                                case EntityState.Modified:
                                    entry.CurrentValues.SetValues(entry.OriginalValues);
                                    entry.State = EntityState.Unchanged;
                                    break;
                                case EntityState.Deleted:
                                    entry.State = EntityState.Unchanged;
                                    break;
                            }
                        }
                    }

                }

                string[] langArr = Request.UserLanguages;

                if (langArr.Length > 0)
                {

                    for (int i = 0; i < langArr.Length; i++)
                    {
                        Metric lang = new Metric();
                        lang.MetricName = Metric.METRIC_LANGUAGE;
                        lang.MetricValue = langArr[i];
                        lang.UserSessionId = this.Session.SessionID;
                        lang.Timestamp = DateTime.Now;
                        //use db context to update metrics database
                        metricDb.Metrics.Add(lang);

                        try
                        {
                            metricDb.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                            {
                                // Get entry
                                DbEntityEntry entry = item.Entry;
                                string entityTypeName = entry.Entity.GetType().Name;

                                // Log error messages
                                foreach (DbValidationError subItem in item.ValidationErrors)
                                {
                                    string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                             subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                                    logger.Log(Logger.ERROR, message, ex);
                                }

                                // Rollback changes
                                switch (entry.State)
                                {
                                    case EntityState.Added:
                                        entry.State = EntityState.Detached;
                                        break;
                                    case EntityState.Modified:
                                        entry.CurrentValues.SetValues(entry.OriginalValues);
                                        entry.State = EntityState.Unchanged;
                                        break;
                                    case EntityState.Deleted:
                                        entry.State = EntityState.Unchanged;
                                        break;
                                }
                            }
                        }



                    }

                }
                //string geolocStr = Request
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
                progress.UserSessionId = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.Metrics.Add(progress);

                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }

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
            logger.Log(Logger.DEBUG, "User Session ID: " + this.Session.SessionID, null);

            //capture the user session
            if (this.Session.SessionID != null)
            {
                //store the partner ref in the session for use later 
                //this should also force iis to session track if it is not already
                Session["PartnerRef"] = model.PartnerRef;


                //call user session db helper to update UserSession with the current session ID
                Boolean success = (new UserSessionDbHelper()).UpdateUserSession(this.Session.SessionID);


                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_ENTER;
                progress.UserSessionId = this.Session.SessionID;

                progress.Timestamp = DateTime.Now;

                //use db context to update metrics database
                metricDb.Metrics.Add(progress);
                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                //use db context to update partner db

                model.CreatedDate = DateTime.Now;

                try
                {
                    logger.Log(Logger.DEBUG, "Checking whether Partner: " + model.PartnerName + "exists in database", null);

                    //first check whether the user session is already in the database, and if not, add it
                    Partner partner = partnerDb.Partners.Where(b => b.PartnerName == model.PartnerName).FirstOrDefault();

                    if (partner == null)
                    {
                        logger.Log(Logger.DEBUG, "Updating database with Partner: " + model.PartnerName, null);
                        partner.PartnerRef = model.PartnerRef;
                        partner.CreatedDate = DateTime.Now;
                        partner.PartnerId = model.PartnerId;
                        partner.Country = model.Country;
                        partnerDb.SaveChanges();
                    }
                    else
                    {
                        //add it
                        logger.Log(Logger.DEBUG, "Adding Partner: " + model.PartnerName + " to the database", null);
                        partnerDb.Partners.Add(model);
                        partnerDb.SaveChanges();

                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }
                //proceed to next tab/screen on user journey
                return RedirectToAction("Submit");
            }
            else
            {
                //show same page for re-entry of information
                return View();
            }


        }

        [HttpGet]
        public ActionResult Submit()
        {
            var model = new Evaluation();
            model.submissionDate = DateTime.Now;

            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_SUBMIT;
                progress.UserSessionId = this.Session.SessionID;
                logger.Log(Logger.DEBUG, "SessionID: " + this.Session.SessionID, null);
                progress.Timestamp = DateTime.Now;

                //use db context to update metrics database
                metricDb.Metrics.Add(progress);
                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }
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
                evalDb.Evaluations.Add(model);
                evalDb.SaveChanges();
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
                progress.UserSessionId = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }
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
                progress.UserSessionId = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;
                //use db context to update metrics database
                metricDb.Metrics.Add(progress);
                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }
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
                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }


            }

            return View();

        }

        public ActionResult ReceivePageLoadTime(int? id)
        {
            Metric metric = new Metric();

            int? seconds = id;
            if (seconds != null)
            {
                metric.MetricValue = seconds.ToString();

                //work out the referring page so we can log the page load time of a specific page
                string url = Request.UrlReferrer.ToString();

                string[] keys = new string[] { "Text", "Display", "Enter", "Submit", "Validate", "Update", "Success" };

                string sKeyResult = keys.FirstOrDefault<string>(s => url.Contains(s));

                switch (sKeyResult)
                {
                    case "Text":
                        metric.MetricName = Metric.PAGE_LOAD_TIME_TEXT;
                        break;
                    case "Display":
                        metric.MetricName = Metric.PAGE_LOAD_TIME_DISPLAY;
                        break;
                    case "Enter":
                        metric.MetricName = Metric.PAGE_LOAD_TIME_ENTER;
                        break;
                    case "Submit":
                        metric.MetricName = Metric.PAGE_LOAD_TIME_SUBMIT;
                        break;
                    case "Validate":
                        metric.MetricName = Metric.PAGE_LOAD_TIME_VALIDATE;
                        break;
                    case "Update":
                        metric.MetricName = Metric.PAGE_LOAD_TIME_UPLOAD;
                        break;
                    case "Success":
                        metric.MetricName = Metric.PAGE_LOAD_TIME_SUCCESS;
                        break;

                }

                //use db context to update metrics database
                metricDb.Metrics.Add(metric);
                try
                {
                    metricDb.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry
                        DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Log error messages
                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            logger.Log(Logger.ERROR, message, ex);
                        }

                        // Rollback changes
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.State = EntityState.Detached;
                                break;
                            case EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = EntityState.Unchanged;
                                break;
                            case EntityState.Deleted:
                                entry.State = EntityState.Unchanged;
                                break;
                        }
                    }
                }
            }
            //redirect back to the page the user came from
            //this is required as the form submission is in the _Layout.cshtml
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }


        public ActionResult Locate()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddSpace(RentOutSpace rentModel)
        {
            Session.Add("RentModel" , rentModel);
            
            return View();
        }
    }
}