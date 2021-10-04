using MetricsAgent.DB;
using MetricsAgent.DB.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Controllers
{
	public abstract class BaseController<TEntity> : ControllerBase where TEntity : BaseEntity
	{
		private readonly ILogger<ControllerBase> _logger;
		private IDbRepository<TEntity> _repository;

		protected BaseController(ILogger<ControllerBase> logger, IDbRepository<TEntity> repository)
		{
			_repository = repository;
			_logger = logger;
		}

		[HttpPost("create")]
		public Task Add([FromBody] TEntity request)
		{
			return _repository.AddAsync(request);
		}

		[HttpGet("all")]
		public Task<List<TEntity>> GetAll()
		{
			return _repository.GetAll().ToListAsync();
		}

		[HttpGet("from/{fromTime}/to/{toTime}")]
		public Task<List<TEntity>> GetByTimePeriod([FromRoute] DateTime fromTime, [FromRoute] DateTime toTime)
		{
			return _repository.GetAll().Where(x => x.Time >= fromTime.Ticks && x.Time <= toTime.Ticks).ToListAsync();
		}

		[HttpPost("update")]
		public Task Update([FromBody] TEntity request)
		{
			return _repository.UpdateAsync(request);
		}

		[HttpPost("delete")]
		public Task Delete([FromBody] TEntity request)
		{
			return _repository.DeleteAsync(request);
		}

		[HttpGet("id/{id}")]
		public Task<TEntity> GetById([FromRoute] int id)
		{
			return _repository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
