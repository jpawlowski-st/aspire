// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Dashboard.Model;
using Aspire.Dashboard.Otlp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Aspire.Dashboard.Components.Controls;

public partial class SpanKindSelect : ComponentBase
{
    [Parameter, EditorRequired]
    public required List<SelectViewModel<OtlpSpanKind?>> SpanKinds { get; set; }

    [Parameter, EditorRequired]
    public required SelectViewModel<OtlpSpanKind?> SpanKind { get; set; }

    [Parameter]
    public EventCallback<SelectViewModel<OtlpSpanKind?>> SpanKindChanged { get; set; }

    [Parameter, EditorRequired]
    public required Func<Task> HandleSelectedSpanKindChangedAsync { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter]
    public required ViewportInformation ViewportInformation { get; set; }

    [Inject]
    public required IStringLocalizer<Resources.Traces> Loc { get; init; }

    private async Task HandleSelectedSpanKindChangedInternalAsync()
    {
        await SpanKindChanged.InvokeAsync(SpanKind);
        await HandleSelectedSpanKindChangedAsync();
    }
}
