using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration; //.json 

namespace SQLDatataBaseHomework
{
    public static class UICommands
    {
        public static string GetConnectionString(string connectionStringName = "Default")
        {
            var output = "";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);
            return output;
        }

        public static void ReadAllContacts(SqlCrud sql)
        {
            var rows = sql.GetAllPeople();

            foreach (var row in rows)
            {
                Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
            }
        }

        public static void ReadPersonById(SqlCrud sql, int personId)
        {
            var person = sql.GetFullPersonById(personId);

            Console.WriteLine(
                $"{person.BasicPerson.Id} {person.BasicPerson.FirstName} {person.BasicPerson.LastName}");

            foreach (var a in person.Address)
            {
                Console.WriteLine($"{a.StreetName}, {a.City}, {a.ZipCode}");
            }

            foreach (var work in person.Company)
            {
                Console.WriteLine($"They work at {work.CompanyName}");
            }
        }

        public static void UpdatePersonName(SqlCrud sql)
        {
            var person = new PersonModel
            {
                Id = 1,
                FirstName = "Timmy",
                LastName = "Storm"
            };

            sql.UpdatePersonName(person);
        }

        public static void DeleteAddress(SqlCrud sql, int personId, int addressId)
        {
            sql.DeleteAddress(personId, addressId);
        }

        public static void CreateNewPerson(SqlCrud sql)
        {
            FullPersonModel user = new FullPersonModel()
            {
                BasicPerson = new PersonModel()
                {
                    FirstName = "viktor",
                    LastName = "Degray"
                }
            };

            user.Address.Add(new AddressModel() { StreetName = "næstby 1", City = "Rognan", ZipCode = "8082"});

            user.Company.Add(new CompanyModel() { CompanyName = "Degray.LLC"});

            sql.CreatePerson(user);
        }
    }
}
