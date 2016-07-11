using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MvcMovie.Models;

namespace MvcMovie.DAL
{
    public class PartnerDbInitialiser : System.Data.Entity.DropCreateDatabaseIfModelChanges<PartnerDbContext>
    {
        protected override void Seed(PartnerDbContext context)
        {
            var partners = new List<Partner>
            {
            //new Partner{partnerName="TestPartner1",partnerRef="P125234"}  //,
                                                                           //new Partner{PartnerName="TestPartner2",PartnerRef="P13245567",CreatedDate=DateTime.Parse("2005-09-01")},
                                                                           //new Partner{PartnerName="TestPartner3",PartnerRef="P12346754567",CreatedDate=DateTime.Parse("2005-09-01")},
                                                                           //new Partner{PartnerName="TestPartner4",PartnerRef="76353322345",CreatedDate=DateTime.Parse("2005-09-01")},
                                                                           //};
            };
        partners.ForEach(p => context.Partners.Add(p));
            context.SaveChanges();
        }
    }
}
