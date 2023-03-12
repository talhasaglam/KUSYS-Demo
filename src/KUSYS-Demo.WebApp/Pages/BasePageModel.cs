using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KUSYS_Demo.WebApp.Pages
{
    public class BasePageModel : PageModel
    {
        protected PageResult InvalidModel() => new() { StatusCode = 444 };

        protected NoContentResult NoContent()
        {
            return new NoContentResult();
        }
    }
}
