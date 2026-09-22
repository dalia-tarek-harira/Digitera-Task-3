//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Task_3.Application.Features.Jobs.Commands.CloseJob
//{
//    public class CloseJobCommand
//    {
//    }
//}


using MediatR;

namespace Task_3.Application.Features.Jobs.Commands.CloseJob
{
    public record CloseJobCommand(int JobId) : IRequest<Unit>;
}