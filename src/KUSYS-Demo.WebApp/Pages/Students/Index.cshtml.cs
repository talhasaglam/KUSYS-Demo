using KUSYS_Demo.Application.Pagination;
using KUSYS_Demo.Application.Services.Interfaces;
using KUSYS_Demo.WebApp.DataTables;
using Microsoft.AspNetCore.Mvc;

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
            var searchText = DataTablesRequest.Search.Value?.ToUpper();
            var pagePropDto = new GetPagePropDto() { SearchText= searchText, Skip= DataTablesRequest.Start, Take= DataTablesRequest.Length };
            var pagedResultDto = await _studentService.GetPagedListAsync(pagePropDto);
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
