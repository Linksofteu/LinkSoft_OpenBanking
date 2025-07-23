namespace Workbench.Domain;

public class ApplicationsStore : IApplicationStore
{
    private string GetDataDirectory => Path.Combine(Directory.GetCurrentDirectory(), "./AppData");

    public async Task<IList<AccountDirectAccessApplicationManifest>> GetApplicationsAsync(string? targetEnvironment = null)
    {
        IDictionary<Guid, AccountDirectAccessApplicationManifest> allApps = await LoadApplicationsAsync(targetEnvironment);

        return allApps.Values.ToList();
    }

    public async Task<AccountDirectAccessApplicationManifest?> GetApplicationAsync(Guid appId, string? targetEnvironment = null)
    {
        IDictionary<Guid, AccountDirectAccessApplicationManifest> allApps = await LoadApplicationsAsync(targetEnvironment);

        if (allApps.TryGetValue(appId, out AccountDirectAccessApplicationManifest? app))
        {
            if (targetEnvironment != null && !targetEnvironment.Equals(app.TargetEnvironment, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return app;
        }

        return null;
    }

    public Task SaveApplication(AccountDirectAccessApplicationManifest application)
    {
        return SaveApplicationAsync(application);
    }

    private async Task<IDictionary<Guid, AccountDirectAccessApplicationManifest>> LoadApplicationsAsync(string? targetEnvironment = null)
    {
        IDictionary<Guid, AccountDirectAccessApplicationManifest> result = new Dictionary<Guid, AccountDirectAccessApplicationManifest>();

        foreach (string file in Directory.GetFiles(GetDataDirectory, "*.json"))
        {
            AccountDirectAccessApplicationManifest app = await LoadApplicationAsync(file);

            if (targetEnvironment != null && !targetEnvironment.Equals(app.TargetEnvironment, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            result.Add(app.Id, app);
        }

        return result;
    }

    private async Task<AccountDirectAccessApplicationManifest> LoadApplicationAsync(string fileName)
    {
        await using FileStream stream = File.OpenRead(fileName);
        AccountDirectAccessApplicationManifest? app = await JsonSerializer.DeserializeAsync<AccountDirectAccessApplicationManifest>(stream);

        if (app == null)
        {
            throw new Exception($"Failed to deserialize application from file {fileName}");
        }

        return app;
    }

    private async Task SaveApplicationAsync(AccountDirectAccessApplicationManifest application)
    {
        if (!Directory.Exists(GetDataDirectory))
        {
            Directory.CreateDirectory(GetDataDirectory);
        }

        await using FileStream stream = File.Create(Path.Combine(GetDataDirectory, $"{application.Id}.json"));
        await JsonSerializer.SerializeAsync(stream, application, SerializerOptions);
    }

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        IndentSize = 4
    };
}

public interface IApplicationStore
{
    Task<IList<AccountDirectAccessApplicationManifest>> GetApplicationsAsync(string? targetEnvironment);

    Task<AccountDirectAccessApplicationManifest?> GetApplicationAsync(Guid appId, string? targetEnvironment = null);

    Task SaveApplication(AccountDirectAccessApplicationManifest application);
}