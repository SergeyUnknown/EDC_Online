using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EDC.Models.Repository
{
    public interface IRepository<T>
        where T:class
    {
        IEnumerable<T> SelectAll();//выбрать все
        T SelectByID(params object[] id);//выбрать по ID
        IEnumerable<T> GetManyByFilter(Expression<Func<T, bool>> filter);//выбрать по фильтру
        T Create(T obj);//создать
        void Update(T obj);//обновить
        void Delete(params object[] id);//удалить
        void Save();//сохранить БД
    }
}
