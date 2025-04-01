using Microsoft.Extensions.Options;

namespace BTL_WebNC.Extensions;

public static class ServiceExtenstion {
    public static void AddScopedRepository(this IServiceCollection services) {
        // Đăng ký repository và service tại đây
        // services.AddScoped<>;
        // services.AddScoped<>;
        // services.AddScoped<>;
        // services.AddScoped<>;
        // services.AddScoped<>;
        // services.AddScoped<>;
    }

    public static void AddSessionCookie(this IServiceCollection services) {
        services.AddSession(options => {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }
}