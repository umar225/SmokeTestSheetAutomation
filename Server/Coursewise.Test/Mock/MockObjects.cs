using AutoMapper;
using Coursewise.Domain.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.Mock
{
    public static class MockObjects
    {
        public static IMapper GetMapper(IMapper mapper)
        {
            var mappingConfig = ModelMapper.Configure(new MapperConfigurationExpression());
            IMapper newMapper = mappingConfig.CreateMapper();
            mapper = newMapper;
            return mapper;
        }
    }
}
