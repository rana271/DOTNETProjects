using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedisCacheUtility;

namespace RadisGeneric
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test t = new Test();
            //t.Test_SetValue();
            //t.Test_GetValue();
            DAL dal = new DAL();
            var empList = dal.GetEmployees();
            foreach(var e in empList)
            {
                Console.WriteLine(e.ENAME);
            }
            Console.WriteLine("---");
            var empList2 = dal.GetEmployees();
            foreach (var e in empList2)
            {
                Console.WriteLine(e.ENAME);
            }
            Console.Read();
        }
    }
    class Test
    {
        ICacheProvider _cacheProvider;
        public Test ()
        {
            _cacheProvider = new RedisCacheProvider();
        }
        
        public void Test_SetValue()
        {
            List<Person> people = new List<Person>()
            {
                new Person(1, "Joe", new List<Contact>()
                {
                    new Contact("1", "123456789"),
                    new Contact("2", "234567890")
                })
            };

            _cacheProvider.Set("People", people);
        }
        public void Test_GetValue()
        {
            var persons = _cacheProvider.Get<List<Person>>("People");

           foreach(var p in persons)
            {
                Console.WriteLine(p.Name);
                foreach(var contact in p.Contacts)
                {
                    Console.WriteLine(contact.Value);
                }
            }
        }

    }
    public class Contact
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public Contact(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
    }

    public class Person
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<Contact> Contacts { get; set; }

        public Person(long id, string name, List<Contact> contacts)
        {
            this.Id = id;
            this.Name = name;
            this.Contacts = contacts;
        }
    }
}
