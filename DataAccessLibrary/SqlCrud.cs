using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public class SqlCrud
    {
        private readonly string _connectionString;
        private SQLDataAccess db = new SQLDataAccess();

        public SqlCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<PersonModel> GetAllPeople()
        {
            string sql = "select Id, FirstName, LastName from dbo.Persons";
            return db.LoadData<PersonModel, dynamic>(sql, new { }, _connectionString);
        }

        public FullPersonModel GetFullPersonById(int id)
        {
            string sql = $"select Id, FirstName, LastName from dbo.Persons where Id = @Id";
            FullPersonModel output = new FullPersonModel();

            output.BasicPerson = db.LoadData<PersonModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

            if (output.BasicPerson == null)
            {
                // do something here to tell user record was not found
                //throw new Exception("user not found");
                return null;
            }

            sql = @"select a.*
                    from dbo.Addresses a
                    inner join
                    dbo.PersonAddress pa on pa.AddressId = a.Id
                    where pa.PersonId = @Id";

            output.Address = db.LoadData<AddressModel, dynamic>(sql, new { Id = id }, _connectionString);

            sql = @"select c.*
                    from dbo.Companies c
                    inner join
                    dbo.PersonCompany pc on pc.CompanyId = c.Id
                    where pc.PersonId = @Id";

            output.Company = db.LoadData<CompanyModel, dynamic>(sql, new { Id = id }, _connectionString);

            return output;
        }

        public void UpdatePersonName(PersonModel person)
        {
            var sql = "Update dbo.Persons set FirstName = @FirstName, LastName = @LastName where Id = @Id";
            db.SaveData(sql, person, _connectionString);
        }

        public void DeleteAddress(int personId, int addressId)
        {
            var sql = "select Id, PersonId, AddressId from dbo.PersonAddress where AddressId = @AddressId;";
            var links = db.LoadData<PersonAddressModel, dynamic>(
                sql,
                new { AddressId = addressId },
                _connectionString);

            sql = "delete from dbo.PersonAddress where AddressId = @AddressId and PersonId = @PersonId;";
            db.SaveData(sql, new { AddressId = addressId, PersonId = personId }, _connectionString);

            if (links.Count == 1)
            {
                sql = "delete from dbo.Addresses where Id = @AddressId;";
                db.SaveData(sql, new { AddressId = addressId }, _connectionString);
            }
        }

        public void CreatePerson(FullPersonModel person)
        {
            
            var sql = "insert into dbo.Persons (FirstName, LastName) values (@FirstName, @LastName);";
            db.SaveData(sql,
                new { person.BasicPerson.FirstName, person.BasicPerson.LastName }, // same as FirstName = person.x.y
                _connectionString);

            
            sql = "select Id from dbo.Persons where FirstName = @FirstName and LastName = @LastName;";
            var personId = db.LoadData<IdLookUpModel, dynamic>(
                sql,
                new { person.BasicPerson.FirstName, person.BasicPerson.LastName },
                _connectionString).First().Id;


            foreach (var address in person.Address)
            {
                if (address.Id == 0)
                {
                    sql = "insert into dbo.Addresses (StreetName, City, ZipCode) values (@StreetName, @City, @ZipCode);";
                    db.SaveData(sql, new { address.StreetName, address.City, address.ZipCode }, _connectionString);

                    sql = "select Id from dbo.Addresses where StreetName = @StreetName and City = @City and ZipCode = @ZipCode;";
                    address.Id =
                        db.LoadData<IdLookUpModel, dynamic>(
                            sql,
                            new { address.StreetName, address.City, address.ZipCode },
                            _connectionString).First().Id;
                }

                sql = "insert into dbo.PersonAddress (PersonId, AddressId) values (@PersonId, @AddressId);";
                db.SaveData(sql, new { PersonId = personId, AddressId = address.Id }, _connectionString);
                
            }

            foreach (var company in person.Company)
            {
                if (company.Id == 0)
                {
                    sql = "insert into dbo.Companies (CompanyName) values (@CompanyName);";
                    db.SaveData(sql, new { company.CompanyName }, _connectionString);

                    sql = "select Id from dbo.Companies where CompanyName = @CompanyName;";
                    company.Id =
                        db.LoadData<IdLookUpModel, dynamic>(
                                sql,
                                new { company.CompanyName },
                                _connectionString).First().Id;
                }

                sql = "insert into dbo.PersonCompany (PersonId, CompanyId) values (@PersonId, @CompanyId);";
                db.SaveData(sql, new { PersonId = personId, CompanyId = company.Id }, _connectionString);
            }

        }
    }
}
