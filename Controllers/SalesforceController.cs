using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PCM.Services;
using PCM.ViewModels;
using System.Net.Http.Headers;

namespace PCM.Controllers
{
    public class SalesforceController : Controller
    {
        
        private readonly SalesforceClient _salesforceClient;
        public SalesforceController(SalesforceClient salesforceClient)
        {
            _salesforceClient = salesforceClient;
        }

        

       
    }
}
