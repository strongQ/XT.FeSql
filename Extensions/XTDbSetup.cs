using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XT.Common.Config;
using XT.Common.Helpers;
using XT.FeSql.Models;

namespace XT.FeSql.Extensions
{
    public static class XTDbSetup
    {
        public static IServiceCollection AddXTDbSetup(this IServiceCollection services,XTDbConfig xtconfig=null)
        {
            //雪花ID器
            new IdHelperBootstrapper().SetWorkderId(1).Boot();
            if (xtconfig == null)
            {
               
                xtconfig = AppSettings.GetObjData<XTDbConfig>();
            }
            var result=BaseDbConfig.GetDataBaseOperate(xtconfig);

            var masterDbs = result.MasterDb;

            if(masterDbs.Count==0 || masterDbs.Count > 3)
            {
                throw new Exception(" db config must <=3");
            }

            for(int i=0;i<masterDbs.Count;i++)
            {
                if (i == 0)
                {


                    Func<IServiceProvider, IFreeSql<OneFlag>> fsqlOne = r =>
                    {
                        var fsql1 = new FreeSqlBuilder().UseConnectionString(masterDbs[0].DbType, masterDbs[0].ConnectionString).UseNameConvert(FreeSql.Internal.NameConvertType.ToLower)
                            .Build<OneFlag>();
                        if(xtconfig.IsSqlAOP)
                        fsql1.Aop.CurdAfter += Aop_CurdAfter;

                        fsql1.Aop.AuditValue += Aop_AuditValue;
                        return fsql1;
                    };
                    services.AddSingleton<IFreeSql<OneFlag>>(fsqlOne);
                }
                else if (i == 1)
                {
                    Func<IServiceProvider, IFreeSql<TwoFlag>> fsqlTwo = r =>
                    {
                        var fsql2 = new FreeSqlBuilder().UseConnectionString(masterDbs[1].DbType, masterDbs[0].ConnectionString).UseNameConvert(FreeSql.Internal.NameConvertType.ToLower)
                            .Build<TwoFlag>();
                        if (xtconfig.IsSqlAOP)
                            fsql2.Aop.CurdAfter += Aop_CurdAfter;

                        fsql2.Aop.AuditValue += Aop_AuditValue;
                        return fsql2;
                    };
                    services.AddSingleton<IFreeSql<TwoFlag>>(fsqlTwo);
                }
                else if (i == 2)
                {
                    Func<IServiceProvider, IFreeSql<ThreeFlag>> fsqlThree = r =>
                    {
                        var fsql3 = new FreeSqlBuilder().UseConnectionString(masterDbs[2].DbType, masterDbs[0].ConnectionString).UseNameConvert(FreeSql.Internal.NameConvertType.ToLower)
                            .Build<ThreeFlag>();
                        if (xtconfig.IsSqlAOP)
                            fsql3.Aop.CurdAfter += Aop_CurdAfter;

                        fsql3.Aop.AuditValue += Aop_AuditValue;
                        return fsql3;
                    };
                    services.AddSingleton<IFreeSql<ThreeFlag>>(fsqlThree);
                }
            }

            services.AddSingleton<XTDbContext>();
            return services;

           
        }

        private static void Aop_AuditValue(object? sender, FreeSql.Aop.AuditValueEventArgs e)
        {
            if(e.Column.CsType == typeof(long) && e.Column.CsName.ToLower()=="id" && e.Value?.ToString()=="0") 
            {
                e.Value = IdHelper.GetLongId();
            }
        }

        private static void Aop_CurdAfter(object? sender, FreeSql.Aop.CurdAfterEventArgs e)
        {
           var sql= $" FullName:{e.EntityType.FullName} ElapsedMilliseconds:{e.ElapsedMilliseconds}ms, {e.Sql}";
            XTDbContext.LogSql(sql);
        }
    }
}
