using System.Text.Json;

public static class SessionExtension {
    public static void SetObject(this ISession session, string key, object value) {
        string _value = JsonSerializer.Serialize(value);
        session.SetString(key, _value);
    }

    public static T? GetObject<T>(this ISession session, string key) {
        string? value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}