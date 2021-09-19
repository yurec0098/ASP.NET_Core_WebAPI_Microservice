using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL
{
	public interface ICpuMetricsRepository : IRepository<CpuMetric>
	{

	}

	public class CpuMetricsRepositorySQLite : ICpuMetricsRepository
	{
		private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
		// инжектируем соединение с базой данных в наш репозиторий через конструктор
		public int Create(CpuMetric item)
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			// прописываем в команду SQL запрос на вставку данных
			var commandText = "INSERT INTO cpumetrics(value, time) VALUES(@value, @time); SELECT last_insert_rowid();";
			// создаем команду
			using var cmd = new SQLiteCommand(commandText, connection);

			// добавляем параметры в запрос из нашего объекта
			cmd.Parameters.AddWithValue("@value", item.Value);

			// в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды
			// через свойство
			cmd.Parameters.AddWithValue("@time", item.Time.Ticks);
			// подготовка команды к выполнению
			cmd.Prepare();

			return Convert.ToInt32(cmd.ExecuteScalar());
		}

		public void Delete(int id)
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			// прописываем в команду SQL запрос на удаление данных
			var commandText = "DELETE FROM cpumetrics WHERE id=@id";
			using var cmd = new SQLiteCommand(commandText, connection);

			cmd.Parameters.AddWithValue("@id", id);
			cmd.Prepare();
			cmd.ExecuteNonQuery();
		}

		public void Update(CpuMetric item)
		{
			using var connection = new SQLiteConnection(ConnectionString);

			// прописываем в команду SQL запрос на обновление данных
			var commandText = "UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id;";
			using var cmd = new SQLiteCommand(commandText, connection);
			cmd.Parameters.AddWithValue("@id", item.Id);
			cmd.Parameters.AddWithValue("@value", item.Value);
			cmd.Parameters.AddWithValue("@time", item.Time.Ticks);
			cmd.Prepare();

			cmd.ExecuteNonQuery();
		}

		public IList<CpuMetric> GetAll()
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			// прописываем в команду SQL запрос на получение всех данных из таблицы
			var commandText = "SELECT * FROM cpumetrics";
			using var cmd = new SQLiteCommand(commandText, connection);

			var returnList = new List<CpuMetric>();

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// пока есть что читать -- читаем
				while (reader.Read())
				{
					// добавляем объект в список возврата
					returnList.Add(new CpuMetric
					{
						Id = reader.GetInt32(0),
						Value = reader.GetInt32(1),
						// налету преобразуем прочитанные секунды в метку времени
						Time = new DateTime(reader.GetInt64(2))
					});
				}
			}

			return returnList;
		}

		public IList<CpuMetric> GetByTimePeriod(DateTime from, DateTime to)
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			// прописываем в команду SQL запрос на получение диапозона данных по времени из таблицы 
			var commandText = "SELECT * FROM cpumetrics WHERE time >= @from AND time <= @to";
			using var cmd = new SQLiteCommand(commandText, connection);
			cmd.Parameters.AddWithValue("@from", from.Ticks);
			cmd.Parameters.AddWithValue("@to", to.Ticks);
			cmd.Prepare();

			var returnList = new List<CpuMetric>();

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// пока есть что читать -- читаем
				while (reader.Read())
				{
					// добавляем объект в список возврата
					returnList.Add(new CpuMetric
					{
						Id = reader.GetInt32(0),
						Value = reader.GetInt32(1),
						// налету преобразуем прочитанные секунды в метку времени
						Time = new DateTime(reader.GetInt64(2))
					});
				}
			}

			return returnList;
		}

		public CpuMetric GetById(int id)
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			var commandText = "SELECT * FROM cpumetrics WHERE id=@id";
			using var cmd = new SQLiteCommand(commandText, connection);
			cmd.Parameters.AddWithValue("@id", id);

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// если удалось что то прочитать
				if (reader.Read())
				{
					// возвращаем прочитанное
					return new CpuMetric
					{
						Id = reader.GetInt32(0),
						Value = reader.GetInt32(1),
						Time = new DateTime(reader.GetInt64(2))
					};
				}
				else
				{
					// не нашлось запись по идентификатору, не делаем ничего
					return null;
				}
			}
		}
	}
}
