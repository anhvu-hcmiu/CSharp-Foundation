using System.Text.Json;
using PersonalLedger.Models;

namespace PersonalLedger.Repositories;

public class JsonFileRepository<T>(string filePath) : IRepository<T> where T : IEntity
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public async Task<List<T>> GetAllAsync()
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, SerializerOptions) ?? [];
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var items = await GetAllAsync();
        return items.FirstOrDefault(item => item.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        var items = await GetAllAsync();
        if (items.Any(item => item.Id == entity.Id))
        {
            throw new InvalidOperationException($"Entity with Id '{entity.Id}' already exists.");
        }

        items.Add(entity);
        await SaveAllAsync(items);
    }

    public async Task UpdateAsync(T entity)
    {
        var items = await GetAllAsync();
        var index = items.FindIndex(item => item.Id == entity.Id);
        if (index == -1)
        {
            throw new InvalidOperationException($"Entity with Id '{entity.Id}' was not found.");
        }

        items[index] = entity;
        await SaveAllAsync(items);
    }

    public async Task DeleteAsync(Guid id)
    {
        var items = await GetAllAsync();
        var removed = items.RemoveAll(item => item.Id == id);
        if (removed == 0)
        {
            throw new InvalidOperationException($"Entity with Id '{id}' was not found.");
        }

        await SaveAllAsync(items);
    }

    private async Task SaveAllAsync(List<T> items)
    {
        var json = JsonSerializer.Serialize(items, SerializerOptions);
        await File.WriteAllTextAsync(filePath, json);
    }
}
