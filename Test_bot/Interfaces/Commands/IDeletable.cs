using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_bot.Interfaces.IServices
{
    public interface IDeletable<T>
    {
        public Task<int> DeleteAsync(T Object);
    }
}
