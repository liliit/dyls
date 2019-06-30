



using DYLS.IDal;
using DYLS.Model.Db;

namespace DYLS.ManageApiController.Generic
{
    /// <summary>
    /// 泛型类, 继承了这个的所有Controller下的Action只有在登陆后才能访问
    /// </summary>
    //[RequiredLoginFilter]
    public class NeedLogin<TModel, TIntarface> : SystemBase<TModel, TIntarface> where TModel : DbBaseModel where TIntarface : class, IDalBase<TModel>
    {

    }
}
