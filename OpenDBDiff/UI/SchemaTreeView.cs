using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Attributes;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.Properties;

namespace OpenDBDiff.UI;

public partial class SchemaTreeView : UserControl
{
    private ISchemaBase databaseSource;

    public delegate void SchemaHandler(string ObjectFullName);

    public event SchemaHandler OnSelectItem;

    private bool busy;

    public SchemaTreeView()
    {
        InitializeComponent();

        var imageList1 = new ImageList
        {
            ImageSize = new Size(16, 16),
            TransparentColor = Color.Transparent

        };

        imageList1.Images.Add("Folder", Resources.image0);
        imageList1.Images.Add("Table", Resources.image1);
        imageList1.Images.Add("Procedure", Resources.image2);
        imageList1.Images.Add("User", Resources.image3);
        imageList1.Images.Add("Column", Resources.image4);
        imageList1.Images.Add("Index", Resources.image5);
        imageList1.Images.Add("Rol", Resources.image6);
        imageList1.Images.Add("Schema", Resources.image7);
        imageList1.Images.Add("View", Resources.image8);
        imageList1.Images.Add("Function", Resources.image9);
        imageList1.Images.Add("XMLSchema", Resources.image10);
        imageList1.Images.Add("Database", Resources.image11);
        imageList1.Images.Add("UDT", Resources.image12);
        imageList1.Images.Add("Assembly", Resources.image13);
        imageList1.Images.Add("PartitionFunction", Resources.image14);
        imageList1.Images.Add("PartitionScheme", Resources.image15);
        treeView1.ImageList = imageList1;



        /*     System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemaTreeView));
             var stream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
             if (stream != null)
             {
                 var il = new ImageList
                 {
                     ImageStream = stream
                 };

                 for (int q = 0; q < il.Images.Count; q++)
                 {
                     var image = il.Images[q];
                     image.Save($@"c:\output\image{q}.bmp");

                 }

                 stream = null;
             }*/

    }

    public ISchemaBase RightDatabase { get; set; }

    public ISchemaBase LeftDatabase
    {
        get => databaseSource;
        set
        {
            databaseSource = value;
            if (value != null)
            {
                RebuildSchemaTree();
            }
        }
    }

    public List<ISchemaBase> GetCheckedSchemas()
    {
        List<ISchemaBase> schemas = [];
        if (treeView1.CheckBoxes)
        {
            GetCheckedNodesToList(schemas, treeView1.Nodes);
        }
        return schemas;
    }

    public void SetCheckedSchemas(List<ISchemaBase> schemas) => SetCheckedNodesFromList(schemas, treeView1.Nodes);

    public void SelectAllSchemas()
    {
        foreach (TreeNode node in treeView1.Nodes)
        {
            node.Checked = true;
            SelectNode(node.Nodes);
        }
    }

