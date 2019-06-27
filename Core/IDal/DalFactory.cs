

using System;
using System.Reflection;
using Ninject;

namespace DYLS.IDal
{
    public class DalFactory
    {

        /// <summary>
        /// Ninject 核心
        /// </summary>
        private static readonly IKernel _kernel;

        /// <summary>
        /// 构造方法
        /// </summary>
        static DalFactory()
        {
            _kernel = new StandardKernel();
            var idalAssemblyName = "DYLS.IDal";
            var dalAssemblyName = "DYLS.Dal";
            var idal = Assembly.Load(idalAssemblyName);
            var dal = Assembly.Load(dalAssemblyName);

            foreach (var type in idal.GetTypes())
            {
                //要是接口
                if (type.IsInterface)
                {

                    if (type.FullName.IndexOf("IDalBase", StringComparison.Ordinal) != -1)
                    {
                        continue;
                    }

                    if (type.FullName.IndexOf("IDalArticleBase", StringComparison.Ordinal) != -1)
                    {
                        continue;
                    }

                    if (type.FullName.IndexOf("IDalCrmBase", StringComparison.Ordinal) != -1)
                    {
                        continue;
                    }

                    var dalName = dalAssemblyName + type.FullName.Replace(idalAssemblyName, "").Replace(".IDal", ".") + "Dal";
                    var dalType = dal.GetType(dalName);
                    if (dalType != null)
                    {
                        _kernel.Bind(type).To(dalType);
                    }

                }

            }

        }

        /// <summary>
        /// 根据接口拿一个实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>() where T : class
        {
            return _kernel.Get<T>();
        }

    }
}
