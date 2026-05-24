// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Aspire.Cli.Tests;

/// <summary>
/// Collection definition that disables parallel execution for tests which
/// build the real Aspire CLI host (via <see cref="Aspire.Cli.Program.Main"/>
/// or <see cref="Aspire.Cli.Program.BuildApplicationAsync"/>) or otherwise
/// create a live <see cref="Aspire.Cli.Telemetry.TelemetryManager"/> with the
/// Azure Monitor exporter enabled in-process.
/// </summary>
/// <remarks>
/// <para>
/// Tracks <see href="https://github.com/microsoft/aspire/issues/17450"/>.
/// </para>
/// <para>
/// Azure.Monitor.OpenTelemetry.Exporter uses <c>RateLimitedSampler</c> by default,
/// which returns a <c>SamplingResult</c> containing a <c>microsoft.sample_rate</c>
/// attribute once its adaptive sampling state has matured (~200ms after the
/// sampler is constructed). OpenTelemetry's <c>TracerProviderSdk</c> then writes
/// those attributes into <c>ActivityCreationOptions.SamplingTags</c> using a hard
/// <see cref="System.Diagnostics.ActivityTagsCollection.Add(string, object?)"/>
/// (no <c>TryAdd</c>). <c>ActivitySource.CreateActivity</c> reuses the SAME
/// <c>ActivityCreationOptions</c> instance across every registered listener, so
/// when two listeners on the <c>Aspire.Cli.Reported</c> source both run samplers
/// that emit <c>microsoft.sample_rate</c>, the second <c>Add</c> throws
/// <see cref="System.InvalidOperationException"/> ("The collection already
/// contains item with same key 'microsoft.sample_rate'").
/// </para>
/// <para>
/// Aspire CLI production never has more than one live <see cref="Aspire.Cli.Telemetry.TelemetryManager"/>
/// (registered as a DI singleton), so the bug is invisible at runtime. xUnit v3
/// runs test classes in parallel by default, however, which lets two host-building
/// tests race in-process. Placing every such test class in this collection
/// serializes their execution and keeps at most one Azure Monitor <c>TracerProvider</c>
/// alive at a time, eliminating the duplicate sampling-tag race.
/// </para>
/// </remarks>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class CliHostTestCollection
{
    public const string Name = "CliHostTests";
}