    private void SelectNode(TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            node.Checked = true;
            SelectNode(node.Nodes);
        }

    }

    private void GetCheckedNodesToList(List<ISchemaBase> schemas, TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Tag != null)
            {
                if (node.Checked)
                {
                    schemas.Add(node.Tag as ISchemaBase);
                }
            }
            GetCheckedNodesToList(schemas, node.Nodes);
        }
    }

    private void SetCheckedNodesFromList(List<ISchemaBase> schemas, TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Tag != null)
            {
                node.Checked = schemas.FirstOrDefault(sch => sch.Id == (node.Tag as ISchemaBase).Id) != null;
            }
            SetCheckedNodesFromList(schemas, node.Nodes);
        }
    }

    private void ReadProperties(Type item, TreeNodeCollection nodes, ISchemaBase schema)
    {
        var pi = item.GetProperties();
        nodes.Clear();
        foreach (var p in pi)
        {
            var attrs = p.GetCustomAttributes(typeof(SchemaNodeAttribute), true);
            if (attrs.Length > 0)
            {
                var show = (SchemaNodeAttribute)attrs[0];
                var node = nodes.Add(p.Name, show.Name);
                node.ImageKey = "Folder";
                ReadPropertyDetail(node, p, schema, show);
            }
        }
    }

    private void ReadPropertyDetail(TreeNode node, PropertyInfo p, ISchemaBase schema, SchemaNodeAttribute attr)
    {
        var NodeColor = Color.Black;
        var items = (IList)p.GetValue(schema, null);
        node.Nodes.Clear();
        foreach (ISchemaBase item in items)
        {
            if (CanNodeAdd(item))
            {
                var subnode = node.Nodes.Add(item.Id.ToString(), attr.IsFullName ? item.FullName : item.Name);
                if (item.Status == ObjectStatus.Drop)
                {
                    subnode.ForeColor = Color.Red;
                    NodeColor = NodeColor == Color.Black || NodeColor == Color.Red ? Color.Red : Color.Plum;
                }
                if (item.Status == ObjectStatus.Create)
                {
                    subnode.ForeColor = Color.Green;
                    NodeColor = NodeColor == Color.Black || NodeColor == Color.Green ? Color.Green : Color.Plum;
                }
                if (item.HasState(ObjectStatus.Alter) || item.HasState(ObjectStatus.Disabled))
                {
                    subnode.ForeColor = Color.Blue;
                    NodeColor = NodeColor == Color.Black || NodeColor == Color.Blue ? Color.Blue : Color.Plum;
                }
                if (item.HasState(ObjectStatus.AlterWhitespace))
                {
                    subnode.ForeColor = Color.DarkGoldenrod;
                    NodeColor = NodeColor == Color.Black || NodeColor == Color.DarkGoldenrod ? Color.DarkGoldenrod : Color.Plum;
                }
                if (item.HasState(ObjectStatus.Rebuild))
                {
                    subnode.ForeColor = Color.Purple;
                    NodeColor = NodeColor == Color.Black || NodeColor == Color.Purple ? Color.Purple : Color.Plum;
                }
                subnode.Tag = item;
                subnode.ImageKey = attr.Image;
                subnode.SelectedImageKey = attr.Image;
            }
        }

        node.Text += items.Count == node.Nodes.Count ? $" ({items.Count})" : $" ({node.Nodes.Count} of {items.Count})";
        node.ForeColor = NodeColor;
    }

    private void RebuildSchemaTree()
    {
        var currentlySelectedNode = treeView1.SelectedNode?.Name;
        var currentTopNode = treeView1.TopNode?.Name;

        busy = true;
        treeView1.BeginUpdate();
        treeView1.Nodes.Clear();
        var databaseNode = treeView1.Nodes.Add("root", databaseSource.Name);
        ReadProperties(databaseSource.GetType(), databaseNode.Nodes, databaseSource);
        treeView1.Sort();
        databaseNode.ImageKey = "Database";
        databaseNode.Expand();

        if (currentlySelectedNode != null)
        {
            var nodes = treeView1.Nodes.Find(currentlySelectedNode, true);
            if (nodes.Any())
            {
                treeView1.SelectedNode = nodes.First();
            }
        }

        if (currentTopNode != null)
        {
            var nodes = treeView1.Nodes.Find(currentTopNode, true);
            if (nodes.Any())
            {
                treeView1.TopNode = nodes.First();
            }
        }

        treeView1.EndUpdate();
        busy = false;
        treeView1.Focus();
    }

    private bool CanNodeAdd(ISchemaBase item)
    {
        var checkedStatus = ObjectStatus.Original;
        // OriginalStatus == 0, so have to treat differently
        if (item.Status == ObjectStatus.Original && ShowUnchangedItems)
        {
            return true;
        }

        if (item.HasState(ObjectStatus.Drop) && ShowMissingItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.Drop;

        if (item.HasState(ObjectStatus.Create) && ShowNewItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.Create;

        if (item.HasState(ObjectStatus.Alter) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.Alter;

        if (item.HasState(ObjectStatus.AlterWhitespace) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.AlterWhitespace;

        if (item.HasState(ObjectStatus.AlterBody) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.AlterBody;

        if (item.HasState(ObjectStatus.Rebuild) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.Rebuild;

        if (item.HasState(ObjectStatus.RebuildDependencies) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.RebuildDependencies;

        if (item.HasState(ObjectStatus.ChangeOwner) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.ChangeOwner;

        if (item.HasState(ObjectStatus.DropOlder) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.DropOlder;

        if (item.HasState(ObjectStatus.Bind) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.Bind;

        if (item.HasState(ObjectStatus.PermissionSet) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.PermissionSet;

        if (item.HasState(ObjectStatus.Disabled) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.Disabled;

        if (item.HasState(ObjectStatus.Update) && ShowChangedItems)
        {
            return true;
        }

        checkedStatus |= ObjectStatus.Update;

        // At the end, we should have check all possible statuses.
        var expectedTotalStatus = ObjectStatus.Original;
        Enum.GetValues(typeof(ObjectStatus)).Cast<ObjectStatus>().ToList().ForEach((s) => expectedTotalStatus |= s);

        return expectedTotalStatus != checkedStatus
            ? throw new Exception(string.Format("The OjbectStatusType '{0:G}' wasn't implemented in the CanNodeAdd() method. Developer, please ensure that all values in the Enum are checked.", (ObjectStatus)(expectedTotalStatus - checkedStatus)))
            : false;
    }

    public bool ShowNewItems
    {
        get => chkNew.Checked; set => chkNew.Checked = value;
    }

    public bool ShowMissingItems
    {
        get => chkOld.Checked; set => chkOld.Checked = value;
    }

    public bool ShowChangedItems
    {
        get => chkDifferent.Checked; set => chkDifferent.Checked = value;
    }

    public bool ShowUnchangedItems
    {
        get => chkShowUnchangedItems.Checked; set => chkShowUnchangedItems.Checked = value;
    }

    private void FilterCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        if (databaseSource == null)
        {
            return;
        }

        RebuildSchemaTree();
    }

    private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (busy)
        {
            return;
        }

        var item = (ISchemaBase)e.Node.Tag;
        if (item != null)
        {
            if (item.ObjectType == ObjectType.Table
                || item.ObjectType == ObjectType.View)
            {
                ReadProperties(item.GetType(), e.Node.Nodes, item);
            }

            OnSelectItem?.Invoke(item.FullName);
        }
    }

    private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
    {
        if (e.Node.Tag == null)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
        }
    }

    public string SelectedNode => treeView1.SelectedNode == null ? null : treeView1.SelectedNode.Tag is not ISchemaBase item ? null : item.FullName;
}
