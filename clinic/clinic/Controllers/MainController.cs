using clinic.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        protected User? AuthUser
        {
            get
            {
                var currentUser = HttpContext.User;

                if (currentUser != null && currentUser.HasClaim(c => c.Type == "UserID"))
                {
                    var userIdClaim = currentUser.FindFirst("UserID");
                    if (userIdClaim != null)
                    {
                        return new User { Id = int.Parse(userIdClaim.Value) };
                    }
                }
                return null;
            }
        }
    }
}
