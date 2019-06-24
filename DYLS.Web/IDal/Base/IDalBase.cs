

using Model.Db.Base;
using Model.Pager.Base;
using System.Collections.Generic;

namespace IDal.Base
{
    public interface IDalBase<TModel> where TModel : DbBaseModel
    {
        /// <summary>
        /// 获取整个表中所有数据
        /// </summary>
        /// <returns></returns>
        IList<TModel> GetAll();

        /// <summary>
        /// 更新排序值
        /// </summary>
        /// <returns></returns>
        int UpdateSort(long id, long val);

        /// <summary>
        /// 按条件获取数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        IList<TModel> GetByWhere(string where, object para = null);

        /// <summary>
        /// 按id获取某一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TModel GetById(long id);

        /// <summary>
        /// 按字符串id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TModel GetById(string id);

        /// <summary>
        /// 插入一个模型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        long? Insert(TModel obj);

        /// <summary>
        /// 按id删除一条数据(DelFlag=1)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(long id);

        /// <summary>
        /// 按id删除一条数据(物理删除)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteFlag(long id);

        /// <summary>
        /// 更新一个模型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int Update(TModel obj);

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
        IList<TP> GetByPager<TP>(BasePager pager, string where = "",
            object param = null, string select = "", string join = "", string orderby = "", string groupby = "", bool tableIsSubQuery = false);
    }
}
