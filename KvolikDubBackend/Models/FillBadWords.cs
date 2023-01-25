using KvolikDubBackend.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KvolikDubBackend.Models;

[Route("api/words")]
public class FillBadWords : ControllerBase
{
    //TODO: English bad words
    private AppDbContext _context;

    public FillBadWords(AppDbContext context)
    {
        _context = context;
    }

    /*[HttpGet]
    public async Task GetWords()
    {
        string[] lines = System.IO.File.ReadAllLines(@"D:\Рабочий стол\BW.txt");
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