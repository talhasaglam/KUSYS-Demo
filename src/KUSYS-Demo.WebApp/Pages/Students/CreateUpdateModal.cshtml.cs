using AutoMapper;
using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KUSYS_Demo.WebApp.Pages.Students
{
    public class CreateUpdateModalModel : BasePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public bool ReadOnly { get; set; }

        [BindProperty]
        public CreateUpdateStudentDto Student { get; set; }

        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        public CreateUpdateModalModel(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }
        public async Task OnGetAsync()
        {
            if (Id > 0)
            {
                var studentDto = await _studentService.GetAsync(Id);
                Student = _mapper.Map<CreateUpdateStudentDto>(studentDto);
            }

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync() {

            if (!ModelState.IsValid)
                return InvalidModel();

            if(Id > 0)
            {
                await _studentService.UpdateAsync(Student,Id);
            }
            else
                await _studentService.CreateAsync(Student);

            return NoContent();
        }
    }
}
