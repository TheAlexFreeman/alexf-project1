using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WebStore.Data.Entities;


namespace WebStore.Data.Repositories
{
    /// <summary>
    /// Abstract class to store common repository functionality
    /// </summary>
    public abstract class Repository //<T> where T : class //: IRepository<T> where T : class, ISearchable
    {
        protected readonly Project0DBContext _dbContext;

        public Repository(Project0DBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "DB Context cannot be null.");
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
