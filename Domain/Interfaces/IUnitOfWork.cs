using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Finalizes the current transaction and persists all staged changes to the database.
        /// </summary>
        /// <param name="cancellationToken">A token to abort the commit process before completion.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
        /// <remarks>
        /// This method should be called only after all business objects have been added or updated. 
        /// If any part of the commit fails, the transaction will typically remain open or be rolled back 
        /// depending on the underlying  provider implementation.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown if there is no active transaction to commit.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the commit is aborted via the cancellation token.</exception>
        Task Commit(CancellationToken cancellationToken);
    }
}
