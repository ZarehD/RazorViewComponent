namespace WebAppUiComponents
{
    public static class IdHelper
    {
        public static string GetUniqueId(string? prefix) =>
            $"{prefix}_{Guid.NewGuid():n}";
    }
}
