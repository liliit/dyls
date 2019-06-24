using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Model.Pager.Base;

namespace Kachannel.Tencentads.Common.Utils
{
    public static class DbHelper
    {

        static DbHelper()
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);
        }

        /// <summary>
        /// 取得一个新的连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetNewConnection()
        {
            IDbConnection conn = new SqlConnection(ConfigHelper.Get(ConfigHelper.DbConnection));
            HttpContextHelper.Set(HttpContextHelper.DbConnection, conn);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 获取当前上下文中的数据库连接, 要是没有, 则返回新的
        /// </summary>
        /// <param name="create">如果为true会创建</param>
        /// <param name="type">如果为true会创建</param>
        /// <returns></returns>
        public static IDbConnection GetCurrentConnection(bool news = true)
        {
            IDbConnection conn = HttpContextHelper.Get<IDbConnection>(HttpContextHelper.DbConnection);
            if (conn == null)
            {
                if (news)
                {
                    return GetNewConnection();
                }

                return null;
            }
            return conn;
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
        /// <param name="tableName">表 </param>
        /// <param name="tableIsSubQuery">表为子查寻</param>
        /// <param name="groupby">分组</param>
        /// <param name="type">分组</param>
        /// <returns></returns>
        public static IList<TP> GetByPager<TP>(string tableName, BasePager pager, string where = "", object param = null, string select = "", string join = "", string orderby = "", string groupby = "", bool tableIsSubQuery = false)
        {

            var baseSql = " from " + tableName + (tableIsSubQuery ? "" : " as obj ") +
                          (!string.IsNullOrEmpty(join) ? join : "") + " " +
                          (!string.IsNullOrEmpty(where) ? "where " + where : "") + " ";

            baseSql += string.IsNullOrEmpty(groupby) ? "" : " GROUP BY " + groupby + " ";

            var orderBySql = (!string.IsNullOrEmpty(orderby)
                ? "order by " + orderby
                : "order by obj.ID desc");

            var countSql = "";

            if (string.IsNullOrEmpty(groupby))
            {
                countSql = "select count(*) " + baseSql;
            }
            else
            {
                countSql = "select count(*) from (select " + groupby + " " + baseSql + ") as sub";
            }


            var count = GetCurrentConnection().QueryFirstOrDefault<long>(countSql, param);
            pager.TotalRecord = count;

            if (count > 0)
            {
                if (pager.CurrentPage > pager.TotalPage)
                {
                    pager.CurrentPage = pager.TotalPage;
                }

                var dataSql = "select * from (select ROW_NUMBER() over (" + orderBySql + ") as RowNum, " + (!string.IsNullOrEmpty(select) ? select : "obj.*") + baseSql + ") as sub where sub.RowNum BETWEEN  " + ((pager.CurrentPage - 1) * pager.PageSize + 1) + " and " + ((pager.CurrentPage - 1) * pager.PageSize + pager.PageSize);

                var list = GetCurrentConnection().Query<TP>(dataSql, param).ToList();
                return list;
            }

            return new List<TP>();
        }

        /// <summary>
        /// 获取当前的事务(整个请求全局有效)
        /// </summary>
        /// <param name="createNew"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDbTransaction GetCurrentTransaction(bool createNew = true)
        {
            IDbTransaction transaction = HttpContextHelper.Get<IDbTransaction>(HttpContextHelper.DbTransaction);
            if (transaction == null && createNew)
            {
                var conn = GetCurrentConnection();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                transaction = conn.BeginTransaction();
                HttpContextHelper.Set(HttpContextHelper.DbTransaction, transaction);
            }
            return transaction;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public static void SubmitTrans()
        {
            IDbTransaction transaction = HttpContextHelper.Get<IDbTransaction>(HttpContextHelper.DbTransaction);
            if (transaction != null)
            {
                transaction.Commit();
                HttpContextHelper.Set(HttpContextHelper.DbTransaction, null);
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public static void RollBackTrans()
        {
            IDbTransaction transaction = HttpContextHelper.Get<IDbTransaction>(HttpContextHelper.DbTransaction);
            if (transaction != null)
            {
                transaction.Rollback();
                HttpContextHelper.Set(HttpContextHelper.DbTransaction, null);
            }
        }


        /// <summary>
        /// 执行一条语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="type"></param>
        public static int ExecuteNonQuery(string sql, DynamicParameters paras)
        {
            return GetCurrentConnection().Execute(sql, paras);
        }

        /// <summary>
        /// 关闭当前上下文中的有效数据库连接
        /// </summary>
        public static void CloseCurrentConnection()
        {
            var conn = GetCurrentConnection();
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close(); ;
            }
        }

        /// <summary>
        /// 执行一行语句, 返回datareader
        /// </summary>
        /// <returns></returns>
        public static IDataReader ExecuteReader(string tableName, string where = "", DynamicParameters param = null, string select = "", string join = "", string orderby = "", bool tableIsSubQuery = false)
        {

            var conn = GetCurrentConnection();  //需要新的连接读

            var baseSql = " from " + tableName + (tableIsSubQuery ? "" : " as obj ") +
                          (!string.IsNullOrEmpty(join) ? join : "") + " " +
                          (!string.IsNullOrEmpty(where) ? "where " + where : "") + " ";

            var orderBySql = (!string.IsNullOrEmpty(orderby)
                ? "order by " + orderby
                : "order by obj.ID desc");

            var dataSql = "select ROW_NUMBER() over (" + orderBySql + ") as RowNum, " + (!string.IsNullOrEmpty(select) ? select : "obj.*") + baseSql;

            var sql = dataSql;

            var result = conn.ExecuteReader(sql, param);
            return result;

        }

    }
}
