using HlyssUI.Graphics;
using SFML.Graphics;
using System.Collections.Generic;

namespace HlyssUI.Components
{
    public class TreeView : Panel
    {
        public TreeView()
        {
            Padding = "5px 1px";
            Layout = HlyssUI.Layout.LayoutType.Column;
            Overflow = HlyssUI.Layout.OverflowType.Scroll;
        }

        public TreeViewNode GetSelectedNode()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();
            GetAllNodes(ref nodes, this);

            foreach (var node in nodes)
            {
                if (node.IsSelected)
                    return node;
            }

            return null;
        }

        internal void DeselectAll(Component root)
        {
            Component node = root;

            foreach (var child in node.Children)
            {
                if (child is TreeViewNode)
                    (child as TreeViewNode).IsSelected = false;

                DeselectAll(child);
            }
        }

        internal void GetAllNodes(ref List<TreeViewNode> nodes, Component root)
        {
            foreach (var child in root.Children)
            {
                if (child is TreeViewNode)
                    nodes.Add(child as TreeViewNode);

                GetAllNodes(ref nodes, child);
            }
        }
    }

    public class TreeViewNode : Component
    {
        public string Label
        {
            get { return (FindChild("treeviewnode_label") as Label).Text; }
            set { (FindChild("treeviewnode_label") as Label).Text = value; }
        }

        public Texture BitmapIcon
        {
            set
            {
                PictureBox picture = FindChild("treeviewnode_bitmap_icon") as PictureBox;
                picture.Image = value;
                picture.Visible = true;
                picture.SmoothImage = true;
            }
        }

        public Icons Icon
        {
            set
            {
                Icon picture = FindChild("treeviewnode_icon") as Icon;
                picture.IconType = value;
                picture.Visible = true;
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    FindChild("treeviewnode_panel").Style = value ? "treeview_node_selected" : "list_item_default";
                    _isSelected = value;
                }
            }
        }

        private bool _isSelected = false;

        public TreeViewNode(string label = "")
        {
            Width = "100%";
            AutosizeY = true;
            Layout = HlyssUI.Layout.LayoutType.Column;

            Children = new List<Component>()
            {
                new Panel()
                {
                    Width = "100%",
                    AutosizeY = true,
                    Padding = "8px 10px",
                    Name = "treeviewnode_panel",
                    Style = "list_item_default",
                    CenterContent = true,
                    Children = new List<Component>()
                    {
                        new Component()
                        {
                            Width = "16px",
                            AutosizeY = true,
                            Name = "treeviewnode_expand_btn",
                            Children = new List<Component>()
                            {
                                new Icon(Icons.Empty)
                                {
                                    Name = "treeviewnode_expand_btn_icon"
                                }
                            }
                        },
                        new PictureBox()
                        {
                            Width = "16px",
                            Height = "16px",
                            MarginRight = "5px",
                            Name = "treeviewnode_bitmap_icon",
                            Visible = false
                        },
                        new Icon(Icons.Empty)
                        {
                            MarginRight = "5px",
                            Name = "treeviewnode_icon",
                            Style = "treeview_icon_default",
                            Visible = false
                        },
                        new Label(label)
                        {
                            Name = "treeviewnode_label",
                        }
                    }
                }
            };

            FindChild("treeviewnode_panel").Clicked += (_) =>
            {
                TreeView treeView = GetTreeView();
                treeView.DeselectAll(treeView);

                IsSelected = true;
            };
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            Children[0].PaddingLeft = $"{10 + GetLevel() * 20}px";
        }

        protected int GetLevel()
        {
            Component component = this;
            int level = -1;

            while (!(component is TreeView))
            {
                level++;

                if (component.Parent != null)
                    component = component.Parent;
                else break;
            }

            return level;
        }

        protected TreeView GetTreeView()
        {
            Component component = this;

            while (!(component is TreeView))
            {
                if (component.Parent != null)
                    component = component.Parent;
                else break;
            }

            if (component != null && component is TreeView)
                return component as TreeView;

            return null;
        }
    }

    public class TreeViewRoot : TreeViewNode
    {
        public delegate void ExpandedHandler(object sender);
        public event ExpandedHandler Expanded;

        private bool _expanded = false;
        public bool IsExpanded
        {
            get { return _expanded; }
            set
            {
                if (value != _expanded)
                {
                    _expanded = value;
                    SetExpanded(value);
                }
            }
        }

        public TreeViewRoot(string label = "") : base(label)
        {
            OverwriteChildren = false;
            Children[0].DoubleClicked += (_) => IsExpanded = !IsExpanded;
        }

        public override void OnInitialized()
        {
            base.OnInitialized();

            FindChild("treeviewnode_expand_btn").Clicked += (_) => IsExpanded = !IsExpanded;
            FindChild("treeviewnode_expand_btn").Style = "treeview_expand_btn_icon_default";

            SetExpanded(false);
        }

        private void SetExpanded(bool expanded)
        {
            for (int i = 1; i < Children.Count; i++)
            {
                Children[i].Visible = expanded;
            }

            (FindChild("treeviewnode_expand_btn_icon") as Icon).IconType = expanded ? Icons.AngleDown : Icons.AngleRight;

            if (expanded == true)
            {
                Expanded?.Invoke(this);
                OnExpanded();
            }
        }

        public virtual void OnExpanded() { }
    }

    public class TreeViewElement : TreeViewNode
    {
        public TreeViewElement(string label = "") : base(label)
        {
        }
    }
}
