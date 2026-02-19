using Core.DAO;
using Core.Helpers;
using Core.Observer;
using Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Controllers
{
    public class Controller<T> where T: class, ISerializable, IIdentifiable, new()
    {
        protected readonly DAO<T> _dao;
        public Controller(DAO<T> dao) {
            _dao = dao;
        }

        public List<T> GetAll()
        {
            return _dao.GetAll();
        }

        public T? GetById(int id)
        {
            return _dao.GetById(id);
        }

        public void Add(T entity)
        {
            _dao.Add(entity);
        }

        public void Delete(int id)
        {
            _dao.Remove(id);
        }

        public void Update(T entity)
        {
            _dao.Update(entity);
        }

        public void Subscribe(IObserver observer)
        {
            _dao.Subject.Subscribe(observer);
        }
    }
}
