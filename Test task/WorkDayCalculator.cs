using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest
{
    public class WorkDayCalculator : IWorkDayCalculator
    {
        public DateTime Calculate(DateTime startDate, int dayCount, WeekEnd[] weekEnds)
        {
            if (weekEnds != null)
            {
                DateTime end = startDate.AddDays(dayCount - 1);
                for (int i = 0; i < weekEnds.Length; i++)
                {
                    DateTime d0 = weekEnds[i].StartDate;
                    DateTime d1 = weekEnds[i].EndDate;
                    if (d0 > end || d1 < startDate) continue;
                    else if (d0 < startDate)
                    {
                        dayCount = (d1 - startDate).Days + 1;
                        end = end.AddDays(dayCount);
                    }
                    else
                    {
                        dayCount = (d1 - d0).Days + 1;
                        end = end.AddDays(dayCount);
                    }
                }
                return end;
            }
            else
            {
                DateTime endDate = startDate;
                endDate = endDate.AddDays(dayCount - 1);
                return endDate;
            }
            throw new NotImplementedException();
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
