using MetricsAgent.DB.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DB
{
	public interface IDbRepository<TEntity> where TEntity : BaseEntity
	{
		IQueryable<TEntity> GetAll();
		Task AddAsync(TEntity entity);
		Task UpdateAsync(TEntity entity);
		Task DeleteAsync(TEntity entity);
	}
}
