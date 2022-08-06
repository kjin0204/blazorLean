using Agno.BlazorInputFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdimin.Services.FileUpload
{
    public interface IFileUploadService
    {
        /// <summary>
        /// 파일업로드
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task UploadAsync(IFileListEntry file);
    }

}
