using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using BsonDocument = MongoDB.Bson.BsonDocument;

namespace Infrastructure.Repository
{
    public class Repository : IRepository
    {
        protected readonly DbContext Context;
        protected readonly IMongoDatabase MongoDatabase;

        public Repository(DbContext context, IMongoDatabase mongoDatabase)
        {
            Context = context;
            MongoDatabase = mongoDatabase;
        }

        public void Add<T>(T entity) where T : class
        {
            Context.Set<T>().Add(entity);
        }

        public int AddSave<T>(T entity) where T : class
        {
            Context.Set<T>().Add(entity);

            return Save();
        }

        public int AddSaveRange<T>(List<T> entityList) where T : class
        {
            Context.Set<T>().AddRange(entityList);

            return Save();
        }

        public void AddUpdate<T>(T entity) where T : class
        {
            Context.Set<T>().Add(entity);
        }

        public void AddRange<T>(List<T> entityList) where T : class
        {
            Context.Set<T>().AddRange(entityList);
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            return predicate == null ? Context.Set<T>() : Context.Set<T>().Where(predicate).AsQueryable();
        }
        public IQueryable<T> GetWithAcount<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            return predicate == null ? Context.Set<T>().Include("Account") : Context.Set<T>().Include("Account").Where(predicate).AsQueryable();
        }
        public IQueryable<LanguageValue> GetWithLanguageLanguageResource(Expression<Func<LanguageValue, bool>> predicate = null)
        {
            return predicate == null ? Context.Set<LanguageValue>().Include(x=>x.Language).Include(x=>x.LanguageResource) : Context.Set<LanguageValue>().Include(x => x.Language).Include(x => x.LanguageResource).Where(predicate).AsQueryable();
        }

        public List<T> GetMongo<T>( string collectionName, Expression<Func<T, bool>> filter, string sort= "{ }", int skip = 0, int take = 15000 ) where T : class
        {
          
            var builder = Builders<T>.Filter;
            
            var filt = builder.Where(filter);

            try
            {
               return MongoDatabase.GetCollection<T>(collectionName).Find(filt).Sort(sort).Limit(take).Skip(skip).ToList();
            }
            catch
            {
               return MongoDatabase.GetCollection<T>(collectionName).Find(filt).Sort(sort).ToList();
            } 

        }
        public int GetMongoCount<T>(string collectionName, Expression<Func<T, bool>> predicate, string filter) where T : class
        {
        
            var builder = Builders<T>.Filter;
            FilterDefinition<T> filt;
            if (filter.Equals("{}") || string.IsNullOrWhiteSpace(filter))
            {
                filt = builder.Where(predicate);
            }
            else
            {
                filt = builder.Where(predicate) & filter;
            }
            
            try
            {
                return (int)MongoDatabase.GetCollection<T>(collectionName).Find(filt).Count();
            }
            catch
            {
                return (int) MongoDatabase.GetCollection<T>(collectionName).Find(filt).Count();
            }
        }

        public List<MongoResult> GetMongoFails(string collectionName, Expression<Func<MongoResult, bool>> filter, string sort = "{ ResultDate: 0}", int skip = 0, int take = 100) 
        {

            var builder = Builders<MongoResult>.Filter;
            var filt = builder.Where(filter);

            try
            {
                return MongoDatabase.GetCollection<MongoResult>(collectionName).Find(filt).Sort(sort).Limit(take).Skip(skip).ToList();
            }
            catch 
            {
                return MongoDatabase.GetCollection<MongoResult>(collectionName).Find(filt).Sort(sort).ToList();
            }

        }

        public List<MongoResult> GetMongoResults(string collectionName, Expression<Func<MongoResult, bool>> filter, string sort = "{ ResultDate: 0}", string filterStr = "{ }", int skip = 0, int take = 100)
        {
            if (sort.Trim().Length < 1)
            {
                sort = "{ ResultDate: 0}";
            }
            var builder = Builders<MongoResult>.Filter;
            FilterDefinition<MongoResult> filt;
            if (filterStr.Equals("{}") || string.IsNullOrWhiteSpace(filterStr))
            {
                 filt = builder.Where(filter);
            }
            else
            {
                 filt = builder.Where(filter) & filterStr;
            }
          
                        
            try
            {
                return MongoDatabase.GetCollection<MongoResult>(collectionName).Find(filt).Sort(sort).Limit(take).Skip(skip).ToList();
            }
            catch
            {
                return MongoDatabase.GetCollection<MongoResult>(collectionName).Find(filt).Sort(sort).ToList();
            }

        }
        public List<T> GetMongo<T>(string collectionName, Expression<Func<T, bool>> filter, DataSourceLoadOptions options, string sort = "{ ResultDate: 0}", int skip = 0, int take = 1500) where T : class
        {

            var builder = Builders<T>.Filter;           

            var filt = builder.Where(filter);
        
            return MongoDatabase.GetCollection<T>(collectionName).Find(filt).Sort(sort).Limit(take).Skip(skip).ToList();

        }

        public List<MongoResult> GetMong(string collectionName, Expression<Func<MongoResult, bool>> filter)
        {
            return null;
   

        }
        public IMongoQueryable<T> GetMongoIQuerable<T>(string collectionName) where T : class
        {
            return MongoDatabase.GetCollection<T>(collectionName).AsQueryable();
        }
        public List<T> GetMongoAll<T>(string collectionName) where T : class
        {
            var builder = Builders<T>.Filter;
            FilterDefinition<T> filt = builder.Empty;

            return MongoDatabase.GetCollection<T>(collectionName).Find(filt).ToList();
        }


