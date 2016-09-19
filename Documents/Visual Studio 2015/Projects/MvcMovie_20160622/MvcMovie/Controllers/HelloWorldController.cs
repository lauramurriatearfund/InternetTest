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
        private UserSessionDbContext sessionDb = new UserSessionDbContext();
        private EvaluationDbContext evalDb = new EvaluationDbContext();

        public ActionResult Index()
        {
            //first check whether the user session is already in the database, and if not, add it
            UserSession userSess = sessionDb.UserSessions.Find(this.Session.SessionID);
            if (userSess == null)
            {
                sessionDb.UserSessions.Add(new UserSession(this.Session.SessionID));

                try
                {
                    sessionDb.SaveChanges();
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
                metric.UserSession = new UserSession();
                metric.UserSession.SessionId = this.Session.SessionID;
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

                UserSession userSessionUpdate = new UserSession();
                userSessionUpdate.SessionId = this.Session.SessionID;

                try
                {
                    //check to see whether this session already exists
                    var userSessionExisting = sessionDb.UserSessions.Find(userSessionUpdate.SessionId);
                    //update with latest values
                    sessionDb.Entry(userSessionExisting).CurrentValues.SetValues(userSessionUpdate);
                    sessionDb.SaveChanges();
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



                //string addressStr = Request.UserHostAddress;
                string addressStr = "163.99.8.26";
                string osStr = Request.UserAgent;


                if (addressStr != null)
                {
                    try
                    {
                        Metric addressMetric = new Metric();
                        addressMetric.MetricName = Metric.METRIC_IP_ADDRESS;
                        addressMetric.MetricValue = addressStr;
                        addressMetric.UserSession = new UserSession();
                        addressMetric.UserSession.SessionId = this.Session.SessionID;
                        addressMetric.Timestamp = DateTime.Now;
                        metricDb.Metrics.Add(addressMetric);
                        metricDb.SaveChanges();

                        //use google maps subgurim api against max mind database to find
                        //city and country from ip address
                        //unfound addresses will be null or -- these are screened out
                        Metric cityMetric = new Metric();
                        cityMetric.MetricName = Metric.METRIC_CITY_FROM_IP;
                        cityMetric.UserSession = new UserSession();
                        cityMetric.UserSession.SessionId = this.Session.SessionID;
                        cityMetric.Timestamp = DateTime.Now;
                        Subgurim.Controles.Location loc = LocationHelper.GetCityLocationFromIP(addressStr);
                        logger.Log(Logger.DEBUG, "City returned from subgurim api was: " + loc.city, null);
                        if (loc != null && loc.city != null && loc.city != "  ")
                        {
                            cityMetric.MetricValue = loc.city;
                            metricDb.Metrics.Add(cityMetric);
                            metricDb.SaveChanges();
                        }

                        Metric countryMetric = new Metric();
                        countryMetric.MetricName = Metric.METRIC_COUNTRY_FROM_IP;

                        countryMetric.UserSession = new UserSession();
                        countryMetric.UserSession.SessionId = this.Session.SessionID;
                        countryMetric.Timestamp = DateTime.Now;
                        Subgurim.Controles.Location country = LocationHelper.GetCountryLocationFromIP(addressStr);
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
                    os.UserSession = new UserSession();
                    os.UserSession.SessionId = this.Session.SessionID;
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
                        lang.UserSession.SessionId = this.Session.SessionID;
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
                progress.UserSession.SessionId = this.Session.SessionID;
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

            //capture the user session
            if (this.Session.SessionID != null)
            {
                //store the partner ref in the session for use later 
                //this should also force iis to session track 
                Session["PartnerRef"] = model.PartnerRef;


                //update the existing session record with
                //the newly discovered partner ref
                UserSession userSessionUpdate = new UserSession();
                userSessionUpdate.SessionId = this.Session.SessionID;
                //logger.Log(Logger.DEBUG, "SessionID: " + this.Session.SessionID, null);

                try
                {
                    //check to see whether this session already exists
                    var userSessionExisting = sessionDb.UserSessions.Find(userSessionUpdate.SessionId);
                    //update with latest values
                    sessionDb.Entry(userSessionExisting).CurrentValues.SetValues(userSessionUpdate);
                    sessionDb.SaveChanges();
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
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_ENTER;
                progress.UserSession = new UserSession();
                progress.UserSession.SessionId = this.Session.SessionID;
                logger.Log(Logger.DEBUG, "User Session ID: " + this.Session.SessionID, null);
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
                try
                {
                    partnerDb.SaveChanges();
                    ViewBag.message = string.Format(Resources.Resources.InfoReceived);
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
                progress.UserSession = new UserSession();
                progress.UserSession.SessionId = this.Session.SessionID;
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
                return RedirectToAction("Locate");
            }
            else
            {
                //show same page for re-entry of information
                return View(model);
            }


        }

        [HttpGet]
        public ActionResult Validate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Validate(Models.Location model)
        {

            if (this.Session.SessionID != null)
            {
                Metric progress = new Metric();
                progress.MetricName = Metric.METRIC_PAGE_REACHED;
                progress.MetricValue = Metric.PROGRESS_SCREEN_VALIDATE;
                progress.UserSession = new UserSession();
                progress.UserSession.SessionId = this.Session.SessionID;
                progress.Timestamp = DateTime.Now;

                Metric lat = new Metric();
                lat.MetricName = Metric.METRIC_PAGE_REACHED;
                lat.MetricValue = Metric.PROGRESS_SCREEN_VALIDATE;
                lat.UserSessionId = this.Session.SessionID;
                lat.Timestamp = DateTime.Now;

                Metric longitude = new Metric();
                longitude.MetricName = Metric.METRIC_PAGE_REACHED;
                longitude.MetricValue = Metric.PROGRESS_SCREEN_VALIDATE;
                longitude.UserSessionId = this.Session.SessionID;
                longitude.Timestamp = DateTime.Now;

                //use db context to update metrics database
                try
                {
                    metricDb.Metrics.Add(progress);
                    metricDb.Metrics.Add(lat);
                    metricDb.Metrics.Add(longitude);
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


        [HttpGet]
        public ActionResult Locate()
        {
            logger.Log(Logger.DEBUG, "Get. Did not Capture latitude: " , null);
            return View();
        }

        [HttpPost]
        public ActionResult Locate(Models.Location model)
        {
            logger.Log(Logger.DEBUG, "Captured latitude: " + model.latitude + "  Longitude: " + model.longitude, null);
            return View();
        }


    }
}