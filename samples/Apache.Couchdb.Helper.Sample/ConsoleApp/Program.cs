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

            // Add document data
            Console.WriteLine("Create documents");
            addOneRecord();
            addMutipleRecords();

            // Update document data
            Console.WriteLine("Update documents");
            updateOneRecord();
            updateMutipleRecords();

            // Query load and select documents
            Console.WriteLine("Query load and select documents to application objects");
            queryFilter("true");  // add filter condition in mango query
            queryFilter("false"); // not add filter condition in mango query
            queryNoFilter();
            queryNoParam();
            queryWithStatuses();

            // Views query load and select documents
            Console.WriteLine("Query load and select documents USING VIEWS to application objects");
            viewWithKeysAndPathParameter();
            viewWithPathParameter();
            viewWithNoParameter();
            viewWithNoParameterWithKeys();

            // Read documents from type object
            Console.WriteLine("Load ALL documents of a type");
            readAllDocumentsFromType();

            // Delete documents
            Console.WriteLine("Delete documents");
            //deleteOneRecord();
            //deleteMutipleRecords();

            Console.ReadKey();

        }

        static void queryNoFilter()
        {

            Console.WriteLine("queryNoFilter");

            IList<User> users;

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("list-all-no-parameters", new { id = "OwnerIdemail@email.com" });
                users = db.List<User>(query);
            }

            Array.ForEach(users.ToArray(), Console.WriteLine);

            Console.WriteLine("=====================");

        }

        static void queryFilter(string addFilter)
        {
            Console.WriteLine($"queryFilter {addFilter}");

            IList <User> users;

            string sid = "OwnerIdemail@email.com";
            if(addFilter.Equals("true"))
            {
                sid = "OwnerIdloop.user.9";
            } else
            {
                sid = "OwnerIdloop.user.3";
            }

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("list-all", new { id = sid, addFilter = Boolean.Parse(addFilter), filter = "(?i)loop.user" }); // (?i) is 'like sql equivalent' regex expression 
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
                var query = db.FindOf("list-status", new { id = "OwnerIdloop.user.7", statuses = sts });
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
                    keys = new String[] { "email@email.com", "email@email.com" }
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

        static void viewWithNoParameterWithKeys()
        {
            Console.WriteLine("viewWithNoParameterWithKeys");

            IList<User> users;

            using (UserRepository db = new UserRepository())
            {
                var query = db.FindOf("view2", new
                {
                    // keys filter view
                    keys = new String[] { "agape.evendas@gmail.com", "root@localhost" }
                });
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

        /// <summary>
        /// Add ONE record document in database
        /// </summary>
        static void addOneRecord()
        {
            Console.WriteLine("addOneRecord");

            User user = createUser("email@email.com");

            using (UserRepository db = new UserRepository())
            {
                var result = db.Insert<User>(user); // add document and return instance changed with operation revision id
                Console.WriteLine(result.Revision);
                Console.WriteLine(result.Id);
            }

            Console.WriteLine("=====================");
        }

        /// <summary>
        /// Add mutiple documents in database
        /// </summary>
        static void addMutipleRecords()
        {
            Console.WriteLine("addMutipleRecords");

            var users = new List<User>();
            for(int i = 0; i < 10; i++)
            {
                users.Add(createUser($"loop.user.{i}"));
            }

            using (UserRepository db = new UserRepository())
            {
                var unitofwork = db.Insert<User>(users);

                // unitofwork contains the return of the operation of each record added
                Console.WriteLine($"Contain error: {unitofwork.HasError()}"); ; // status that indicates if there was an error

                Console.WriteLine("Listing errors:");
                // Access records that have errors to be processed
                Array.ForEach(unitofwork.Errors.ToArray(), Console.WriteLine);

                Console.WriteLine("Listing success:");
                // Access records that have been successfully processed
                Array.ForEach(unitofwork.Success.ToArray(), Console.WriteLine);

                Console.WriteLine("Sent items:");
                // Access the original items sent for operation in the database
                Array.ForEach(unitofwork.Items.ToArray(), Console.WriteLine);


            }

            Console.WriteLine("=====================");
        }

        static void updateOneRecord()
        {
            Console.WriteLine("updateOneRecord");

            using (UserRepository db = new UserRepository())
            {
                // Load document data by ID
                var user = db.Get<User>("email@email.com");
                user.Name = user.Name + "::CHANGED";

                var result = db.Update<User>(user); // update document and return instance changed with operation revision id
                Console.WriteLine(result.Revision);
            }

            Console.WriteLine("=====================");
        }

        /// <summary>
        /// Updates a group of documents at once in the database.Remember that CouchDb does not implement ACID properties.
        /// </summary>
        static void updateMutipleRecords()
        {
            Console.WriteLine("updateMutipleRecords");                       

            using (UserRepository db = new UserRepository())
            {

                // Loads all documents of a type
                var users = db.GetAllOf<User>();
                users.ForEach(u => u.Name = u.Name + "::CHANGED");

                // Send update command with data to will be update
                var unitofwork = db.Update<User>(users);

                // unitofwork contains the return of the operation of each record added
                Console.WriteLine($"Contain error: {unitofwork.HasError()}"); ; // status that indicates if there was an error

                Console.WriteLine("Listing errors:");
                // Access records that have errors to be processed
                Array.ForEach(unitofwork.Errors.ToArray(), Console.WriteLine);

                Console.WriteLine("Listing success:");
                // Access records that have been successfully processed
                Array.ForEach(unitofwork.Success.ToArray(), Console.WriteLine);

                Console.WriteLine("Sent items:");
                // Access the original items sent for operation in the database
                Array.ForEach(unitofwork.Items.ToArray(), Console.WriteLine);


            }

            Console.WriteLine("=====================");
        }

        static void deleteOneRecord()
        {
            Console.WriteLine("deleteOneRecord");

            using (UserRepository db = new UserRepository())
            {
                // Load document data by ID
                var user = db.Get<User>("email@email.com");

                var result = db.Delete<User>(user); // delete document from database. Return true case sucess or false case not deleted
                Console.WriteLine($"Sucesso: {result}");
            }

            Console.WriteLine("=====================");
        }

        static void deleteMutipleRecords()
        {
            Console.WriteLine("deleteMutipleRecords");

            using (UserRepository db = new UserRepository())
            {

                // Loads all documents of a type
                var users = db.GetAllOf<User>();

                // Send update command with data to will be deleted
                var unitofwork = db.Delete<User>(users);

                // unitofwork contains the return of the operation of each record added
                Console.WriteLine($"Contain error: {unitofwork.HasError()}"); ; // status that indicates if there was an error

                Console.WriteLine("Listing errors:");
                // Access records that have errors to be processed
                Array.ForEach(unitofwork.Errors.ToArray(), Console.WriteLine);

                Console.WriteLine("Listing success:");
                // Access records that have been successfully processed
                Array.ForEach(unitofwork.Success.ToArray(), Console.WriteLine);

                Console.WriteLine("Sent items:");
                // Access the original items sent for operation in the database
                Array.ForEach(unitofwork.Items.ToArray(), Console.WriteLine);

            }

            Console.WriteLine("=====================");
        }

        static User createUser(string id)
        {
            return new User {
                Id = id,
                Name = $"User Name for {id}",
                AcctId = $"ACCT-{id}",
                AssetIam = $"AssetIam{id}",
                DocType = DocType.USER,
                Email = $"email::{id}",
                OwnerId = $"OwnerId{id}",
                Serial = $"Serial-{id}",
                SourceId = $"SRC:{id}",
                Status = Status.ACTIVE
            };
        }

    }

}
