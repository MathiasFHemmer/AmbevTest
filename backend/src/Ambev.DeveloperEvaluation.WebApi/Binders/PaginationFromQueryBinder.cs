using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ambev.DeveloperEvaluation.Common.Pagination;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class PaginationFromQueryAttribute : Attribute, IBindingSourceMetadata, IBinderTypeProviderMetadata
{
    public BindingSource BindingSource => BindingSource.Custom;
    public Type BinderType => typeof(PaginationFromQueryModelBinder);
    public bool IsBindingRequired => false;
}

public class PaginationFromQueryModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var query = bindingContext.HttpContext.Request.Query;
        uint page = 1;
        uint size = 10;
        if (query.TryGetValue("_page", out var pageValue) && uint.TryParse(pageValue, out var p))
            page = p;

        if (query.TryGetValue("_size", out var sizeValue) && uint.TryParse(sizeValue, out var s))
            size = s;

        var result = new PaginateRequest(page, size);
        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }
}