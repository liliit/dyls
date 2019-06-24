using IDal.Base;
using Kachannel.Tencentads.Common.Utils;
using Model.Db.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Model.Pager.Base;
using System.Linq;

namespace Dal.Base
{
    /// <summary>
    /// dal基类
    /// </summary>
    /// <typeparam name="TModel">模型</typeparam>
    public class BaseDal<TModel> : IDalBase<TModel> where TModel : DbBaseModel
    {
        /// <summary>
        /// 当前数据库链接
        /// </summary>
        protected IDbConnection conn = DbHelper.GetCurrentConnection();

        /// <summary>
        /// 表名称
        /// </summary>
        protected string TableName = string.Empty;

        /// <summary>
        /// 可排序的
        /// </summary>
        protected bool Sorted = false;

        /// <summary>
        /// 默认构造方法, 用来确定tablename
        /// </summary>
        protected BaseDal()
        {
            var attributes = typeof(TModel).GetCustomAttributesData();
            foreach (var attribute in attributes)
            {
                if (attribute.ConstructorArguments.Count > 0)
                {
                    TableName = attribute.ConstructorArguments[0].Value.ToString();
                }
            }
        }

        /// <summary>
        /// 执行一条sql语句更新数据
        /// </summary>
        /// <param name="values"></param>
        /// <param name="wheres"></param>
        /// <returns></returns>
        public int Update(string values, string wheres)
        {
            var sql = "UPDATE " + TableName + " SET " + values + " WHERE " + wheres;
            return conn.Execute(sql);
        }

        /// <summary>
        /// 获取分页数据, 复杂版本, 支持join
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="where">查询条件</param>
        /// <param name="param">参数</param>
        /// <param name="join">连接</param>
        /// <param name="orderby">排序</param>
        /// <param name="select">选择什么</param>
        /// <param name="tableIsSubQuery"></param>
        /// <returns></returns>
        public IList<TP> GetByPager<TP>(BasePager pager, string where = "", object param = null, string select = "", string join = "", string orderby = "", string groupby = "", bool tableIsSubQuery = false)
        {
            return DbHelper.GetByPager<TP>(TableName, pager, where, param, select, join, orderby, groupby, tableIsSubQuery);
        }



        /// <summary>
        /// 获取分页数据, 复杂版本, 支持join
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="where">查询条件</param>
        /// <param name="param">参数</param>
        /// <param name="join">连接</param>
        /// <param name="orderby">排序</param>
        /// <param name="select">选择什么</param>
        /// <returns></returns>
        public IList<TModel> GetByPager(BasePager pager, string where = "", object param = null, string select = "", string join = "", string orderby = "", string groupby = "", bool tableIsSubQuery = false)
        {
            return GetByPager<TModel>(pager, where, param, select, join, orderby, groupby, tableIsSubQuery);
        }

        /// <summary>
        /// 更新排序值
        /// </summary>
        /// <returns></returns>
        public int UpdateSort(long id, long val)
        {
            return conn.Execute("update " + TableName + " set Sort=" + val + " where Id=" + id);
        }

        /// <summary>
        /// 统计数据总数
        /// </summary>
        /// <returns></returns>
        public long GetCount()
        {
            var sql = "select count(*) from " + TableName + " where DelFlag=0";
            return conn.QuerySingle<long>(sql);
        }

        /// <summary>
        /// 按条件获取数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public IList<TModel> GetByWhere(string @where, object para = null)
        {
            var sql = "select * from " + TableName + " as obj where " + where;
            return conn.Query<TModel>(sql, para).ToList();
        }

        /// <summary>
        /// 获取整个表中所有数据
        /// </summary>
        /// <returns></returns>
        public IList<TModel> GetAll()
        {
            var sql = "select * from " + TableName;
            return conn.Query<TModel>(sql).ToList();
        }

        /// <summary>
        /// 按id获取某一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bol"></param>
        /// <returns></returns>
        public TModel GetById(long id)
        {
            var sql = "select * from " + TableName + " where ID=" + id;
            return conn.QueryFirstOrDefault<TModel>(sql);
        }

        /// <summary>
        /// 按id获取某一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bol"></param>
        /// <returns></returns>
        public TModel GetById(string id)
        {
            id = id.ToLower();
            var sql = "select * from " + TableName + " where StrId=@StrId";
            var paras = new DynamicParameters();
            paras.Add("StrId", id);
            return conn.QueryFirstOrDefault<TModel>(sql, paras);
        }

        /// <summary>
        /// 插入一个模型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual long? Insert(TModel obj)
        {
            return conn.Insert(obj, GetCurrentTrans());
        }

        /// <summary>
        /// 按id删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteFlag(long id)
        {
            var sql = "update " + TableName + " set DelFlag=1 where ID=" + id;
            return conn.Execute(sql);
        }

        /// <summary>
        /// 按id删除一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(long id)
        {
            var sql = "delete " + TableName + " where ID=" + id;
            return conn.Execute(sql);
        }

        /// <summary>
        /// 更新一个模型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(TModel obj)
        {
            return conn.Update(obj, GetCurrentTrans());
        }


        /// <summary>
        /// 开启一个事务, 并保存在上下文中
        /// </summary>
        protected void BeginTransaction()
        {
            DbHelper.GetCurrentTransaction();
        }

        /// <summary>
        /// 获取当前上下文中的事务
        /// </summary>
        /// <returns></returns>
        protected IDbTransaction GetCurrentTrans()
        {
            return DbHelper.GetCurrentTransaction(false);
        }

    }
}
