using System.Web.Http;
using PhonebookApi.Models;

namespace PhonebookApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(new PhonebookDatabaseInitializer());

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
