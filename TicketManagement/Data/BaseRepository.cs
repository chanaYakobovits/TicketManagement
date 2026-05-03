using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using Common.Models;
using Common.DTO;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Data;


    public class BaseRepository<TEntity> where TEntity : class

        {

        private readonly AppDbContext context;

        public virtual IQueryable<TEntity> Items { get; set; }



            public BaseRepository(AppDbContext context)

            {
                this.context = context;
                Items = context.Set<TEntity>().AsQueryable();
            }


   
    public virtual async Task<EntityEntry<TEntity>> Add(TEntity entity, bool AllowAnonymous = false)
        {
            try
            {
                var res = await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving entity: " + ex.Message);
                throw; // אל תשכח לזרוק שוב, כדי שה-API יחזיר 500 אמיתי עם פרטים
            }
        

    }

 
    public virtual async Task<EntityEntry<TEntity>> Update(TEntity entity)

            {
            var key = GetKey();
            var keyValue = key.FieldInfo.GetValue(entity);

            var curEntity = context.Set<TEntity>().Find(keyValue);
            context.Entry(curEntity).State = EntityState.Detached;
            Console.WriteLine($"[Before Save] Status = {entity}");

            var updatedEntry = context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync(); 
            Console.WriteLine($"[After Save] Status = {updatedEntry.Entity}");

            return updatedEntry;

            }

            public virtual async Task<EntityEntry<TEntity>> Delete(TEntity entity)

            {

                var result= context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
                return result;

        }



        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))

            {

                return context.SaveChangesAsync(cancellationToken);

            }



            //public async Task<EntityEntry<TEntity>> AddOrUpdate(TEntity entity, Guid curUserId)

            //{

            //    var key = GetKey();

            //    var keyValue = key.FieldInfo.GetValue(entity);

            //    if (key.ClrType == typeof(Guid) && IsNew((Guid?)keyValue) || keyValue == null || (key.ClrType == typeof(int) && (int)keyValue == 0))

            //    {

            //        if (key.ClrType == typeof(Guid) && IsNew((Guid?)keyValue))

            //            key.FieldInfo.SetValue(entity, Guid.NewGuid());

            //        return Add(entity, curUserId);

            //    }



            //    return Update(entity, curUserId);

            //}



            private IProperty GetKey(IEntityType entityType = null)

            {

                if (entityType == null)

                    entityType = context.Model.FindEntityType(typeof(TEntity));

                return entityType.FindPrimaryKey().Properties[0];

            }
        public virtual async Task<TEntity> GetById(object id)
        {
            try
            {
                var entity = await context.Set<TEntity>().FindAsync(id);
                if (entity == null)
                {
                    Console.WriteLine($"Entity of type {typeof(TEntity).Name} with ID {id} not found.");
                }
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving entity: " + ex.Message);
                throw;
            }
        }


        public bool IsNew(Guid? key)

            {

                return key == null || key == Guid.Empty;

            }



            public virtual void DeleteRange(IEnumerable<TEntity> entities)

            {

                context.Set<TEntity>().RemoveRange(entities);

            }

        }

    

