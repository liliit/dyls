using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Ninject;

namespace IDal
{
    public class DalFactory
    {
        private static readonly IKernel _kernel;

        static DalFactory()
        {
            _kernel = new StandardKernel();
            var idalAssemblyName = "IDal";
            var dalAssemblyName = "Dal";
            var idal = Assembly.Load(idalAssemblyName);
            var dal = Assembly.Load(dalAssemblyName);

            foreach (var item in idal.GetTypes())
            {
                if(item.IsInterface)
                {
                    if(item.FullName.IndexOf("IDalBase",StringComparison.Ordinal)!=-1)
                    {
                        continue;
                    }

                    var dalName = dalAssemblyName + item.FullName
                        .Replace(idalAssemblyName, "")
                        .Replace(".IDal", ".") + "Dal";
                    var dalType = dal.GetType(dalName);
                    if(dalType!=null)
                    {
                        _kernel.Bind(item).To(dalType);
                    }
                }
            }
        }

        /// <summary>
        /// 根据接口拿一个实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Instance<T>() where T : class
        {
            return _kernel.Get<T>();
        }
    }
}
