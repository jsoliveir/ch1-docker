using Api.Client.Subscriptions.Exceptions;
using Api.Core.Subscriptions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Services
{
    public class CoreSubscriptionsService : ICoreSubscriptionsService
    {
        private readonly ILogger<ICoreSubscriptionsService> _logger;
        private readonly HttpClient _httpClient;

        public CoreSubscriptionsService(
            ILogger<ICoreSubscriptionsService> logger,
            HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        private HttpContent GetRequestBody<T>(T @object)
        {
            return new StringContent(
                JsonSerializer.Serialize(@object),
                Encoding.UTF8,
                "application/json");
        }
        private async Task<T> GetResponse<T>(HttpResponseMessage result)
        {
            await ValidateResponse(result);

            var data = await result.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private async Task ValidateResponse(HttpResponseMessage result)
        {
            var data = await result.Content.ReadAsStringAsync();
            if (result.StatusCode < System.Net.HttpStatusCode.OK
               || result.StatusCode >= System.Net.HttpStatusCode.BadRequest)
            {
                _logger.LogError(data);
                _logger.LogError(JsonSerializer.Serialize(result));
                throw new HttpException(result.StatusCode, data);
            }
        }

        public async Task<Subscription> Create(SubscriptionViewModel subscription)
        {
            var request = GetRequestBody(subscription);
            var response = await _httpClient.PostAsync(_httpClient.BaseAddress, request);
            return await GetResponse<Subscription>(response);
        }

        public async Task Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/{id}");
            await ValidateResponse(response);
        }

        public async Task<Subscription> Get(int id)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{id}");
            return await GetResponse<Subscription>(response);
        }

        public async Task<IEnumerable<Subscription>> List(int max = 10)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/List?max={max}");
            return await GetResponse<List<Subscription>>(response);
        }

        public async Task<Subscription> Update(int id,Subscription subscription)
        {
            var request = GetRequestBody(subscription);
            var response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/{id}", request);
            return await GetResponse<Subscription>(response);
        }
    }
}
