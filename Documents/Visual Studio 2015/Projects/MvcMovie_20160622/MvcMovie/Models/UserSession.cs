using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMovie.Models
{
    public class UserSession
    {
        int ID { get; set; }
        string SessionID { get; set; }
        Partner partner { get; set; }
        
    }
}