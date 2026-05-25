// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Dashboard.Model;
using Aspire.Dashboard.Otlp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Aspire.Dashboard.Components.Controls;

public partial class SpanStatusSelect : ComponentBase
{
    [Parameter, EditorRequired]
    public required List<SelectViewModel<OtlpSpanStatusCode?>> SpanStatuses { get; set; }

    [Parameter, EditorRequired]
    public required SelectViewModel<OtlpSpanStatusCode?> SpanStatus { get; set; }

    [Parameter]
    public EventCallback<SelectViewModel<OtlpSpanStatusCode?>> SpanStatusChanged { get; set; }

    [Parameter, EditorRequired]
    public required Func<Task> HandleSelectedSpanStatusChangedAsync { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter]
    public required ViewportInformation ViewportInformation { get; set; }

    [Inject]
    public required IStringLocalizer<Resources.Traces> Loc { get; init; }

    private async Task HandleSelectedSpanStatusChangedInternalAsync()
    {
        await SpanStatusChanged.InvokeAsync(SpanStatus);
        await HandleSelectedSpanStatusChangedAsync();
    }
}
