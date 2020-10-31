using CouchDbRepository.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Repositories
{
    /// <summary>
    /// Create a repository to represent the database informed
    /// in context.Extend the CouchDb Helper repository and tell 
    /// the constructor what context will be used for this created repository.
    /// </summary>
    public class UserRepository: CouchRepository
    {
        public UserRepository() : base("users-db") { } //users-db is context name defined in appsettings.json
    }
}
