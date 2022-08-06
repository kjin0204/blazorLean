using Agno.BlazorInputFile;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace WebAdimin.Services.FileUpload
{
    public class FileUploadService : IFileUploadService
    {
        private  IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment env)
        {
            this._environment = env;
        }

        /// <summary>
        /// 파일 업로드
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UploadAsync(IFileListEntry fileEntry)
        {
            //_environment.WebRootPath  (wwwroot(word wireld web 루트 경로), 폴더, 파일명
            var path = Path.Combine(_environment.WebRootPath, "TestUpload", fileEntry.Name);
            var ms = new MemoryStream(); //메모리 스트림 생성
            await fileEntry.Data.CopyToAsync(ms); // 메모리 스트림에 업로드해온 파일리스트를 복사
            //파일 쓰기(경로, 파일모드(생성),파일 엑세스는 쓰기)
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(file);
            }
        }
    }

}
