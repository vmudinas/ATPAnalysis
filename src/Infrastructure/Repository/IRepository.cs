using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Infrastructure.ClientEntities;
using Infrastructure.MongoEntities;
using MongoDB.Driver.Linq;
using Infrastructure.Entities;

namespace Infrastructure.Repository
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void AddUpdate<T>(T entity) where T : class;
        int AddSave<T>(T entity) where T : class;
        int AddSaveRange<T>(List<T> entityList) where T : class;
        void AddRange<T>(List<T> entityList) where T : class;

        List<T> GetMongo<T>(string collectionName, Expression<Func<T, bool>> filter, string sort = "{ ResultDate: 1}", int skip = 0, int take = 500) where T : class;

        List<MongoResult> GetMong(string collectionName, Expression<Func<MongoResult, bool>> filter);

        List<MongoResult> GetMongoFails(string collectionName, Expression<Func<MongoResult, bool>> filter,
            string sort = "{ ResultDate: 0}", int skip = 0, int take = 50);

        IQueryable<LanguageValue> GetWithLanguageLanguageResource(Expression<Func<LanguageValue, bool>> predicate = null);
        List<MongoResult> GetMongoResults(string collectionName, Expression<Func<MongoResult, bool>> filter, string sort = "{ ResultDate: 0}", string filterStr = "{ }", int skip = 0, int take = 100);
        List<T> GetMongo<T>(string collectionName, Expression<Func<T, bool>> filter, DataSourceLoadOptions options, string sort = "{ ResultDate: 1}", int skip = 0, int take = 500) where T : class;

       int GetMongoCount<T>(string collectionName, Expression<Func<T, bool>> predicate, string filter) where T : class;

        IMongoQueryable<T> GetMongoIQuerable<T>(string collectionName) where T : class;

        MongoStatus UpdateMongo<T>(List<T> entity, string entityName) where T : class;
        IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : class;

        IQueryable<T> GetWithAcount<T>(Expression<Func<T, bool>> predicate = null) where T : class;

        IQueryable<T> GetAll<T>() where T : class;
        void Update<T>(T entity) where T : class;
        void UpdateRange<T>(List<T> entity) where T : class;
        List<T> GetMongoAll<T>(string collectionName) where T : class;

        int UpdateSave<T>(T entity) where T : class;
        int UpdateRangeSave<T>(List<T> entity) where T : class;

        void Remove<T>(T entity) where T : class;
        void RemoveRange<T>(List<T> entity) where T : class;
        int RemoveSave<T>(T entity) where T : class;
        int RemoveRangeSave<T>(List<T> entity) where T : class;
        MongoStatus UpdateResultMongoDb(List<MongoResult> entity, string entityName);
        int Save();
    }
}