using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Data
{
    public static class LibraryContextSeeding
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.Authors.AnyAsync())
                return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "Seeding", "Authors.json");

            if (!File.Exists(filePath))
                return;

            await using var stream = File.OpenRead(filePath);
            var authors = await JsonSerializer.DeserializeAsync<List<Author>>(stream, SerializerOptions);

            if (authors is null || authors.Count == 0)
                return;

            await dbContext.Authors.AddRangeAsync(authors);
            await dbContext.SaveChangesAsync();
        }
    }
}
