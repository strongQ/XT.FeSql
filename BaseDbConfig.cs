using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XT.Common.Config;
using XT.FeSql.Models;

namespace XT.FeSql
{
    public static class BaseDbConfig
    {

        
        public static (List<DataBaseOperate> MasterDb, List<DataBaseOperate> SlaveDbs) GetDataBaseOperate(XTDbConfig dbConfig=null)
        {
            return InitDataBaseConn(dbConfig);
        }

        private static (List<DataBaseOperate>, List<DataBaseOperate>) InitDataBaseConn(XTDbConfig dbConfig = null)
        {
            List<DataBaseOperate> masterDbs = new List<DataBaseOperate>();
            var slaveDbs = new List<DataBaseOperate>();
            var allDbs = new List<DataBaseOperate>();


            string path = AppSettings.IsDevelopment ? "appsettings.Development.json" : "appsettings.json";


            if (dbConfig == null)
            {


                var xtconfig = AppSettings.GetObjData<XTDbConfig>("XTDbConfig");

                if (xtconfig == null || xtconfig.Dbs.Count < 1)
                {
                    throw new System.Exception("请确保appsettings.json中配置连接字符串,并设置Enabled为true;");
                }


                allDbs = xtconfig.Dbs;
                //如果开启读写分离
                if (xtconfig.IsReadAndWrite)
                {
                    slaveDbs = allDbs.Where(x => x.DbType == masterDbs[0].DbType && x.IsMain == false && x.Enabled)
                        .ToList();
                    if (slaveDbs.Count < 1)
                    {
                        throw new System.Exception($"请确保存在IsMain=false DbType相同的从库;");
                    }
                }
            }
            else
            {
                allDbs = dbConfig.Dbs;
            }




            masterDbs = allDbs.Where(x => x.IsMain && x.Enabled).ToList();
            if (masterDbs.Count == 0)
            {
                throw new System.Exception($"请确保存在IsMain的数据库;");
            }





            return (masterDbs, slaveDbs);


        }


    }
}
