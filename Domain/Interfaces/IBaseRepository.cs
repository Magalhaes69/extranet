using Domain.Models;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T: BaseEntity
    {
        /// <summary>
        /// Prepares a new entity for insertion into Database
        /// </summary>
        /// <param name="entity">The business object to be created</param>
        /// <returns>A task representing the staging operation</returns>
        Task CreateAsync(T entity);

        /// <summary>
        /// Marks an existing entity to be updated in the next commit.
        /// </summary>
        /// <param name="entity">The entity containing the updated values.</param>
        /// <remarks>This change is staged locally and will not persist to SAP until Commit is called.</remarks>
        void UpdateAsync(T entity);
        /// <summary>
        /// Marks an existing entity for removal from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        /// <remarks>The deletion is queued and requires a call to Commit to finalize.</remarks>
        void DeleteAsync(T entity);

        /// <summary>
        /// Retrieves record of type <typeparamref name="T"/> from database.
        /// </summary>
        /// <param name="Id">The unique identifier of the order.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The corresponding <see cref="T"/> if found; otherwise, null.</returns>
        Task<T?> GetByIdAsync(
            Guid Id, 
            CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all records of type <typeparamref name="T"/> from database.
        /// </summary>
        /// <param name="cancellationToken">A token to abort the data retrieval.</param>
        /// <returns>A collection of all entities found in the Service database.</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    }
}
