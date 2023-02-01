using KvolikDubBackend.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Models;

[Route("api/words")]
public class FillBadWords : ControllerBase
{
    private AppDbContext _context;

    public FillBadWords(AppDbContext context)
    {
        _context = context;
    }

    /*[HttpGet]
    public async Task GetWords()
    {
        string[] lines = System.IO.File.ReadAllLines(@"BW.txt");
        foreach (var line in lines)
        {
            var badWord = new BadWordEntity()
            {
                Id = new Guid(),
                Word = line
            };
            
            await _context
                .BadWords
                .AddAsync(badWord);
            await _context.SaveChangesAsync();
        }
    }*/
}