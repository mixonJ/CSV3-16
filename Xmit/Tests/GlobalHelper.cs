using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using OracleFirewall.Controllers;

namespace Tests {
    public static class GlobalHelper {
        public static HttpRequestMessage GetRequestObject(RemoteController controller){
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            return controller.Request;
        }
    }
}
