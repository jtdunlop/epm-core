using System.Collections.Generic;

namespace Dbsoft.Epm.Web.Controllers.Home
{
    public class DashboardModel
    {
        public DashboardModel()
        {
            DashboardImages = new List<DashboardImage>();
        }
        public string HelpUrl { get; set; }
        public bool IsAdmin { get; set; }
        public string Impersonate { get; set; }
        public IEnumerable<DashboardImage> DashboardImages { get; set; }
    }

    public class DashboardImage
    {
        public string CharacterName { get; set; }
        public string ImageUrl { get; set; }
    }
}

