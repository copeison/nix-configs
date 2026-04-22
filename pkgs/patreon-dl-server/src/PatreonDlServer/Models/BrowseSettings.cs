namespace PatreonDlServer.Models;

public sealed class BrowseSettings
{
    public string Theme { get; set; } = "default";

    public int CampaignsPerPage { get; set; } = 20;

    public int ContentPerPage { get; set; } = 20;

    public int MediaPerPage { get; set; } = 30;
}
