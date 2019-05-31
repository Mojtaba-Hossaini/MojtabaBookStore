using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Services
{
    public interface IConvertDate
    {
        DateTime ShamsiToMiladi(string date);

        string MiladiToShamsi(DateTime date, string format);
    }
}
