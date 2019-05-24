using MojtabaBookStore.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojtabaBookStore.Models.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public BookStoreDb Context { get; }
        private IBookRepository bookRepository;
        public IBookRepository BookRepository
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BooksRepository(Context);
                return bookRepository;
            } }
        public UnitOfWork(BookStoreDb context)
        {
            Context = context;
        }

        public IBaseRepository<TEntity> BaseRepository<TEntity>() where TEntity: class
        {
            IBaseRepository<TEntity> repository = new BaseRepository<TEntity, BookStoreDb>(Context);
            return repository;
        }

        public async Task Commit()
        {
            await Context.SaveChangesAsync();
        }
        
    }
}
