using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Application.Services.Interfaces;
using KUSYS_Demo.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace KUSYS_Demo.WebApp.Pages.Students
{
    public class AddCourseModalModel : BasePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public bool IsSaveButtonDisabled { get; set; }

        [BindProperty]
        public AddStudentCourseDto Input { get; set; }

        public List<SelectListItem> CourseNames { get; set; }

        private readonly ICourseService _courseService;

        private readonly IStudentService _studentService;
        public AddCourseModalModel(ICourseService courseService, IStudentService studentService)
        {
            _courseService = courseService;
            _studentService = studentService;
        }
        private async Task GetCourseNames()
        {
            CourseNames = new();
            var corusesDto = await _courseService.GetListAsync();
            var studentwithDetailsDto = await _studentService.GetWithDetailsAsync(Id);
            var studentCourseIds = studentwithDetailsDto.Courses.Select(x => x.CourseId);
            var courses = corusesDto.Where(x => !studentCourseIds.Contains(x.CourseId));
            CourseNames = courses.Select(x => new SelectListItem()
            {
                Text = x.CourseName,
                Value = x.CourseId.ToString()
            }).ToList();

            IsSaveButtonDisabled = CourseNames.Count == 0;
        }
        public async Task OnGetAsync()
        {
            await GetCourseNames();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _studentService.SetCoruseAsync(Id, Input.Id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Input.Id", ex.Message);
                await GetCourseNames();
                return InvalidModel();
            }

            return NoContent();
        }
    }
}
