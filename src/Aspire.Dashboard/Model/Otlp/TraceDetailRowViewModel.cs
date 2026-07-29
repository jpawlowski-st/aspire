// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Dashboard.Otlp.Model;
using Aspire.Dashboard.Otlp.Storage;

namespace Aspire.Dashboard.Model.Otlp;

/// <summary>
/// Represents an item in the trace detail hierarchy.
/// </summary>
public sealed class TraceDetailRowViewModel
{
    private TraceDetailRowViewModel(SpanWaterfallViewModel? spanViewModel, LogSummary? logEntry, int depth)
    {
        SpanViewModel = spanViewModel;
        LogEntry = logEntry;
        Depth = depth;
    }

    /// <summary>
    /// Gets the span represented by this row, if any.
    /// </summary>
    public SpanWaterfallViewModel? SpanViewModel { get; }

    /// <summary>
    /// Gets the structured log represented by this row, if any.
    /// </summary>
    public LogSummary? LogEntry { get; }

    /// <summary>
    /// Gets the nesting depth relative to the trace.
    /// </summary>
    public int Depth { get; }

    /// <summary>
    /// Gets a value indicating whether this is a structured log row.
    /// </summary>
    public bool IsLog => LogEntry is not null;

    /// <summary>
    /// Gets the stable key used by the data grid.
    /// </summary>
    public string Key => SpanViewModel is { } spanViewModel
        ? $"span-{spanViewModel.Span.SpanId}"
        : $"log-{LogEntry!.InternalId}";

    /// <summary>
    /// Creates a span row.
    /// </summary>
    public static TraceDetailRowViewModel CreateSpan(SpanWaterfallViewModel spanViewModel)
    {
        return new TraceDetailRowViewModel(spanViewModel, logEntry: null, spanViewModel.Depth);
    }

    /// <summary>
    /// Creates a structured log row.
    /// </summary>
    public static TraceDetailRowViewModel CreateLog(LogSummary logEntry, int depth)
    {
        return new TraceDetailRowViewModel(spanViewModel: null, logEntry, depth);
    }
}
