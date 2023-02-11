using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS.Web.Components;

public class AnalyticsComponent : ViewComponent
{
    private readonly IConfiguration _configuration;
    
    public AnalyticsComponent(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Task<IViewComponentResult> InvokeAsync(string extraScript)
    {
        var configValues = new List<string>();
        
        var configValue = _configuration["google-analytics"];
        // Google analytics
        if (!string.IsNullOrEmpty(configValue))
        {
            configValues.Add(configValue);
        }
        
        // Google ads
        configValue = _configuration["google-ads"];
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