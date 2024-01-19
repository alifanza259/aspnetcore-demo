using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class CategoryRepository: ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }
    
    
    public ICollection<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }

    public Category GetCategory(int id)
    {
        return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
    }

    public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
    {
        return _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
    }

    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }

    public bool CreateCategory(Category category)
    {
        // Change Tracker
        _context.Add(category);
        
        // _context.SaveChanges(); Where entity framework take all you just give to context and save it to db
        // Can be put here (not calling Save, but to make it explicit I put it here

        return Save();
    }

    public bool UpdateCategory(Category category)
    {
        _context.Update(category);
        return Save();
    }

    public bool DeleteCategory(Category category)
    {
        _context.Remove(category);
        return Save();
    }

    public string LogTest()
    {
        return "hello from review repo";
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}