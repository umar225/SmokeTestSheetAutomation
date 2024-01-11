using AutoMapper;
using Coursewise.AWS.Extensions;
using Coursewise.Domain.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace Coursewise.Module.DependencyInjection
{
    public static class ComponentModule
    {
        public static void AddCoursewiseComponent(this IServiceCollection services, MapperConfigurationExpression? mapperConfigurationExpression = null)
        {
            var mapperConfig = ModelMapper.Configure(mapperConfigurationExpression);
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddAWS();
            services.AddSingleton(mapper);
        }
    }
}