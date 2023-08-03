using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.Shared.Models;

namespace Principal.Telemedicine.DataConnectors.Models;
public class ApiDbContext:DbContext
{
    public ApiDbContext(DbContextOptions option) : base(option)
    {
            
    }

    [NotMapped]
    public virtual DbSet<VirtualSurgeryBasicOverviewDataModel> VirtualSurgeryBasicOverviewDataModel { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VirtualSurgeryBasicOverviewDataModel>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=tmworkstoresqlserver.database.windows.net;Initial Catalog=VANDA_TEST;Application Name=VANDA_TEST_AZURE;Integrated Security=False;User ID=TM_DB_dev;Password=Ap7M9$eWSj8TUQ734FcGdqnfHkqw$BHf;Persist Security Info=True;Enlist=False;Pooling=True;Min Pool Size=1;Max Pool Size=100;Connect Timeout=45;User Instance=False;MultipleActiveResultSets=True");
              
        }
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

