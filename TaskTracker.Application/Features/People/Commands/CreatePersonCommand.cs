using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Application.Features.People.Commands
{
    public class CreatePersonCommand
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
