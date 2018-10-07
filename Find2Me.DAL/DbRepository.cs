using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.DAL
{
    public class DbRepository<T> where T : class
    {
        protected ApplicationDbContext dbContext;
        public DbRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public IQueryable<T> GetAll()
        {
            return dbContext.Set<T>();
        }

        public T GetSingle(int key)
        {
            return dbContext.Set<T>().Find(key);
        }

        public T GetSingle(string key)
        {
            return dbContext.Set<T>().Find(key);
        }

        public void Insert(T Entity)
        {
            dbContext.Set<T>().Add(Entity);
        }

        public void Insert(List<T> Entities)
        {
            dbContext.Set<T>().AddRange(Entities);
        }

        public void Update(T Entity)
        {
            dbContext.Entry(Entity).State = System.Data.Entity.EntityState.Modified;
        }

        public void Update(List<T> Entities)
        {
            dbContext.Entry(Entities).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(T Entity)
        {
            dbContext.Set<T>().Remove(Entity);
        }

        public void Delete(List<T> Entities)
        {
            dbContext.Set<T>().RemoveRange(Entities);
        }

        public DbRawSqlQuery<T> RawSql(string query, object[] parameters)
        {
            return dbContext.Database.SqlQuery<T>(query, parameters);
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }
    }
}
