using MojtabaBookStore.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Models.UnitOfWork
{
    public interface IUnitOfWork
    {
        BookStoreDb Context { get; }
        IBookRepository BookRepository { get; }
        IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity : class;
        Task Commit();
    }
}
