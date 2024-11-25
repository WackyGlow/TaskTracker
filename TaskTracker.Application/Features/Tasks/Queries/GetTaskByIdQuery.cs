using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Features.Tasks.Queries
{
    public class GetTaskByIdQuery : IRequest<TaskItemDto>
    {
        public int Id { get; set; }
        public GetTaskByIdQuery(int id)
        {
            Id = id;
        }
    }
}
