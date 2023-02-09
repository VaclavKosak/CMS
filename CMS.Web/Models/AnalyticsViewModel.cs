using System.Collections.Generic;

namespace CMS.Web.Models;

public class AnalyticsViewModel
{
    public List<string> AnalyticsCodes { get; set; }
    public string ExtraScript { get; set; }
}