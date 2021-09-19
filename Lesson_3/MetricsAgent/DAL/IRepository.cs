using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();

        IList<T> GetByTimePeriod(DateTime from, DateTime to);

        T GetById(int id);

        int Create(T item);

        void Update(T item);

        void Delete(int id);
    }
}
