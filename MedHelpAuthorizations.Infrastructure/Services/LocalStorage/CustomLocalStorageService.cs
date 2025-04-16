using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.Options;

namespace MedHelpAuthorizations.Infrastructure.Services.LocalStorage
{
    //public class CustomLocalStorageService : ILocalStorageService
    //{
    //    private readonly JsonSerializerOptions _jsonOptions;

    //    public CustomLocalStorageService(IOptions<LocalStorageOptions> options)
    //    {
    //        _jsonOptions = options.Value.JsonSerializerOptions;
    //    }

    //    public async ValueTask ClearAsync(CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }
                
    //    public async ValueTask<T> GetItemAsync<T>(string key, CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask<string> GetItemAsStringAsync(string key, CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public event EventHandler<ChangingEventArgs> Changing;
    //    public event EventHandler<ChangedEventArgs> Changed;
    //}
}
