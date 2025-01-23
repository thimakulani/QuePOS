using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Shared.Services
{ 
    public interface IHttpClientService
    {
        Task DeleteAsync(string endpoint);
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, object payload);
        Task<T> PutAsync<T>(string endpoint, object payload);
    }
}

