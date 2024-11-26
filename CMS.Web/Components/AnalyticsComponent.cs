using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS.Web.Components;

public class AnalyticsComponent(IConfiguration configuration) : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(string extraScript)
    {
        var configValues = new List<string>();
        
        var configValue = configuration["google-analytics"];
        
        // Google analytics
        if (!string.IsNullOrEmpty(configValue))
        {
            configValues.Add(configValue);
        }
        
        // Google ads
        configValue = configuration["google-ads"];
        if (!string.IsNullOrEmpty(configValue))
        {
            configValues.Add(configValue);
        }

        var analyticsModel = new AnalyticsViewModel
        {
            AnalyticsCodes = configValues,
            ExtraScript = extraScript
        };

        return Task.FromResult<IViewComponentResult>(View(analyticsModel));
    }
}