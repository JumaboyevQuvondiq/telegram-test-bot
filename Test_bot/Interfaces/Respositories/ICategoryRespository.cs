using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_bot.Interfaces.IServices;
using Test_bot.models;

namespace Test_bot.Interfaces.Respositories
{
    public interface ICategoryRespository :
        ICreatable<Category>, IDeletable<int>, IUpdatable<Category>, IGet<Category>, IGetAll<IEnumerable<Category>>
    {
    }
}
