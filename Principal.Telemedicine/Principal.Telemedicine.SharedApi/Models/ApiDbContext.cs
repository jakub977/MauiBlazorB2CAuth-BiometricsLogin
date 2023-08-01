using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.SharedApi.Models;

    public class ApiDbContext:DbContext
    {
        public ApiDbContext(DbContextOptions option) : base(option)
        {


        }

       
        public List<T> ExecSqlQuery<T>(string query)
        {
            using var command = Database.GetDbConnection().CreateCommand();
           
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                Database.OpenConnection();

                List<T> list = new List<T>();
                using (var result = command.ExecuteReader())
                {
                    T obj = default(T);
                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                }
                Database.CloseConnection();
                return list;
           
        }


    }

