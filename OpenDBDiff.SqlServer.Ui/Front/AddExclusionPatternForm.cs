using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.SqlServer.Schema.Options;

namespace OpenDBDiff.SqlServer.Ui
{
    public partial class AddExclusionPatternForm : Form
    {
        private readonly SqlOption sqlOption;
        private readonly int indexFilter;

        public AddExclusionPatternForm(SqlOption sqlOption)
            : this(sqlOption, -1)
        { }

        public AddExclusionPatternForm(SqlOption sqlOption, int Index)
        {
            InitializeComponent();

            PopulateObjectTypeDropDownList();

            this.sqlOption = sqlOption;
            indexFilter = Index;
            if (indexFilter != -1)
            {
                txtFilter.Text = sqlOption.Filters.Items[indexFilter].FilterPattern;
                cboObjects.SelectedValue = sqlOption.Filters.Items[indexFilter].ObjectType;
            }
        }

        private string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            return attributes != null &&
                attributes.Length > 0
                ? attributes[0].Description
                : value.ToString();
        }

        private void PopulateObjectTypeDropDownList()
        {
            var data = Enum.GetValues(typeof(ObjectType)).Cast<ObjectType>()
                .Select(ot => new { ObjectType = ot, Description = GetEnumDescription(ot) })
                .OrderBy(a => a.Description)
                .ToList();

            cboObjects.DataSource = data;
            cboObjects.DisplayMember = "Description";
            cboObjects.ValueMember = "ObjectType";
        }

        private void CancelFormButton_Click(object sender, EventArgs e) => this.Close();

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (cboObjects.SelectedItem == null)
            {
                _ = MessageBox.Show(this, "All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var fi = new SqlOptionFilterItem((ObjectType)Enum.Parse(typeof(ObjectType), cboObjects.SelectedValue.ToString(), true), txtFilter.Text);

            if (sqlOption.Filters.Items.Contains(fi))
            {
                _ = MessageBox.Show(this, string.Format("The list of name filters already includes an entry for text '{0}' of type '{1}'", fi.FilterPattern, fi.ObjectType.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (indexFilter == -1)
            {
                sqlOption.Filters.Items.Add(fi);
            }
            else
            {
                sqlOption.Filters.Items[indexFilter].FilterPattern = fi.FilterPattern;
                sqlOption.Filters.Items[indexFilter].ObjectType = fi.ObjectType;
            }
            HandlerHelper.RaiseOnChange();

            this.Close();
        }
    }
}
