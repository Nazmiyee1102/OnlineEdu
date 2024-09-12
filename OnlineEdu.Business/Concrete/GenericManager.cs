using OnlineEdu.Business.Abstract;
using OnlineEdu.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Business.Concrete
{
    public class GenericManager<T>(IRepository<T> repository) : IGenericService<T> where T : class
    {
        public int TCount()
        {
            return repository.Count();
        }

        public void TCreate(T entity)
        {
            repository.Create(entity);
        }

        public void TDelete(int id)
        {
            repository.Delete(id);
        }

        public int TFilteredCount(Expression<Func<T, bool>> predicate)
        {
            return repository.FilteredCount(predicate);
        }

        public T TGetByFilter(Expression<Func<T, bool>> predicate)
        {
            return repository.GetByFilter(predicate);           
        }

        public T TGetById(int id)
        {
            return repository.GetById(id);
        }

        public List<T> TGetFilteredList(Expression<Func<T, bool>> predicate)
        {
            return repository.GetFilteredList(predicate);
        }

        public List<T> TGetList()
        {
           return repository.GetList();
        }

        public void TUpdate(T entity)
        {
            repository.Update(entity);
        }
    }
}
