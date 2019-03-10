using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Api.ModelBinders
{
    public class JsonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            ValueProviderResult valueProviderResult = context.ValueProvider.GetValue(context.ModelName);
            if (valueProviderResult != ValueProviderResult.None)
            {
                context.ModelState.SetModelValue(context.ModelName, valueProviderResult);

                string valueAsString = valueProviderResult.FirstValue;
                object result = JsonConvert.DeserializeObject(valueAsString, context.ModelType);
                if (result != null)
                {
                    context.Result = ModelBindingResult.Success(result);
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }
    }
}
