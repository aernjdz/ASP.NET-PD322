namespace StoreApi.Interfaces
{
   
        public interface IImageTool
        {

            Task<string> Save(IFormFile image);
            Task<string> Save(string url);
            bool Delete(string fileName);
        }
    }




