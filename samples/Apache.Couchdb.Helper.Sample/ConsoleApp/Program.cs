using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using ConsoleApp.Repositories;
using CouchDb.Repository.Helper.Extensions;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             * Indicates the configuration file containing the couchDb
             * access data [appsettings.json], the name of the section 
             * within this file with these data [CouchDbConnections] and 
             * also the file with the commands mango queries find and 
             * view that will be used by the program [mango -queries.xml].
             */
            CouchDbRepositoryExtensions.ConfigureCouchdDbHelper("appsettings.json", "CouchDbConnections", "mango-queries.xml");

            Console.WriteLine("CouchDb Helper Hello World use Sample!");
            
            queryFilter("true");  // add filter condition in mango query
            queryFilter("false"); // not add filter condition in mango query
            queryNoFilter();
            queryNoParam();
            queryWithStatuses();
            viewWithKeysAndPathParameter();
            viewWithPathParameter();
            viewWithNoParameter();
            readAllDocumentsFromType();

            Console.ReadKey();

        }

        static void queryNoFilter()
        {

            Console.WriteLine("queryNoFilter");

            IList<User> users;

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("list-all-no-parameters", new { id = "OwncyaftCh" });
                users = db.List<User>(query);
            }

            Array.ForEach(users.ToArray(), Console.WriteLine);

            Console.WriteLine("=====================");

        }


        static void queryFilter(string addFilter)
        {
            Console.WriteLine($"queryFilter {addFilter}");

            IList <User> users;

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("list-all", new { id = "OwncyaftCh", addFilter = addFilter, filter = "(?i)roo" }); // (?i) is 'like sql equivalent' regex expression 
                users = db.List<User>(query);
            }

            Array.ForEach(users.ToArray(), Console.WriteLine);

            Console.WriteLine("=====================");

        }

        static void queryNoParam()
        {
            Console.WriteLine("queryNoParam");

            IList<User> users;

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("list"); 
                users = db.List<User>(query);
            }

            Array.ForEach(users.ToArray(), Console.WriteLine);

            Console.WriteLine("=====================");

        }

        static void queryWithStatuses()
        {
            Console.WriteLine("queryWithStatuses");

            IList<User> users;

            var sts = new List<String> { "ACTIVE", "LOCKED" };

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("list-status", new { id = "OwncyaftCh", statuses = sts });
                users = db.List<User>(query);
            }

            Array.ForEach(users.ToArray(), Console.WriteLine);

            Console.WriteLine("=====================");

        }

        /// <summary>
        /// Run the view by passing keys and additional parameters according to couchdb documentation
        /// </summary>
        static void viewWithKeysAndPathParameter()
        {
            Console.WriteLine("viewWithKeysAndPathParameter");

            //// view additional params
            //var pathparam = new Dictionary<string, string>
            //{
            //    ["descending"] = "false",
            //    ["include_docs "] = "true"
            //};

            IList<User> users;

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("view", new
                {
                    // keys filter view
                    keys = new String[] { "agape.evendas@gmail.com", "root@localhost" }
                });

                users = db.List<User>(query);
            }

            Console.WriteLine("=====================");

        }

        /// <summary>
        /// Run the view by no passing keys but use additional parameters according to couchdb documentation
        /// </summary>
        static void viewWithPathParameter()
        {
            Console.WriteLine("viewWithParameter");

            // view additional params only
            var pathparam = new Dictionary<string, string>
            {
                ["descending"] = "false",
                ["include_docs "] = "true"
            };

            IList<User> users;

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("view2");
                users = db.List<User>(query, pathparam);
            }

            Console.WriteLine("=====================");

        }

        /// <summary>
        /// Executes the view but does not pass any keys or additional parameters.
        /// </summary>
        static void viewWithNoParameter()
        {
            Console.WriteLine("viewWithNoParameter");

            IList<User> users;

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("view2");
                users = db.List<User>(query);
            }

            Array.ForEach(users.ToArray(), Console.WriteLine);

            Console.WriteLine("=====================");

        }

        static void readAllDocumentsFromType()
        {
            Console.WriteLine("readAllDocumentsFromType");

            IList<User> users;

            using (UserRepository db = new UserRepository())
            {
                /* The document must contain the 'typeDoc' attribute with the same value as the type entered. */
                users = db.GetAllOf<User>(); //Get all doc from type parametized. need 
            }

            Array.ForEach(users.ToArray(), Console.WriteLine);

            Console.WriteLine("=====================");

        }

    }

}
