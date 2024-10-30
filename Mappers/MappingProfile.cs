using AutoMapper;
using TodoAPI.DTO;
using TodoAPI.Models;

namespace TodoAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Todo, TodoResponseDTO>();
            CreateMap<User, UserResponseDTO>();
           ;
            
        }
    }
}