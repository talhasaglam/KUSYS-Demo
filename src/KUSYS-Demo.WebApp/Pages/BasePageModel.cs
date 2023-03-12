using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KUSYS_Demo.WebApp.Pages
{
    public class BasePageModel : PageModel
    {
        protected PageResult InvalidModel() => new() { StatusCode = 444 };
    }
}
