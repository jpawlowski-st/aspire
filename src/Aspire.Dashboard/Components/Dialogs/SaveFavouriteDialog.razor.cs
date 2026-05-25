// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Aspire.Dashboard.Components.Dialogs;

public partial class SaveFavouriteDialog
{
    private string _name = string.Empty;
    private EditContext _editContext = default!;
    private FluentTextField? _nameField;

    [CascadingParameter]
    public FluentDialog? Dialog { get; set; }

    [Parameter]
    public SaveFavouriteDialogViewModel Content { get; set; } = default!;

    protected override void OnInitialized()
    {
        _name = Content.InitialName ?? string.Empty;
        _editContext = new EditContext(this);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _nameField?.FocusAsync();
        }
    }

    private async Task SaveAsync()
    {
        var trimmed = _name?.Trim();
        if (string.IsNullOrEmpty(trimmed) || Dialog is null)
        {
            return;
        }

        await Dialog.CloseAsync(DialogResult.Ok(trimmed));
    }

    private async Task CancelAsync()
    {
        if (Dialog is not null)
        {
            await Dialog.CancelAsync();
        }
    }

    public sealed class SaveFavouriteDialogViewModel
    {
        public string? InitialName { get; set; }
    }
}
