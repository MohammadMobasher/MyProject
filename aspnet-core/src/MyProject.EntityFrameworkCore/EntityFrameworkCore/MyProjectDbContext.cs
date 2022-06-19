using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MyProject.Authorization.Roles;
using MyProject.Authorization.Users;
using MyProject.MultiTenancy;
using System.Linq.Expressions;
using System;
using Abp.Domain.Entities;

namespace MyProject.EntityFrameworkCore
{
    public class MyProjectDbContext : AbpZeroDbContext<Tenant, Role, User, MyProjectDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public MyProjectDbContext(DbContextOptions<MyProjectDbContext> options)
            : base(options)
        {
        }


        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            Expression<Func<TEntity, bool>> expression = base.CreateFilterExpression<TEntity>();

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                //if (this._httpContextAccessor.HttpContext.Request.Headers.Keys.Contains("IsDeleted"))
                {
                    Expression<Func<TEntity, bool>> deletedFilter = e => ((ISoftDelete)e).IsDeleted == false;
                    expression = expression == null ? deletedFilter : CombineExpressions(expression, deletedFilter);
                }
            }

            return expression;
        }
    }
}
