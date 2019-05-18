using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace LazZiya.ExpressLocalization
{
    public class RouteTemplateModelConvention : IPageRouteModelConvention
    {
        public void Apply(PageRouteModel model)
        {
            var selectorCount = model.Selectors.Count;
            for (var i = 0; i < selectorCount; i++)
            {
                var selector = model.Selectors[i];
                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Order = -1,
                        Template = AttributeRouteModel.CombineTemplates(
                            "{culture?}",
                            selector.AttributeRouteModel.Template)
                    }
                });
            }
        }
    }
}
