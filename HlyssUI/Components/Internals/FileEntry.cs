using HlyssUI.Components.Dialogs;
using HlyssUI.Graphics;

namespace HlyssUI.Components.Internals
{
    internal class FileEntry : ListItem
    {
        private BrowseFolderDialog _browser;
        private BrowseFolderDialog.FSEntry _entry;
        private bool _selected = false;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (value != _selected)
                {
                    _selected = value;
                    //select
                }
            }
        }

        public FileEntry(BrowseFolderDialog browser, BrowseFolderDialog.FSEntry entry) : base(entry.Name)
        {
            _browser = browser;
            _entry = entry;

            Clicked += FileEntry_Clicked;
            DoubleClicked += FileEntry_DoubleClicked;

            Icon = (entry.IsDirectory) ? Icons.Folder : Icons.File;
        }

        private void FileEntry_DoubleClicked(object sender)
        {
            if (_entry.IsDirectory)
                _browser.Navigate(_entry.FullPath);
        }

        private void FileEntry_Clicked(object sender)
        {
            if (!_entry.IsDirectory)
                Selected = true;
        }
    }
}
