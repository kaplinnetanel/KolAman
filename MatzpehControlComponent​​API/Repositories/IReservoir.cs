using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MatzpehControlComponent​​API.Repositories;

public interface IReservoir
{
    Task<Dictionary<string, long>> Count();
}
