using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_bot.Interfaces.IServices;
using Test_bot.models;

namespace Test_bot.Interfaces.Respositories
{
    public interface IExamRespository: ICreatable<Exam>, IDeletable<Exam>, IUpdatable<Exam>, IGet<Exam>,IGetAll<IEnumerable<Exam>>
    {

    }
}
