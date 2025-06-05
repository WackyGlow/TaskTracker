using AutoMapper;
using TaskTracker.Domain.Entities;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Application.Features.Tasks.Dtos;
using TaskTracker.Domain.ValueObjects;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Person
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.Age))
                .ReverseMap(); // No need for Age on reverse map

            // Project
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Contributors, opt => opt.MapFrom(src => src.Contributors))
                .ReverseMap();


            // Recurrence
            CreateMap<Recurrence, RecurrenceDto>()
                .ReverseMap()
                .ConstructUsing(dto => new Recurrence(dto.Interval, Enum.Parse<RecurrenceUnit>(dto.Unit, true)));

            // Category (Value Object as string)
            CreateMap<Category, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, Category>().ConvertUsing(name => Category.FromName(name));

            // Priority (Enum as string)
            CreateMap<Priority, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, Priority>().ConvertUsing(name => Enum.Parse<Priority>(name, true));

            // TaskItem
            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.Recurrence, opt => opt.MapFrom(src => src.Recurrence))
                .ForMember(dest => dest.AssignedPeople, opt => opt.MapFrom(src => src.AssignedPeople))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Category.FromName(src.Category)))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => Enum.Parse<Priority>(src.Priority, true)))
                .ForMember(dest => dest.Recurrence, opt => opt.MapFrom(src => src.Recurrence))
                .ForMember(dest => dest.AssignedPeople, opt => opt.Ignore()); // Or map manually if needed

        }
    }
}