using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

public partial class GenreEditorForm : EditorFormBase, IGenreEditorView
{
    public GenreEditorForm()
    {
        InitializeComponent();
    }

    string IGenreEditorView.GenreName
    {
        get => nameTextBox.Text;
        set => nameTextBox.Text = value;
    }

    protected override Control? GetFieldControl(string field) => field switch
    {
        nameof(IGenreEditorView.GenreName) => nameTextBox,
        _ => null
    };
}
