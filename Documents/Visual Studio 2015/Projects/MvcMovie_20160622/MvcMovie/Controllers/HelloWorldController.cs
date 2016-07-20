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

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        private Logger logger = Logger.GetLogger;

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

                //this is our first record of the user session so record it in the database
                //as a new user session
                //TODO  - handle scenario where user goes back to the start in the same session
                //as currently a new usersession entry will be created in the database with 
                //duplicate UserSession.SessionID but unique UserSession.ID
                metric.UserSession = new UserSession();
                metric.UserSession.SessionID = this.Session.SessionID;

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


            if (this.Session.SessionID != null)
            {
                Metric metric = new Metric();
                metric.MetricName = Metric.METRIC_PAGE_REACHED;
                metric.MetricValue = Metric.PROGRESS_SCREEN_HOME;
                metric.UserSession = new UserSession();
                metric.UserSession.SessionID = this.Session.SessionID;
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
                progress.UserSession = new UserSession();
                progress.UserSession.SessionID = this.Session.SessionID;
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



                string addressStr = Request.UserHostAddress;
                string osStr = Request.UserAgent;


                if (addressStr != null)
                {
                    try
                    {
                        Metric addressMetric = new Metric();
                        addressMetric.MetricName = Metric.METRIC_IP_ADDRESS;
                        addressMetric.MetricValue = addressStr;
                        addressMetric.UserSession = new UserSession();
                        addressMetric.UserSession.SessionID = this.Session.SessionID;
                        //addressMetric.Timestamp = DateTime.Now;
                        metricDb.Metrics.Add(addressMetric);

                        //use google maps subgurim api against max mind database to find
                        //city and country from ip address
                        //unfound addresses will be null
                        Metric cityMetric = new Metric();
                        cityMetric.MetricName = Metric.METRIC_CITY_FROM_IP;
                        Location loc = LocationHelper.GetCityLocationFromIP(addressStr);
                        if (loc != null && loc.city != null)
                        {
                            cityMetric.MetricValue = loc.city;
                            metricDb.Metrics.Add(cityMetric);
                            metricDb.SaveChanges();
                        }
                        


                        Metric countryMetric = new Metric();
                        countryMetric.MetricName = Metric.METRIC_COUNTRY_FROM_IP;
                        Location country = LocationHelper.GetCountryLocationFromIP(addressStr);
                        if (country != null && loc.countryCode != null)
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
                    os.UserSession = new UserSession();
                    os.UserSession.SessionID = this.Session.SessionID;
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
                        lang.UserSession = new UserSession();
                        lang.UserSession.SessionID = this.Session.SessionID;
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
                progress.UserSession = new UserSession();
                progress.UserSession.SessionID = this.Session.SessionID;
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

            if (this.Session.SessionID != null)
            {

                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_ENTER;
                progress.UserSession = new UserSession();
                progress.UserSession.SessionID = this.Session.SessionID;
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
                partnerDb.Partners.Add(model);
                partnerDb.SaveChanges();
                ViewBag.message = string.Format("Thank you. Your information has been received");
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
            model.submissionDate = DateTime.Today;

            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_SUBMIT;
                progress.UserSession = new UserSession();
                progress.UserSession.SessionID = this.Session.SessionID;
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
                progress.UserSession = new UserSession();
                progress.UserSession.SessionID = this.Session.SessionID;
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

        public ActionResult ViewIt()
        {

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

        public ActionResult ReceivePageLoadTime(long seconds)
        {
            Metric progress = new Metric();
            progress.MetricName = Metric.METRIC_PAGE_LOAD_TIME;
            progress.MetricValue = seconds.ToString();
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

            //redirect back to the page the user came from
            //this is required as the form submission is in the _Layout.cshtml
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }



    }
}