using Azure.Storage.Blobs;
using AzureImage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AzureImage.Controllers;

public class HomeController : Controller
{
    private readonly Context _context;

    private readonly IConfiguration _configuration;

    public HomeController(Context context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public IActionResult Index()
    {
        return View(_context.Images.ToList().LastOrDefault());
    }

    [HttpPost("/image")]
    public async Task<IActionResult> PostImageInAzureAsync(IFormFile? image)
    {
        var connectionString = _configuration.GetValue<string>("Azure:BlobStorage:ConnectionString");
        string imagePath = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

        string containerName = "images";
        BlobContainerClient containerClient;

        if (blobServiceClient.GetBlobContainers().Any(x => x.Name == containerName))
        {
            containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }
        else
        {
            containerClient = blobServiceClient.CreateBlobContainer(containerName);
        }

        var blobClient = containerClient.GetBlobClient(imagePath);
        using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream);
        }

        if (image != null)
        {
            _context.Images.Add(new Image
            {
                Url = blobClient.Uri.AbsoluteUri
            });
            _context.SaveChanges();
        }
        return Redirect("/");
    }
}