        public IQueryable<T> GetAll<T>() where T : class
        {
            return Context.Set<T>();
        }

        public void Update<T>(T entity) where T : class
        {
            Context.Set<T>().Update(entity);
        }

        public void UpdateRange<T>(List<T> entity) where T : class
        {
            Context.Set<T>().UpdateRange(entity);
        }

        public int UpdateSave<T>(T entity) where T : class
        {
            Context.Set<T>().Update(entity);
            return Save();
        }

        public int UpdateRangeSave<T>(List<T> entity) where T : class
        {
            
            Context.Set<T>().UpdateRange(entity);
            return Save();
        }


        public void Remove<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
        }

        public void RemoveRange<T>(List<T> entity) where T : class
        {
            Context.Set<T>().RemoveRange(entity);
        }

        public int RemoveSave<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
            return Save();
        }

        public int RemoveRangeSave<T>(List<T> entity) where T : class
        {
            Context.Set<T>().RemoveRange(entity);
            return Save();
        }

        public int Save()
        {
            return Context.SaveChanges();
        }

        public MongoStatus UpdateResultMongoDb(List<MongoResult> entity, string entityName)
        {
         
            var updates = new List<WriteModel<MongoResult>>();
            foreach (var appUser in entity)
            {
                var filter =
                    Builders<MongoResult>.Filter.Eq(x => x.ResultId, appUser.ResultId);
        
                var bsonDoc = appUser.ToBsonDocument();
                var updateDefinition = new UpdateDefinitionBuilder<MongoResult>().Unset("______"); // HACK: I found no other way to create an empty update definition
                updateDefinition = bsonDoc.Elements.Where(element => element.Name != "_id" && element.Value != BsonNull.Value).Aggregate(updateDefinition, (current, element) => current.Set(element.Name, element.Value));
                var update = new UpdateOneModel<MongoResult>(filter, updateDefinition) { IsUpsert = true };
                updates.Add(update);
            }

            var dbStatus = MongoDatabase.GetCollection<MongoResult>("Results").BulkWrite(updates);
            var updatedInserted = new MongoStatus {Inserted = dbStatus.Upserts.Count, Updated = dbStatus.MatchedCount};
            return updatedInserted;
        }

    
        public MongoStatus UpdateMongo<T>(List<T> entity, string entityName) where T : class
        {
            var filter = Builders<T>.Filter;
            FilterDefinition<T> filt = filter.Empty;

          //  var updates = new List<WriteModel<T>>();
            foreach (var appUser in entity)
            {

           //     var bsonDoc = appUser.ToBsonDocument();
               var col = MongoDatabase.GetCollection<T>(entityName);
                col.InsertOne(appUser);
                //  var updateDefinition = new UpdateDefinitionBuilder<T>().Unset("______"); // HACK: I found no other way to create an empty update definition
                //    updateDefinition = bsonDoc.Elements.Where(element => element.Name != "_id" && element.Value != BsonNull.Value).Aggregate(updateDefinition, (current, element) => current.Set(element.Name, element.Value));
                // var update = new UpdateOneModel<T>(filt, updateDefinition) { IsUpsert = true };
                // updates.Add(update);
            }

        //    var dbStatus = MongoDatabase.GetCollection<T>(entityName).BulkWrite(updates);
           var updatedInserted = new MongoStatus { Inserted = 1, Updated = 1 };
            return updatedInserted;
        }
    }   

    public static class AsyncCursorSourceExtensions
    {
        public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(
            this IAsyncCursorSource<T> asyncCursorSource) =>
            new AsyncEnumerableAdapter<T>(asyncCursorSource);

        private class AsyncEnumerableAdapter<T> : IAsyncEnumerable<T>
        {
            private readonly IAsyncCursorSource<T> _asyncCursorSource;

            public AsyncEnumerableAdapter(IAsyncCursorSource<T> asyncCursorSource)
            {
                _asyncCursorSource = asyncCursorSource;
            }

            public IAsyncEnumerator<T> GetEnumerator() =>
                new AsyncEnumeratorAdapter<T>(_asyncCursorSource);
        }

        private class AsyncEnumeratorAdapter<T> : IAsyncEnumerator<T>
        {
            private readonly IAsyncCursorSource<T> _asyncCursorSource;
            private IAsyncCursor<T> _asyncCursor;
            private IEnumerator<T> _batchEnumerator;

            public T Current => _batchEnumerator.Current;

            public AsyncEnumeratorAdapter(IAsyncCursorSource<T> asyncCursorSource)
            {
                _asyncCursorSource = asyncCursorSource;
            }

            public async Task<bool> MoveNext(CancellationToken cancellationToken)
            {
                if (_asyncCursor == null)
                {
                    _asyncCursor = await _asyncCursorSource.ToCursorAsync(cancellationToken);
                }

                if (_batchEnumerator != null &&
                    _batchEnumerator.MoveNext())
                {
                    return true;
                }

                if (_asyncCursor != null &&
                    await _asyncCursor.MoveNextAsync(cancellationToken))
                {
                    _batchEnumerator?.Dispose();
                    _batchEnumerator = _asyncCursor.Current.GetEnumerator();
                    return _batchEnumerator.MoveNext();
                }

                return false;
            }

            public void Dispose()
            {
                _asyncCursor?.Dispose();
                _asyncCursor = null;
            }
        }
    }


}