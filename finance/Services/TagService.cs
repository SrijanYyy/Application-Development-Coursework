using System.Text.Json;
using finance.Models;

namespace finance.Services
{
    public class TagService
    {
        private const string FilePath = "tags.json";

        public async Task<List<Tag>> GetTagsAsync()
        {
            if (!File.Exists(FilePath))
                return new List<Tag>();

            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Tag>>(json) ?? new List<Tag>();
        }

        public async Task SaveTagsAsync(List<Tag> tags)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                throw new ArgumentException("FilePath cannot be null or empty", nameof(FilePath));
            }

            var directoryPath = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var json = JsonSerializer.Serialize(tags, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }

        public async Task AddTagAsync(Tag tag)
        {
            var tags = await GetTagsAsync();
            tag.Id = tags.Any() ? tags.Max(t => t.Id) + 1 : 1;
            tags.Add(tag);
            await SaveTagsAsync(tags);
        }

        public async Task UpdateTagAsync(Tag updatedTag)
        {
            var tags = await GetTagsAsync();
            var existingTag = tags.FirstOrDefault(t => t.Id == updatedTag.Id);

            if (existingTag != null)
            {
                existingTag.TagName = updatedTag.TagName;
                await SaveTagsAsync(tags);
            }
        }

        public async Task DeleteTagAsync(int id)
        {
            var tags = await GetTagsAsync();
            tags.RemoveAll(t => t.Id == id);
            await SaveTagsAsync(tags);
        }
    }
}
