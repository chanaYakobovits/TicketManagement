using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Common.DTO;

namespace Data
{
  

        public interface IBaseRepository<T> : IDisposable where T : class

        {

            T Create(T newItem);



            T Get(int num);



            T Update(T newItem);



            T Find(object id);



            IEnumerable<T> GetAll();



            IQueryable<T> Where(Expression<Func<T, bool>> predicate);



            bool Delete(int id);



            int GetNextId();

        T GetById(object id);
        Task<List<UserDTO>> GetAllUsers();

        }

}
