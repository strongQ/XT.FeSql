using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using XT.Common.Config;
using XT.Common.Extensions;
using XT.FeSql.Models;

namespace XT.FeSql
{
    public class XTDbContext
    {
        public static event EventHandler<string> AopSqlEvent;
      

        /// <summary>
        /// sql事件
        /// </summary>
        /// <param name="sql"></param>
        public static void LogSql(string sql)
        {
            AopSqlEvent?.Invoke(null, sql);
        }


     


        private readonly IServiceProvider _serviceProvider;

        public XTDbContext(IServiceProvider provider)
        {
           

            _serviceProvider = provider;

        }

      

        #region 实例方法
        /// <summary>
        /// 获取特定数据库实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IFreeSql<T> GetSqlDb<T>() where T:BaseFlag
        {
           return _serviceProvider.GetService<IFreeSql<T>>();
        }
        /// <summary>
        /// 获取默认数据库实例
        /// </summary>
        /// <returns></returns>
        public IFreeSql<OneFlag> GetDb()
        {
            var db= _serviceProvider.GetService<IFreeSql<OneFlag>>();
            return db;
        }


       

        #endregion


        #region 根据实体类生成数据库表

       /// <summary>
       /// 根据实体和表名创建表
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="tableName"></param>
        public void CreateTableByEntity<T>(string tableName) where T : class, new()
        {
            var db = GetDb();
            if (db.DbFirst.ExistsTable(tableName) == false)
            {
                db.CodeFirst.SyncStructure<T>();
            }
        }

       


       

        #endregion
    }
}
