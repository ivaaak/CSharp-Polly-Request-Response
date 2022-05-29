using Microsoft.AspNetCore.Mvc;

namespace ResponseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        // calling the API (/api/response) with a number - successPercent 
        // between 0 and 100 (over 100 is always successful)
        // the higher the number is the more likely the response is to be successful
        [Route("{successPercent}")]
        public ActionResult GiveAResponse(int successPercent)
        {
            Random rnd = new Random();
            int rndInteger = rnd.Next(1, 101);
            
            if (rndInteger >= successPercent)
            {
                Console.WriteLine($"-> Return 500");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                Console.WriteLine($"-> Return 200");
                return Ok();
            }
        }
        
        // /api/response/90 gives ~~ 90% Ok responses
        // /api/response/10 gives ~~ 10% Ok responses
    }
}
