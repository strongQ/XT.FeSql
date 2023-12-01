# freesql封装库，包含雪花id创建器

# 初始化
1. 使用前需要添加配置（appsettings），配置对象为 XTDbConfig （支持多库切换）
2. 使用时要初始化 
```
Services.AddSingleton(new AppSettings(builder.Environment.IsDevelopment()));
Services.AddXTDbSetup();

// 或者不读取配置文件，直接初始化
Services.ADDXTDbSetup(new XT.FeSql.Models.XTDbConfig())
```
# 使用方式
1. 注入XTDbContext对象
2. GetDb()获取FreeSql实例
3. 支持最多3个主库，通过 GetSqlDb<OneFlag> GetSqlDb<TwoFlag> 切换


#  数据库连接串
- Oracle:
```
DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.0.1)(PORT=21)))(CONNECT_DATA=(SERVICE_NAME=XX)));USER ID=XX;Password=XX
```
- PostgreSql:
```
Host=192.168.0.1 Port=20; Database=XX; Username=XX; Password=XX;
```
- Sqlite
```
DataSource=./test.db
```