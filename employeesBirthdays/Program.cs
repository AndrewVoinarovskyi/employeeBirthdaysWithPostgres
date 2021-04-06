using System;
using System.Collections.Generic;
using Npgsql;

namespace employeesBirthdays
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input months how much you want to watch: ");
            int months = Convert.ToInt32(Console.ReadLine());
            Calendar cal = new Calendar();
            cal.WriteEmployees(months);
        }
    }


    class Calendar
    {
        public void WriteEmployees(int months)
        {
            // public Dictionary<string, DateTime> birthdays = new Dictionary<string, DateTime>();
            var connString = "Host=127.0.0.1;Username=employee;Password=andrii;Database=employee";

            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT first_name, last_name, birthday FROM birthdays", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    DateTime today = DateTime.Now;
                    Dictionary<string, DateTime> employees = new Dictionary<string, DateTime>();

                    while (reader.Read())
                    {
                        string fullName = $"{reader.GetString(1)} {reader.GetString(0)}";
                        DateTime birthday = reader.GetDateTime(2);
                        
                        employees.Add(fullName, birthday);
                    }

                    for (int i = 0; i <= months; i++)
                    {
                        DateTime currentMonth = DateTime.Now.AddMonths(i);
                        Console.WriteLine(currentMonth.ToString("MMMM yyyy"));
                        
                        foreach (var employee in employees)
                        {
                            int age = today.Year - employee.Value.Year;
                            if (employee.Value.Date > today.AddYears(-age)) age--;
                            
                            if (DateTime.Now.Month == employee.Value.Month - i)
                            {

                                Console.WriteLine($"({employee.Value.ToString("dd")}) - {employee.Key} ({age} years)");
                            }
                            else continue;
                        }
                    }
                }
            }
        }
    }
}
