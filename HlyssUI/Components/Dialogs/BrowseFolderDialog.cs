using HlyssUI.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HlyssUI.Components.Dialogs
{
    public class BrowseFolderDialog : HlyssForm
    {
        public delegate void FolderSelectedHandler(object sender, string path);
        public event FolderSelectedHandler OnFolderSelected;

        public BrowseFolderDialog()
        {
            Title = "Przeglądanie w poszukiwaniu folderu";
            Size = new Vector2u(350, 400);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Root.Padding = "5px";
            Root.Layout = Layout.LayoutType.Column;

            Root.Children = new List<Component>()
            {
                new Label("Wybierz folder")
                {
                    Margin = "5px"
                },
                new TreeView()
                {
                    Width = "100%",
                    Expand = true,
                    Name = "treeview",
                    Children = new List<Component>()
                    {
                        new TreeViewRoot("Computer")
                        {
                            Name = "computer_node",
                            Icon = Icons.Desktop
                        }
                    }
                },
                new Component()
                {
                    Width = "100%",
                    MarginTop = "5px",
                    AutosizeY = true,
                    Children = new List<Component>()
                    {
                        new Button("Nowy folder")
                        {
                            Action = () => CreateNewFolder()
                        },
                        new Spacer(),
                        new Button("Anuluj")
                        {
                            Action = () => Hide()
                        },
                        new Button("Ok")
                        {
                            Appearance = Button.ButtonStyle.Filled,
                            MarginLeft = "5px",
                            Action = () =>
                            {
                                TreeViewNode node = (Root.FindChild("treeview") as TreeView).GetSelectedNode();

                                if (node != null && node is TreeViewDirectory)
                                {
                                    OnFolderSelected?.Invoke(this, (node as TreeViewDirectory).Path);
                                }
                                Hide();
                            }
                        },
                    }
                }
            };

            string[] drives = Directory.GetLogicalDrives();
            TreeViewNode computerNode = Root.FindChild("computer_node") as TreeViewNode;

            foreach (var drive in drives)
            {
                computerNode.Children.Add(new TreeViewDirectory(drive)
                {
                    Icon = Icons.Hdd
                });
            }
        }

        private void CreateNewFolder()
        {
            TreeViewNode node = (Root.FindChild("treeview") as TreeView).GetSelectedNode();

            if (node != null && node is TreeViewDirectory)
            {
                string path = (node as TreeViewDirectory).Path;

                InputBox input = new InputBox("Nazwij folder", "Podaj nazwę nowego folderu:");
                input.ResultHandler = (object sender, string text) =>
                {
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(path, text));
                        (node as TreeViewDirectory).LoadDirectories(true);
                    }
                    catch(Exception e)
                    {
                        Application.RegisterAndShow(new MessageBox("Error", e.Message, "Ok"));
                    }
                };

                Application.RegisterAndShow(input);
            }
        }
    }

    internal class TreeViewDirectory : TreeViewRoot
    {
        public string Path { get; private set; }
        private bool _alreadyExpanded = false;

        public TreeViewDirectory(string path = "") : base("")
        {
            Path = path;
            Label = GetLastPathElement(path);
        }

        public override void OnExpanded()
        {
            base.OnExpanded();
            LoadDirectories();
        }

        public void LoadDirectories(bool force = false)
        {
            if (_alreadyExpanded && !force)
                return;

            try
            {
                string[] childDirectories = Directory.GetDirectories(Path);

                foreach (var dir in childDirectories)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);

                    if (!directoryInfo.Attributes.HasFlag(FileAttributes.Hidden) &&
                        !directoryInfo.Attributes.HasFlag(FileAttributes.System) &&
                        Children.Where(n => (n is TreeViewDirectory && (n as TreeViewDirectory).Path == dir)).Count() == 0)
                    {

                        Children.Add(new TreeViewDirectory(dir)
                        {
                            Icon = Icons.Folderpen
                        });
                    }
                }

                _alreadyExpanded = true;
            }
            catch (Exception e)
            {
                Form.Application.RegisterAndShow(new MessageBox("Error", e.Message, "Ok"));
            }
        }

        private string GetLastPathElement(string path)
        {
            string[] elements = path.Split(new char[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            return elements[elements.Length >= 1 ? elements.Length - 1 : 0];
        }
    }
}
