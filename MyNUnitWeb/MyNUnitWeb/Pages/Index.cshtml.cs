namespace MyNUnitWeb.Pages;

[BindProperties]
public class IndexModel : PageModel
{
    public readonly string TestsPath = Directory.GetCurrentDirectory() + "/wwwroot/Assemblies";

    public async Task<IActionResult> OnPostLoadAsync(IFormFile file)
    {
        if (file is not null)
        {
            var filePath = Path.Combine(TestsPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        return Page();
    }

    public IActionResult OnPostDelete(string filePath)
    {
        System.IO.File.Delete(filePath);
        return Page();
    }

    // public Task<IActionResult> OnPostRun(string fileName)
    // {
    //     var filePath = Path.Combine(TestsPath, fileName);
    //     if (System.IO.File.Exists(filePath))
    //     {
    //         System.IO.File.Delete(filePath);
    //     }

    //     return Page();
    // }
}
