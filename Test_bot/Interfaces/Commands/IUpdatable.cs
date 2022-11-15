using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_bot.models;

namespace Test_bot.Interfaces.IServices
{
    public interface IUpdatable<T>
    {
        public Task<int> UpdateAsync(T Object);
    }
}
