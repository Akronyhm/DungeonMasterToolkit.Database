using DungeonMasterToolkit.Database.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMasterToolkit.Database;

public static class Startup
{
    public static void AddFDatabase(this IServiceCollection services, FDatabaseOptions options)
    {
        DbUtil.WaitForDatabase(options.ConnectionString);
        FContext.ConnectionString = options.ConnectionString;
        if (options.DatabaseVersion != null && options.DatabaseVersion.Count > 0)
        {
            DbUtil.Update(options.ConnectionString, options.DatabaseVersion, options.AllowDowngrade, options.DatabaseUpdateCommandTimeoutInSeconds);
        }
        services.AddDbContextFactory<FContext>(opts =>
        {
            opts.UseSqlServer(options.ConnectionString);
        });

        services.AddScoped(sp => sp.GetRequiredService<IDbContextFactory<FContext>>().CreateDbContext());
        //services.AddDbContext<FContext>(opts =>
        //{
        //    opts.UseSqlServer(options.ConnectionString);
        //});

        //builder.Services.AddDbContext<FContext>(options =>
        //    options.UseSqlServer(connectionString));
        //builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    }
}
