

using DataAccessLibrary;

namespace SQLDatataBaseHomework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlCrud sql = new SqlCrud(UICommands.GetConnectionString());

            //UICommands.ReadAllContacts(sql);

            //UICommands.ReadPersonById(sql, 3);

            //UICommands.UpdatePersonName(sql); // this will update id 1 first name and last name

            //UICommands.DeleteAddress(sql, 3,2);

            UICommands.CreateNewPerson(sql);

            Console.WriteLine("Done processing");
            Console.ReadLine();
        }
    }
}
