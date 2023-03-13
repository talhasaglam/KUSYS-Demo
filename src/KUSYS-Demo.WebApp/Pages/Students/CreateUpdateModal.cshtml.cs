using AutoMapper;
using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
        public CreateUpdateStudentViewModal Input { get; set; }

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
                Input = _mapper.Map<CreateUpdateStudentViewModal>(studentDto);
            }

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync() {

            if (!ModelState.IsValid)
                return InvalidModel();

            var studentDto = _mapper.Map<StudentDto>(Input);

            if(Id > 0)
            {
                await _studentService.UpdateAsync(studentDto, Id);
            }
            else
                await _studentService.CreateAsync(studentDto);

            return NoContent();
        }
    }

    public class CreateUpdateStudentViewModal
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
    }
}
