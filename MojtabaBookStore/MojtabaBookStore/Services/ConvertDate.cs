using MD.PersianDateTime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Services
{
    public class ConvertDate
    {
        public DateTime ShamsiToMiladi(string date)
        {
            PersianDateTime pd = PersianDateTime.Parse(date);
            return pd.ToDateTime();
        }

        public string MiladiToShamsi(DateTime date, string format)
        {
            PersianDateTime pd = new PersianDateTime(date);
            return pd.ToString(format);
        }
    }
}
