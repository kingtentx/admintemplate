using King.DAL;
using King.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace King.BLL
{
    public class DIBllRegister
    {
        public void DIRegister(IServiceCollection services)
        {
            // 用于实例化DalService对象，获取上下文对象
            services.AddTransient(typeof(IDalService<>), typeof(DalService<>));

            // 配置一个依赖注入映射关系 
            services.AddTransient(typeof(IBllService<>), typeof(BllService<>));

        }
    }
}
