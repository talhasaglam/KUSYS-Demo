using AutoMapper;
using KUSYS_Demo.Application.DataTables;
using KUSYS_Demo.Application.Dtos.Student;
using KUSYS_Demo.Application.Pagination;
using KUSYS_Demo.Application.Services.Interfaces;
using KUSYS_Demo.DataAccess.Repositories.Interfaces;
using KUSYS_Demo.Entity.Entities;
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
        public async Task CreateAsync(CreateUpdateStudentDto createStudentDto)
        {
            var student = _mapper.Map<Student>(createStudentDto);
            await _studentRepository.InsertAsync(student,true);
        }

        public async Task<StudentDto> GetAsync(int id)
        {
            var student = await _studentRepository.GetAsync(x => x.Id == id);
            return _mapper.Map<StudentDto>(student);
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

            var students = await _studentRepository.GetPagedListAsync(searchText, skip, take);

            var studentSimpleListDto = _mapper.Map<List<StudentSimpleDto>>(students);

            return new PagedResultDto<StudentSimpleDto> { Items = studentSimpleListDto, TotalCount = totalCount, RecordCount = recordCount };
        }

        public async Task UpdateAsync(CreateUpdateStudentDto updateStudentDto)
        {
            var student = _mapper.Map<Student>(updateStudentDto);
            await _studentRepository.UpdateAsync(student, true);
        }
    }
}
