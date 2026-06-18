using AutoMapper;
using CashFlow.Application.AutoMapper;
using Microsoft.Extensions.Logging;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var loggerFactory = new LoggerFactory();

        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapping());
        }, loggerFactory);

        return mapper.CreateMapper();
    }
}
