// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.Bff.SessionManagement.SessionStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Duende.Bff.EntityFramework;

/// <summary>
/// Extensions for BffBuilder
/// </summary>
public static class BffBuilderExtensions
{
    /// <summary>
    /// Adds entity framework core support for user session store.
    /// </summary>
    /// <param name="bffBuilder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static BffBuilder AddEntityFrameworkServerSideSessions(this BffBuilder bffBuilder, Action<IServiceProvider, DbContextOptionsBuilder> action) => bffBuilder.AddEntityFrameworkServerSideSessions<SessionDbContext>(action);

    /// <summary>
    /// Adds entity framework core support for user session store.
    /// </summary>
    /// <param name="bffBuilder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static BffBuilder AddEntityFrameworkServerSideSessions(this BffBuilder bffBuilder, Action<DbContextOptionsBuilder> action) => bffBuilder.AddEntityFrameworkServerSideSessions<SessionDbContext>(action);

    /// <summary>
    /// Adds entity framework core support for user session store.
    /// </summary>
    /// <param name="bffBuilder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static BffBuilder AddEntityFrameworkServerSideSessions<TContext>(this BffBuilder bffBuilder, Action<IServiceProvider, DbContextOptionsBuilder> action)
        where TContext : DbContext, ISessionDbContext
    {
        ArgumentNullException.ThrowIfNull(bffBuilder);
        bffBuilder.Services.AddDbContext<TContext>(action);
        return bffBuilder.AddEntityFrameworkServerSideSessionsServices<TContext>();
    }

    /// <summary>
    /// Adds entity framework core support for user session store.
    /// </summary>
    /// <param name="bffBuilder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static BffBuilder AddEntityFrameworkServerSideSessions<TContext>(this BffBuilder bffBuilder, Action<DbContextOptionsBuilder> action)
        where TContext : DbContext, ISessionDbContext
    {
        ArgumentNullException.ThrowIfNull(bffBuilder);
        bffBuilder.Services.AddDbContext<TContext>(action);
        return bffBuilder.AddEntityFrameworkServerSideSessionsServices<TContext>();
    }


    /// <summary>
    /// Adds entity framework core support for user session store, but does not register a DbContext.
    /// Use this API to register the BFF Entity Framework services when you plan to register your own DbContext (e.g. with AddDbContextPool).
    /// </summary>
    /// <param name="bffBuilder"></param>
    /// <returns></returns>
    public static BffBuilder AddEntityFrameworkServerSideSessionsServices<TContext>(this BffBuilder bffBuilder)
        where TContext : ISessionDbContext
    {
        ArgumentNullException.ThrowIfNull(bffBuilder);
        bffBuilder.Services.AddTransient<IUserSessionStoreCleanup, UserSessionStore>();
        bffBuilder.Services.AddTransient<ISessionDbContext>(svcs => svcs.GetRequiredService<TContext>());
        return bffBuilder.AddServerSideSessions<UserSessionStore>();
    }

    /// <summary>
    /// Allows configuring the SessionStoreOptions.
    /// </summary>
    public static BffBuilder ConfigureEntityFrameworkSessionStoreOptions(this BffBuilder bffBuilder, Action<SessionStoreOptions> action)
    {
        ArgumentNullException.ThrowIfNull(bffBuilder);
        bffBuilder.Services.Configure(action);
        return bffBuilder;
    }
}
