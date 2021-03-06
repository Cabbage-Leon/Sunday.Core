using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Sunday.Core.Api.Filter
{
    public class AutofacPropertityModuleReg : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
        }
    }
}