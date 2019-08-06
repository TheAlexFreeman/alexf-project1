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
    public abstract class Repository
    {
        protected readonly StoreDBContext _dbContext;

        public Repository(StoreDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "DB Context cannot be null.");
        }

        public void Save()
        {
            _dbContext.SaveChanges();
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
