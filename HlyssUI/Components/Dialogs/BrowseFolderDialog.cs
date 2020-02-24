using HlyssUI.Components.Internals;
using HlyssUI.Graphics;
using HlyssUI.Layout;
using HlyssUI.Styling;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HlyssUI.Components.Dialogs
{
    public class BrowseFolderDialog : HlyssForm
    {
        public struct FSEntry
        {
            public string Name;
            public string FullPath;
            public bool IsDirectory;
            public FileAttributes Attributes;

            public FSEntry(string path)
            {
                Name = Path.GetFileName(path);
                FullPath = path;
                Attributes = File.GetAttributes(path);
                IsDirectory = Attributes.HasFlag(FileAttributes.Directory);
            }
        }

        public delegate void FileSelectedHandler(object sender, FSEntry entry);
        public event FileSelectedHandler OnFileSelected;

        private string _currentDirectory = string.Empty;
        private List<FSEntry> _entries = new List<FSEntry>();

        private string CurrentPath
        {
            get { return (Root.FindChild("directory_box") as TextBox).Text; }
            set { (Root.FindChild("directory_box") as TextBox).Text = value; }
        }

        public string StartDirectory { get; set; } = "C:\\";

        public BrowseFolderDialog()
        {
            Title = "Przeglądanie w poszukiwaniu folderu";
            Size = new Vector2u(550, 400);
            Icon = null;
        }

        public void Navigate(string path)
        {
            if (Directory.Exists(path))
            {
                _currentDirectory = path;
                CurrentPath = path;

                _entries.Clear();

                foreach (var file in Directory.GetFileSystemEntries(path))
                {
                    FSEntry fsEntry = new FSEntry(file);

                    if (fsEntry.Attributes.HasFlag(FileAttributes.Hidden) == false
                        && fsEntry.Attributes.HasFlag(FileAttributes.System) == false
                        && fsEntry.IsDirectory)
                    {
                        _entries.Add(fsEntry);
                    }
                }

                RefreshList();
            }
        }

        private void RefreshList()
        {
            Component list = Root.FindChild("files_panel");
            list.ScrollToY(0);
            list.Children.Clear();

            foreach (var entry in _entries)
            {
                list.Children.Add(new FileEntry(this, entry));
            }
        }

        protected override void OnInitialized()
        {
            Root.Children.Add(new Component()
            {
                Width = "100%",
                Height = "100%",
                Padding = "10px",
                Layout = LayoutType.Column,
                Children = new List<Component>()
                {
                    new Component()
                    {
                        Width = "100%",
                        AutosizeY = true,
                        CenterContent = true,
                        Children = new List<Component>()
                        {
                            new Button()
                            {
                                Padding = "10px",
                                Children = new List<Component>()
                                {
                                    new Icon(Icons.AngleUp)
                                },
                                Name = "up_btn"
                            },
                            new TextBox()
                            {
                                Expand = true,
                                Margin = "0px 10px",
                                Height = "0px",
                                Name = "directory_box"
                            },
                            new Dropdown()
                            {
                                Width = "100px",
                                Name = "drives",
                                Items = DriveInfo.GetDrives().Select(o => o.Name).ToList()
                            }
                        }
                    },
                    new Panel()
                    {
                        Width = "100%",
                        MarginTop = "10px",
                        Padding = "5px 1px",
                        Expand = true,
                        Name = "files_panel",
                        Layout = LayoutType.Column,
                        Overflow = OverflowType.Scroll
                    },
                    new Component()
                    {
                        Width = "100%",
                        AutosizeY = true,
                        Reversed = true,
                        MarginTop = "10px",
                        Children = new List<Component>()
                        {
                            new Button("Otwórz")
                            {
                                Appearance = Button.ButtonStyle.Filled,
                                Name = "ok_btn"
                            },
                            new Button("Anuluj")
                            {
                                MarginRight = "5px",
                                Name = "close_btn"
                            }
                        }
                    }
                }
            });

            Root.FindChild("dropdown_button").Padding = "10px";

            Root.FindChild("close_btn").Clicked += (object sender) =>
            {
                Window.Close();
            };

            Root.FindChild("up_btn").Clicked += Up;

            Window.KeyReleased += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.Enter)
                    Navigate(CurrentPath);
            };

            (Root.FindChild("drives") as Dropdown).OnSelected += (object sender, string text, int id) =>
            {
                Navigate(text);
            };

            Root.FindChild("ok_btn").Clicked += (object sender) =>
            {
                OnFileSelected?.Invoke(this, new FSEntry(CurrentPath));
                Window.Close();
            };

            Navigate(StartDirectory);
        }

        private void Up(object sender)
        {
            if (Directory.Exists(_currentDirectory))
            {
                DirectoryInfo dInfo = new DirectoryInfo(_currentDirectory);
                if (dInfo.Parent != null)
                {
                    Navigate(dInfo.Parent.FullName);
                }
            }
        }
    }
}
