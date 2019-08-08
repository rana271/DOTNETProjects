using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheUtility
{
    public class Employee
    {
        public int EID { get; set; }

        public string ENAME { get; set; }
        public int SALARY { get; set; }
        public  string ADDRESS { get; set; }

        
    }
    public class DAL
    {
        ICacheProvider _cacheProvider;
        public DAL()
        {
            _cacheProvider = new RedisCacheProvider();
        }
        public List<Employee> GetEmployees()
        {
            List<Employee> empList = _cacheProvider.Get<List<Employee>>("Employees");
            if (empList != null)
                return empList;
            List<Employee> lstemp = new List<Employee>();
            Employee emp = null;
            // Assume connection is an open SqlConnection.
            SqlConnection connection = new SqlConnection(@"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=AbhijitDB;Integrated Security=True");
            
                connection.Open();
                

                
                // Create a new SqlCommand object.
                using (SqlCommand command = new SqlCommand(
                    "SELECT * FROM dbo.EMPLOYEE",
                    connection))
                {
                
                command.Notification = null;
                // Create a dependency and associate it with the SqlCommand.
                SqlDependency.Start(connection.ConnectionString);
                SqlDependency dependency = new SqlDependency(command);
                    // Maintain the refence in a class member.

                    // Subscribe to the SqlDependency event.
                    dependency.OnChange += new
                       OnChangeEventHandler(OnDependencyChange);

                    // Execute the command.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Process the DataReader.
                        while (reader.Read())
                        {
                            emp = new Employee();
                            emp.EID = Convert.ToInt32(reader["EID"]);
                            emp.ENAME = Convert.ToString(reader["ENAME"]);
                            emp.SALARY = Convert.ToInt32(reader["SALARY"]);
                            emp.ADDRESS = Convert.ToString(reader["ADDRESS"]);
                            lstemp.Add(emp);
                        }
                    }


                }
                //SqlDependency.Stop(connection.ConnectionString);
            
            
            _cacheProvider.Set("Employees", lstemp);
            return lstemp;
        }
        // Handler method
        void OnDependencyChange(object sender,
           SqlNotificationEventArgs e)
        {
            // Handle the event (for example, invalidate this cache entry).
            _cacheProvider.Remove("Employees");
        }

        //void Termination()
        //{
        //    // Release the dependency.
        //    SqlDependency.Stop(connectionString, queueName);
        //}
    }
}
