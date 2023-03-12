using KUSYS_Demo.Application.DataTables;
using KUSYS_Demo.Application.Services;
using KUSYS_Demo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KUSYS_Demo.WebApp.Pages.Students
{
    public class IndexModel : BasePageModel
    {
        private readonly IStudentService _studentService;
        public IndexModel(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [BindProperty]
        public DataTablesRequest DataTablesRequest { get; set; }
        public async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var pagedResultDto = await _studentService.GetPagedListAsync(DataTablesRequest);
            return new JsonResult(new
            {
                Draw = DataTablesRequest.Draw,
                RecordsTotal = pagedResultDto.TotalCount,
                RecordsFiltered = pagedResultDto.RecordCount,
                Data = pagedResultDto.Items
            });
        }
    }
}
