using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        //different implementation like update will be here
        void Update(Category obj);
        

    }
}
