﻿using Invoices.Data;
using Invoices.Models;

using Microsoft.AspNetCore.Identity;

namespace Invoices;

public class SeedData
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedData>>();

            var context = scope.ServiceProvider.GetRequiredService<InvoicesContext>();
            //await context.Database.EnsureDeletedAsync();
            //context.Database.Migrate();
            await context.Database.EnsureCreatedAsync();

            if(!context.Invoices.Any())
            {
                context.Invoices.AddRange(new[]
                {
                    new Invoice(DateTime.Now, InvoiceStatus.Created, 100, 25, 0.25),
                    new Invoice(DateTime.Now, InvoiceStatus.Created, 100, 25, 0.25)
                });

                await context.SaveChangesAsync();
            }
        }
    }
}