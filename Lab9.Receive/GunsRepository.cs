using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Lab9.Receive
{
	public sealed class GunsRepository : IDisposable
	{
		private readonly IDbConnection _db;

		public GunsRepository(string connectionString)
		{
			_db = new SqlConnection(connectionString);
		}

		public List<Gun> GetAll()
		{
			return _db.Query<Gun>("SELECT * FROM Guns").ToList();
		}

		public void Add(Gun gun)
		{
			string sqlQuery = "INSERT INTO Guns (Model, Handy, Origin, Material, ShootingDistance, EffectiveFiringRange, HasClip, HasSights) " +
						"VALUES(@Model, @Handy, @Origin, @Material, @ShootingDistance, @EffectiveFiringRange, @HasClip, @HasSights)";

			_db.Execute(sqlQuery, gun);
		}

		public void Dispose()
		{
			_db.Close();
		}
	}
}
