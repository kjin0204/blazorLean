using BlazorInputFile;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public interface IFileUploadService
    {
        Task UploadAsync(IFileListEntry file);
    }
}
