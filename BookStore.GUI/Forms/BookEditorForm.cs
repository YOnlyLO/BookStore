using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

public partial class BookEditorForm : EditorFormBase, IBookEditorView
{
    public BookEditorForm()
    {
        InitializeComponent();
    }

    string IBookEditorView.BookTitle
    {
        get => titleTextBox.Text;
        set => titleTextBox.Text = value;
    }

    string IBookEditorView.Author
    {
        get => authorTextBox.Text;
        set => authorTextBox.Text = value;
    }

    string IBookEditorView.Description
    {
        get => descriptionTextBox.Text;
        set => descriptionTextBox.Text = value;
    }

    decimal IBookEditorView.Price
    {
        get => priceUpDown.Value;
        set => priceUpDown.Value = Math.Clamp(value, priceUpDown.Minimum, priceUpDown.Maximum);
    }

    int IBookEditorView.StockQuantity
    {
        get => (int)stockUpDown.Value;
        set => stockUpDown.Value = Math.Clamp(value, stockUpDown.Minimum, stockUpDown.Maximum);
    }

    bool IBookEditorView.IsStockQuantityEditable
    {
        set
        {
            stockUpDown.Enabled = value;
            stockHintLabel.Visible = !value;
        }
    }

    Guid? IBookEditorView.GenreId
    {
        get => (genreComboBox.SelectedItem as LookupItem)?.Id;
        set => genreComboBox.SelectedItem = genreComboBox.Items
            .OfType<LookupItem>()
            .FirstOrDefault(item => item.Id == value);
    }

    public void SetGenres(IReadOnlyList<LookupItem> genres)
    {
        genreComboBox.Items.Clear();
        genreComboBox.Items.AddRange([.. genres]);
    }

    protected override Control? GetFieldControl(string field) => field switch
    {
        nameof(IBookEditorView.BookTitle) => titleTextBox,
        nameof(IBookEditorView.Author) => authorTextBox,
        nameof(IBookEditorView.Description) => descriptionTextBox,
        nameof(IBookEditorView.Price) => priceUpDown,
        nameof(IBookEditorView.StockQuantity) => stockUpDown,
        nameof(IBookEditorView.GenreId) => genreComboBox,
        _ => null
    };
}
