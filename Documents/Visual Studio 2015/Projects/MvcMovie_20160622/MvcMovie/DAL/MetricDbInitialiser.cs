﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MvcMovie.Models;

namespace MvcMovie.DAL
{
    public class MetricDbInitialiser : System.Data.Entity.DropCreateDatabaseIfModelChanges<MetricDbContext>
    {

        //Seed method not required as database starts empty in both test and production
    }
}
