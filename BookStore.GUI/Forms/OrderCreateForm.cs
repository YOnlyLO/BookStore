using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

public partial class OrderCreateForm : EditorFormBase, IOrderCreateView
{
    public OrderCreateForm()
    {
        InitializeComponent();
    }

    public Guid? UserId => (userComboBox.SelectedItem as LookupItem)?.Id;

    public void SetUsers(IReadOnlyList<LookupItem> users)
    {
        userComboBox.Items.Clear();
        userComboBox.Items.AddRange([.. users]);
    }

    protected override Control? GetFieldControl(string field) => field switch
    {
        nameof(UserId) => userComboBox,
        _ => null
    };
}
