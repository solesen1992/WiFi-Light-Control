using Microsoft.AspNetCore.Mvc;
using WEXO_Project_BLL.Facade;
using WEXO_Projekt_DAL.DB;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Projekt_API.Controllers
{
 /**
 * LoginController
 * 
 * Provides an API endpoint for user login functionality.
 * Accepts login credentials and delegates authentication logic to the APIFacade.
 * Returns HTTP 200 OK on successful login and HTTP 401 Unauthorized on failure.
 */

    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {

        private readonly IAPIFacade _facade;

        public LoginController(IAPIFacade apifacade)
        {
            _facade = apifacade;
        }

        [HttpPost]
            public ActionResult<LoginUser> Post([FromBody] LoginUser loginUser)
            {
                bool res = _facade.Login(loginUser);
                if (res)
                {
                    return Ok(); 
                } else
                {
                    return Unauthorized();
                }
            }

        }
    }
