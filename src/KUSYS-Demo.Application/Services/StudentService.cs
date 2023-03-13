using AutoMapper;
using KUSYS_Demo.Application.DataTables;
using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Application.Pagination;
using KUSYS_Demo.Application.Services.Interfaces;
using KUSYS_Demo.DataAccess.Repositories.Interfaces;
using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(StudentDto createStudentDto)
        {
            var student = _mapper.Map<Student>(createStudentDto);
            await _studentRepository.InsertAsync(student,true);
        }

        public async Task<StudentDto> GetAsync(int id)
        {
            var student = await _studentRepository.GetAsync(x => x.Id == id);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentWithDetailsDto> GetWithDetailsAsync(int id)
        {
            var student = await _studentRepository.GetAsync(x => x.Id == id, x => x.Include(y => y.Courses).ThenInclude(z => z.Course));
            return _mapper.Map<StudentWithDetailsDto>(student);
        }

        public async Task<List<StudentSimpleDto>> GetListAsync()
        {
            var students =  await _studentRepository.GetListAsync();
            return _mapper.Map<List<StudentSimpleDto>>(students);
        }

        public async Task<PagedResultDto<StudentSimpleDto>> GetPagedListAsync(DataTablesRequest dataTablesRequest)
        {
            var searchText = dataTablesRequest.Search.Value?.ToUpper();
            var skip = dataTablesRequest.Start;
            var take = dataTablesRequest.Length;

            var (totalCount, recordCount) = await _studentRepository.CountAsync(searchText, skip, take);

            var students = await _studentRepository.GetPagedListAsync(searchText, skip, take, x=>x.Include(y=>y.Courses).ThenInclude(z=>z.Course));

            var studentSimpleListDto = _mapper.Map<List<StudentSimpleDto>>(students);

            return new PagedResultDto<StudentSimpleDto> { Items = studentSimpleListDto, TotalCount = totalCount, RecordCount = recordCount };
        }

        public async Task UpdateAsync(StudentDto updateStudentDto, int id)
        {
            var student = await _studentRepository.GetAsync(x => x.Id == id);
            if (student is null)
                throw new Exception("Student Not Found");

            var mappedStudent = _mapper.Map(updateStudentDto,student);
            await _studentRepository.UpdateAsync(mappedStudent, true);
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _studentRepository.GetAsync(x => x.Id == id);
            if (student is null)
                throw new Exception("Student Not Found");

            await _studentRepository.DeleteAsync(student,true);
        }

        public async Task SetCoruseAsync(int sutdentId, int courseId)
        {
            var student = await _studentRepository.GetAsync(x => x.Id == sutdentId, x => x.Include(y => y.Courses).ThenInclude(z => z.Course));

            if (student is null)
                throw new Exception("Student Not Found");

            if (student.Courses.Any(x => x.CourseId == courseId))
                throw new Exception("Student already registired this course.");

            student.AddCourse(courseId);

            await _studentRepository.UpdateAsync(student, true);
        }
    }
}
