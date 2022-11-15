using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_bot.Interfaces.IServices;
using Test_bot.models;

namespace Test_bot.Interfaces.Respositories
{
    public interface IUserExamRespository: ICreatable<UserExam>, IGet<UserExam>, IGetAll<IEnumerable<UserExam>>,IUpdatable<UserExam>
    {
    }
}
