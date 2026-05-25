// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Dashboard.Model.Favourites;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Icons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace Aspire.Dashboard.Components.Controls;

public partial class FavouritesMenu : ComponentBase
{
    private static readonly Icon s_starFilled = new Icons.Filled.Size20.Star();
    private static readonly Icon s_starRegular = new Icons.Regular.Size20.Star();

    private readonly string _buttonId = Identifier.NewId();
    private bool _open;
    private bool _hasFavourites;
    private Icon _starIcon = s_starRegular;

    [Parameter]
    public IReadOnlyList<SavedFavourite>? Favourites { get; set; }

    [Parameter]
    public SavedFavourite? SelectedFavourite { get; set; }

    [Parameter]
    public EventCallback OnSaveCurrent { get; set; }

    [Parameter]
    public EventCallback<SavedFavourite> OnApply { get; set; }

    [Parameter]
    public EventCallback<SavedFavourite> OnDelete { get; set; }

    protected override void OnParametersSet()
    {
        _hasFavourites = Favourites is { Count: > 0 };
        _starIcon = _hasFavourites ? s_starFilled : s_starRegular;
    }

    private void ToggleMenu()
    {
        _open = !_open;
    }

    private async Task OnSaveClickedAsync()
    {
        _open = false;
        if (OnSaveCurrent.HasDelegate)
        {
            await OnSaveCurrent.InvokeAsync();
        }
    }

    private async Task OnApplyAsync(SavedFavourite favourite)
    {
        _open = false;
        if (OnApply.HasDelegate)
        {
            await OnApply.InvokeAsync(favourite);
        }
    }

    private async Task OnDeleteAsync(SavedFavourite favourite)
    {
        if (OnDelete.HasDelegate)
        {
            await OnDelete.InvokeAsync(favourite);
        }
    }
}
