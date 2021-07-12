using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Extensions
{
    public static class Extension
    {
        public static void AddOrUpdate<TEntity>(this System.Data.Entity.DbContext context, System.Data.Entity.DbContext queryContext, Func<TEntity, Guid> KeyProperty, params TEntity[] entities) where TEntity : class
        {
            var querySet = queryContext.Set(typeof(TEntity));
            var set = context.Set(typeof(TEntity));

            foreach (var entity in entities)
            {
                Guid key = KeyProperty.Invoke(entity);

                context.AddOrUpdate(set, querySet, key, entity);
            }
        }

        public static void AddOrUpdate<TEntity>(this System.Data.Entity.DbContext context, System.Data.Entity.DbContext queryContext, Func<TEntity, int> KeyProperty, params TEntity[] entities) where TEntity : class
        {
            var querySet = queryContext.Set(typeof(TEntity));
            var set = context.Set(typeof(TEntity));

            foreach (var entity in entities)
            {
                int key = KeyProperty.Invoke(entity);

                context.AddOrUpdate(set, querySet, key, entity);
            }
        }

        private static void AddOrUpdate<TEntity>(this System.Data.Entity.DbContext context, System.Data.Entity.DbSet set, System.Data.Entity.DbSet querySet, object key, TEntity entity) where TEntity : class
        {
            var existing = querySet.Find(key);
            set.Add(entity);

            var entry = context.Entry(entity);

            if (existing == null)
            {
                entry.State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                entry.State = System.Data.Entity.EntityState.Modified;
            }
        }
    }
}
