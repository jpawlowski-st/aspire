// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Aspire.Dashboard.Model.Favourites;

public sealed class SavedFavourite
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }
    public string? FilterText { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
