using Web_Hulk.interfaces;
namespace Web_Hulk.Services
{
    public class ImageWorker: IImageWorker

    {
        public string ImageSave(string url)
        {
            string imageName = Guid.NewGuid().ToString() + ".webp";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Send a GET request to the image URL
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Check if the response status code indicates success (e.g., 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the image bytes from the response content
                        byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                        var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                        var path = Path.Combine(dir, imageName);
                        File.WriteAllBytes(path, imageBytes);

                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve image. Status code: {response.StatusCode}");
                        return String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return String.Empty;
            }
            return imageName;
        }
    }

}

