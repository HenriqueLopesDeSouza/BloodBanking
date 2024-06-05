using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BloodBanking.Infrastructure.EnumDocumentFilter
{
    public class EnumDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var enums = context.ApiDescriptions
                .SelectMany(apiDescription => apiDescription.ActionDescriptor.Parameters)
                .Where(param => param.ParameterType.IsEnum)
                .Select(param => param.ParameterType)
                .Union(
                    context.ApiDescriptions
                        .SelectMany(apiDescription => apiDescription.ActionDescriptor.Parameters)
                        .Where(param => param.ParameterType.IsGenericType && param.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>) && Nullable.GetUnderlyingType(param.ParameterType).IsEnum)
                        .Select(param => param.ParameterType.GetGenericArguments()[0])
                )
                .Distinct();

            foreach (var enumType in enums)
            {
                if (enumType.IsEnum)
                {
                    var enumSchema = swaggerDoc.Components.Schemas.FirstOrDefault(s => s.Key == enumType.Name);
                    if (enumSchema.Value != null && enumSchema.Value.Enum != null)
                    {
                        var enumNames = Enum.GetNames(enumType);
                        var enumDescriptions = new List<string>();

                        foreach (var enumName in enumNames)
                        {
                            var memberInfo = enumType.GetMember(enumName).FirstOrDefault();
                            if (memberInfo != null)
                            {
                                var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
                                if (displayAttribute != null)
                                {
                                    enumDescriptions.Add(displayAttribute.Name);
                                }
                                else
                                {
                                    enumDescriptions.Add(enumName);
                                }
                            }
                        }

                        enumSchema.Value.Description = string.Join(", ", enumDescriptions);
                    }
                }
            }
        }
    }
}
