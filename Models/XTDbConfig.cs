using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XT.FeSql.Models
{
    /// <summary>
    /// db配置项
    /// </summary>
    public class XTDbConfig
    {
        /// <summary>
        /// 是否监听sql日志
        /// </summary>
        public bool IsSqlAOP { get; set; }
        /// <summary>
        /// 是否读写分离
        /// </summary>
        public bool IsReadAndWrite { get; set; }
        /// <summary>
        /// 所有数据库
        /// </summary>
        public List<DataBaseOperate> Dbs { get; set; }
    }
}
