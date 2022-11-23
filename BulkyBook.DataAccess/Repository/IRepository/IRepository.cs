using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{   
    //generic repository
    public interface IRepository<T> where T:class
    {
        //filter -- while retreiving 1 record
        T GetFirstOrDefault(Expression<Func<T, bool>> filter,string? incudeProperties=null);
        IEnumerable<T> GetAll(string? includeProperties=null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}
