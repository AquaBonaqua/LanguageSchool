using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool.Classes
{

    public partial class Client
    {
        public string LastServiceDate
        {
            get
            {
                var list = AppData.Ent.ClientService.Where(x => x.ClientID == ID).ToList();
                string LastServiceDate = list.LastOrDefault()?.StartTime.ToShortDateString() ?? "Не было посещений";
                return LastServiceDate;
            }
        }

        public int ServiceCount
        {
            get
            {
                var list = AppData.Ent.ClientService.Where(x => x.ClientID == ID).ToList();

                int ServiceCount = list.Count;
                return ServiceCount;
            }
        }
    }


   
}
