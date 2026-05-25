// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Dashboard.Utils;

namespace Aspire.Dashboard.Model.Favourites;

public sealed class FavouritesStore
{
    private readonly ILocalStorage _localStorage;

    public FavouritesStore(ILocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<List<SavedFavourite>> GetAsync(string pageKey)
    {
        var result = await _localStorage.GetUnprotectedAsync<List<SavedFavourite>>(BrowserStorageKeys.FavouritesKey(pageKey)).ConfigureAwait(false);
        return result.Success && result.Value is { } favourites ? favourites : new List<SavedFavourite>();
    }

    public async Task<SavedFavourite> AddAsync(string pageKey, string name, string url, string? filterText)
    {
        var favourites = await GetAsync(pageKey).ConfigureAwait(false);
        var favourite = new SavedFavourite
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = name,
            Url = url,
            FilterText = string.IsNullOrEmpty(filterText) ? null : filterText,
            CreatedAt = DateTimeOffset.UtcNow,
        };
        favourites.Add(favourite);
        await _localStorage.SetUnprotectedAsync(BrowserStorageKeys.FavouritesKey(pageKey), favourites).ConfigureAwait(false);
        return favourite;
    }

    public async Task RemoveAsync(string pageKey, string id)
    {
        var favourites = await GetAsync(pageKey).ConfigureAwait(false);
        if (favourites.RemoveAll(f => f.Id == id) > 0)
        {
            await _localStorage.SetUnprotectedAsync(BrowserStorageKeys.FavouritesKey(pageKey), favourites).ConfigureAwait(false);
        }
    }
}
