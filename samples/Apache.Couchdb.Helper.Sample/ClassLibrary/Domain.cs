using CouchDbRepository.Document;
using Newtonsoft.Json;
using System;

namespace ClassLibrary
{

    public enum DocType
    {
        USER,
        GROUP
    }

    public enum Status
    {
        PRE_ACTIVE, ACTIVE, INACTIVE, LOCKED, PRE_CANCEL, CANCEL
    }

    /// <summary>
    /// The objects that represent a document must inherit from AbstractDocument 
    /// and set the generic to the type of the object itself. With this, this 
    /// object must not contain the "_id" and "_rev" properties since the inherited 
    /// class contains these implementations and the services related to these 
    /// two attributes.
    /// Only the methods mapped with [JsonProperty] attribute will be persisted in the 
    /// document as well as read and filled in automatically.
    /// </summary>
    public class User: AbstractDocument<User>
    {

        [JsonProperty("sourceId")] //Newtonsoft
        public String SourceId { get; set; }

        [JsonProperty("ownerId")] //Newtonsoft
        public String OwnerId { get; set; }

        [JsonProperty("name")] //Newtonsoft
        public String Name { get; set; }

        [JsonProperty("email")] //Newtonsoft
        public String Email { get; set; }

        [JsonProperty("acctId")] //Newtonsoft
        public String AcctId { get; set; }

        [JsonProperty("docType")] //Newtonsoft
        public DocType DocType { get; set; }

        [JsonProperty("status")] //Newtonsoft
        public Status Status { get; set; }

        [JsonProperty("assetIam")] //Newtonsoft
        public String AssetIam { get; set; }

        [JsonProperty("serial")] //Newtonsoft
        public String Serial { get; set; }

        public override string ToString()
        {
            return $"User data: [SourceID: {SourceId}, OwnerID: {OwnerId}, Name: {Name}, Email: {Email}. AcctId: {AcctId}, docType: {DocType} Type: {TypeDocument}, Status: {Status}, AssetIam: {AssetIam}, Serial: {Serial}]";
        }

    }
}
