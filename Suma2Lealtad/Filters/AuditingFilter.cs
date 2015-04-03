using Suma2Lealtad.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Suma2Lealtad.Filters
{
    public class AuditingFilter : ActionFilterAttribute, IActionFilter
    {


    void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext){

        if (HttpContext.Current.Session["login"] == null)
        {

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = "SessionExpired",
                controller = "Error",
                area = ""
            }));

        }
        else
        {
            using (LealtadEntities db = new LealtadEntities())
            {

                Auditing log = new Auditing()
                {
                    id = db.Auditings.Max(x => x.id) + 1,
                    objectid = 1,
                    operationsid = 1,
                    userid = (int) HttpContext.Current.Session["userid"],     
                    creationdate = filterContext.HttpContext.Timestamp,
                    originaldata = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    changedata = filterContext.ActionDescriptor.ActionName,
                    ipaddress = filterContext.HttpContext.Request.UserHostAddress
                };

                db.Auditings.Add(log);
                db.SaveChanges();

            }

        }

        this.OnActionExecuting(filterContext);

    }


    }

}