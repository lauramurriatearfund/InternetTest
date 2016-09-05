using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcMovie.Models
{
    public class UserSession
    {
        [Key]
        public string SessionId { get; set; }

        public int? PartnerId { get; set; }

        public Partner Partner { get; set; }

        public UserSession() {
        
        }
        
        public UserSession(string SessionId)
        {
            this.SessionId = SessionId;
        }
        
    }
}