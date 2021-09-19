using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
	public interface IHddMetricsRepository : IRepository<HddMetric>
	{

	}
	public class HddMetricsRepositorySQLite : IHddMetricsRepository
	{
		private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
		// инжектируем соединение с базой данных в наш репозиторий через конструктор
		public int Create(HddMetric item)
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			// прописываем в команду SQL запрос на вставку данных
			var commandText = "INSERT INTO hddmetrics(value, time) VALUES(@value, @time); SELECT last_insert_rowid();";
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
			var commandText = "DELETE FROM hddmetrics WHERE id=@id";
			using var cmd = new SQLiteCommand(commandText, connection);

			cmd.Parameters.AddWithValue("@id", id);
			cmd.Prepare();
			cmd.ExecuteNonQuery();
		}

		public void Update(HddMetric item)
		{
			using var connection = new SQLiteConnection(ConnectionString);

			// прописываем в команду SQL запрос на обновление данных
			var commandText = "UPDATE hddmetrics SET value = @value, time = @time WHERE id=@id;";
			using var cmd = new SQLiteCommand(commandText, connection);
			cmd.Parameters.AddWithValue("@id", item.Id);
			cmd.Parameters.AddWithValue("@value", item.Value);
			cmd.Parameters.AddWithValue("@time", item.Time.Ticks);
			cmd.Prepare();

			cmd.ExecuteNonQuery();
		}

		public IList<HddMetric> GetAll()
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			// прописываем в команду SQL запрос на получение всех данных из таблицы
			var commandText = "SELECT * FROM hddmetrics";
			using var cmd = new SQLiteCommand(commandText, connection);

			var returnList = new List<HddMetric>();

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// пока есть что читать -- читаем
				while (reader.Read())
				{
					// добавляем объект в список возврата
					returnList.Add(new HddMetric
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

		public IList<HddMetric> GetByTimePeriod(DateTime from, DateTime to)
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			// прописываем в команду SQL запрос на получение диапозона данных по времени из таблицы 
			var commandText = "SELECT * FROM hddmetrics WHERE time >= @from AND time <= @to";
			using var cmd = new SQLiteCommand(commandText, connection);
			cmd.Parameters.AddWithValue("@from", from.Ticks);
			cmd.Parameters.AddWithValue("@to", to.Ticks);
			cmd.Prepare();

			var returnList = new List<HddMetric>();

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// пока есть что читать -- читаем
				while (reader.Read())
				{
					// добавляем объект в список возврата
					returnList.Add(new HddMetric
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

		public HddMetric GetById(int id)
		{
			using var connection = new SQLiteConnection(ConnectionString);
			connection.Open();

			var commandText = "SELECT * FROM hddmetrics WHERE id=@id";
			using var cmd = new SQLiteCommand(commandText, connection);
			cmd.Parameters.AddWithValue("@id", id);

			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// если удалось что то прочитать
				if (reader.Read())
				{
					// возвращаем прочитанное
					return new HddMetric
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
