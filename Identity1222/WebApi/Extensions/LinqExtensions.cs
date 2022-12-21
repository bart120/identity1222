using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace WebApi.Extensions
{
    public static class LinqExtensions
    {

        /*public static IEnumerable<T> SelectFilter<T>(this T values)
        {
            dynamic data = new ExpandoObject();
            var dataDico = (IDictionary<string, object>)data;
            var props = typeof(T).GetProperties().Where(x => x.CustomAttributes.Any(y => y.AttributeType.Name == "DisplayApiAttribute"));
            foreach (var prop in props)
            {
                dataDico.Add(prop, props.Single(x => x.Name == prop.Name).GetValue())
            }
        }*/
    }
}
