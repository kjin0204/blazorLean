using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Agno.BlazorInputFile;
using Microsoft.AspNetCore.Components; // Inject 사용하기 위함
using WebAdimin.Services.FileUpload;

namespace WebAdimin.Pages.Demo
{
    public partial class FrmFileUpload
    {
        //startup에서 주입받아서 사용
        [Inject]
        public IFileUploadService FileUploadServiceReference { get; set; }

        private IFileListEntry[] selectedFiles;
        protected void HandleSelection(IFileListEntry[] files)
        {
            this.selectedFiles = files;

            for (int i = 0; i < files.Length; i++)
                Debug.WriteLine(files[i].Name);
        }

        protected async void UploadClick()
        {
            if (selectedFiles.Length == 0)
                return;

            for(int i =0; i< selectedFiles.Length; i++)
                await FileUploadServiceReference.UploadAsync(selectedFiles[i]);
        }

        int i = 0;

        public void test()
        {
            i++;
            Debug.WriteLine(i);
        }
    }
}
