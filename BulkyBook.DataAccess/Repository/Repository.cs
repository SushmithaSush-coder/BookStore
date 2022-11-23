using BookStore.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        //creating instance db set

        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
         //   _db.Products.Include(u => u.Category).Include(u => u.CoverType);
            this.dbSet=_db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        //include prop-"category,covertype
        public IEnumerable<T> GetAll(string? incudeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(incudeProperties!= null)
            {
                foreach(var incudeProp in incudeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                    {
                    query = query.Include(incudeProp);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? incudeProperties = null)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);
            if (incudeProperties != null)
            {
                foreach (var incudeProp in incudeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incudeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
