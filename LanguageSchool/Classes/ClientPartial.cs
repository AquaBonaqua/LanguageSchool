using System.Linq;

namespace LanguageSchool.Classes
{
    public partial class Client
    {
        public string LastServiceDate
        {
            get
            {
                var list = AppData.Ent.ClientService.Where(x => x.ClientID == ID).ToList();
                var LastServiceDate = list.LastOrDefault()?.StartTime.ToShortDateString() ?? "Не было посещений";
                return LastServiceDate;
            }
        }

        public int ServiceCount
        {
            get
            {
                var list = AppData.Ent.ClientService.Where(x => x.ClientID == ID).ToList();

                var ServiceCount = list.Count;
                return ServiceCount;
            }
        }
    }
}