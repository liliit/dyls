

using System;
using DYLS.Common.Utils;
using DYLS.IDal;
using DYLS.Model.Db;

using Microsoft.AspNetCore.Mvc;

namespace DYLS.ManageApiController.Generic
{
    /// <summary>
    /// Controller 泛型 通用基类, 写一些通用的方法 ,  里面包含基本的增删改查
    /// </summary>
    [Route("mapi/[controller]/[action]")]
    public class SystemBase<TModel, TIntarface> : Controller where TModel : DbBaseModel where TIntarface : class, IDalBase<TModel>
    {

        /// <summary>
        /// 通用添加
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Restore([FromBody] TModel obj)
        {
            return Restore<TIntarface, TModel>(obj);
        }

        /// <summary>
        /// 还原数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        protected virtual ActionResult Restore<I, Model>([FromBody] Model obj) where Model : DbBaseModel where I : class, IDalBase<Model>
        {
            if (obj == null)
            {
                return JsonResultHelper.InvalidParameter();
            }

            var dal = DalFactory.GetInstance<I>();
            var result = dal.Update(obj);
            if (result > 0)
            {
                return JsonResultHelper.Success();
            }
            return JsonResultHelper.Fail();
        }

        /// <summary>
        /// 更新orderId
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult UpdateSort([FromBody] dynamic obj)
        {
            return UpdateSort<TIntarface, TModel>(obj);
        }


        /// <summary>
        /// 用在子类中直接调取
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="Model"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual ActionResult UpdateSort<I, Model>([FromBody] dynamic obj) where Model : DbBaseModel where I : class, IDalBase<Model>
        {
            var id = (long)obj.id;
            var val = (long)obj.val;
            var result = DalFactory.GetInstance<I>().UpdateSort(id, val);
            if (result > 0)
            {
                return JsonResultHelper.Success();
            }
            return JsonResultHelper.Fail();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Delete([FromBody] dynamic obj)
        {
            var id = (long)obj.id;
            return Delete<TIntarface, TModel>(id);
        }

        /// <summary>
        /// 用在子类中直接调取
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="Model"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual ActionResult Delete<I, Model>([FromBody] long id) where Model : DbBaseModel where I : class, IDalBase<Model>
        {
            var result = DalFactory.GetInstance<I>().DeleteById(id);
            if (result > 0)
            {
                return JsonResultHelper.Success();
            }
            return JsonResultHelper.Fail();
        }

        /// <summary>
        /// 通用添加
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Add([FromBody] TModel obj)
        {
            return Add<TIntarface, TModel>(obj);
        }

        /// <summary>
        /// 用在子类中直接调取
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="Model"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual ActionResult Add<I, Model>([FromBody] Model obj) where Model : DbBaseModel where I : class, IDalBase<Model>
        {
            if (obj == null)
            {
                return JsonResultHelper.InvalidParameter();
            }
            //检测对象是否是拥有排序值, 如果有的话, 需要设置一下
            var dal = DalFactory.GetInstance<I>();
            var result = dal.Insert(obj);
            if (result > 0)
            {
                return JsonResultHelper.Success(result);
            }
            return JsonResultHelper.Fail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Published([FromBody] TModel obj)
        {
            return Published<TIntarface, TModel>(obj);
        }

        /// <summary>
        /// 用在子类中直接调取
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="Model"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual ActionResult Published<I, Model>([FromBody] Model obj) where Model : DbBaseModel where I : class, IDalBase<Model>
        {
            if (obj == null)
            {
                return JsonResultHelper.InvalidParameter();
            }
            var dal = DalFactory.GetInstance<I>();
            var result = dal.Update(obj);
            if (result > 0)
            {
                return JsonResultHelper.Success(result);
            }
            return JsonResultHelper.Fail();
        }

        /// <summary>
        /// 更新整个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Update([FromBody] TModel obj)
        {
            return Update<TIntarface, TModel>(obj);
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Load([FromBody] dynamic obj)
        {
            return Load<TIntarface, TModel>(obj);
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="Model"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual ActionResult Load<I, Model>([FromBody] dynamic obj) where Model : DbBaseModel where I : class, IDalBase<Model>
        {
            if (obj == null)
            {
                return JsonResultHelper.InvalidParameter();
            }
            return JsonResultHelper.Success(DalFactory.GetInstance<I>().GetById((long)obj.id));
        }



        /// <summary>
        /// 用在子类中直接调取
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="Model"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual ActionResult Update<I, Model>([FromBody] Model obj) where Model : DbBaseModel where I : class, IDalBase<Model>
        {
            if (obj == null)
            {
                return JsonResultHelper.InvalidParameter();
            }
            var result = DalFactory.GetInstance<I>().Update(obj);
            if (result > 0)
            {
                return JsonResultHelper.Success();
            }
            return JsonResultHelper.Fail();
        }

    }
}
