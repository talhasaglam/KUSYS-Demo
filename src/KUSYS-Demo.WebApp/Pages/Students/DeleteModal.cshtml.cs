using AutoMapper;
using KUSYS_Demo.Application.Services.Interfaces;
using KUSYS_Demo.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KUSYS_Demo.WebApp.Pages.Students
{
    public class DeleteModalModel : BasePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        private readonly IStudentService _studentService;
        public DeleteModalModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (Id==0)
                return InvalidModel();

                await _studentService.DeleteAsync(Id);

            return NoContent();
        }
    }
}
