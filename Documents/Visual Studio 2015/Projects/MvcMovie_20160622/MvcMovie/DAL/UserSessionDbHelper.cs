using MvcMovie.Helpers;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace MvcMovie.DAL
{

    public class UserSessionDbHelper
    {
        Logger logger = Helpers.Logger.Get();

        public Boolean UpdateUserSession(string sessionID)
        {
            UserSessionDbContext sessionDb = new UserSessionDbContext();

            logger.Log(Logger.DEBUG, "Checking whether User Session ID: " + sessionID + "exists in database", null);

            //first check whether the user session is already in the database, and if not, add it
            UserSession userSess = sessionDb.UserSessions.Find(sessionID);
            if (userSess == null)
            {
                logger.Log(Logger.DEBUG, "Updating database with User Session ID: " + sessionID, null);
                sessionDb.UserSessions.Add(new UserSession(sessionID));

                try
                {
                    sessionDb.SaveChanges();
                    return true;
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

                    return false;
                }
            }
            else
            {
                return false;
            }


        }

    }
}