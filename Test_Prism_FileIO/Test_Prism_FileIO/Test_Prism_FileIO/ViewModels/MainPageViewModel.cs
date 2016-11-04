using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using PCLStorage;
using Prism.Services;

namespace Test_Prism_FileIO.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand 寫入檔案Command { get; private set; }
        public DelegateCommand 讀取檔案Command { get; private set; }

        public readonly IPageDialogService _dialogService;

        public MainPageViewModel(IPageDialogService dialogService)
        {
            _dialogService = dialogService;

            寫入檔案Command = new DelegateCommand(async () =>
            {
                try
                {
                    IFolder rootFolder = FileSystem.Current.LocalStorage;
                    IFolder sourceFolder = await rootFolder.CreateFolderAsync("MydataList", CreationCollisionOption.ReplaceExisting);
                    IFile sourceFile = await sourceFolder.CreateFileAsync("Myfile.dat", CreationCollisionOption.ReplaceExisting);
                    await sourceFile.WriteAllTextAsync("檔案內容1");
                    await _dialogService.DisplayActionSheetAsync("Info", "字串已寫入檔案中!", "OK");
                }
                catch (Exception EX)
                {

                    await _dialogService.DisplayActionSheetAsync("Error", $"字串已寫入檔案時發生錯誤!:{ EX.Message}", "OK");
                }
            }
            );
            讀取檔案Command = new DelegateCommand(async () =>
            {
                try
                {
                    IFolder sourceFolder = await FileSystem.Current.LocalStorage.GetFolderAsync("MydataList");
                    IFile sourceFile = await sourceFolder.GetFileAsync("Myfile.dat");
                    var foo = await sourceFile.ReadAllTextAsync();
                    await _dialogService.DisplayActionSheetAsync("Info", $"這是檔案中的文字:{foo}", "OK");
                }
                catch (Exception EX)
                {
                    await _dialogService.DisplayActionSheetAsync("Error", $"字串已讀取檔案時發生錯誤!:{ EX.Message}", "OK");
                }

            }
            );
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("title"))
                Title = (string)parameters["title"] + " and Prism";
        }
    }
}
