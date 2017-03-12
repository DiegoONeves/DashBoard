using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infra.DataAccess
{
    public class MongoRepository
    {
        IMongoClient _client;
        IMongoDatabase _db;

        public MongoRepository()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _db = _client.GetDatabase("reportsdb");
        }

        public void Add(Report doc)
        {
            _db.GetCollection<Report>(typeof(Report).Name).InsertOne(doc);
        }

        public void Update(Report doc, string id)
        {
            _db.GetCollection<Report>(typeof(Report).Name).ReplaceOne(x => x.Id == new ObjectId(id), doc);
        }

        public IEnumerable<Report> Find(Expression<Func<Report, bool>> predicate = null)
        {
            return _db.GetCollection<Report>(typeof(Report).Name).Find(predicate).ToList();
        }

    }
}
