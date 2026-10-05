using System.ComponentModel;
using BookStore.GUI.Theme;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Forms;

public partial class OrderEditorForm : Form, IOrderEditorView
{
    private OrderActions _actions = new(false, false, false, false);
    private bool _isBusy;
    private bool _isUpdatingBook;

    public OrderEditorForm()
    {
        InitializeComponent();
        DarkTheme.Apply(this);

        bookComboBox.SelectedIndexChanged += (_, _) =>
        {
            if (!_isUpdatingBook)
                BookChanged?.Invoke(this, EventArgs.Empty);
        };

        addItemButton.Click += (_, _) => AddItemRequested?.Invoke(this, EventArgs.Empty);
        removeItemButton.Click += (_, _) => RemoveItemRequested?.Invoke(this, EventArgs.Empty);
        confirmButton.Click += (_, _) => ConfirmRequested?.Invoke(this, EventArgs.Empty);
        completeButton.Click += (_, _) => CompleteRequested?.Invoke(this, EventArgs.Empty);
        cancelOrderButton.Click += (_, _) => CancelRequested?.Invoke(this, EventArgs.Empty);

        itemsGrid.SelectionChanged += (_, _) => UpdateButtons();
        itemsGrid.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Delete && removeItemButton.Enabled)
            {
                removeItemButton.PerformClick();
                e.SuppressKeyPress = true;
            }
        };
    }

    public event EventHandler? Started;

    public event EventHandler? BookChanged;

    public event EventHandler? AddItemRequested;

    public event EventHandler? RemoveItemRequested;

    public event EventHandler? ConfirmRequested;

    public event EventHandler? CompleteRequested;

    public event EventHandler? CancelRequested;

    Guid? IOrderEditorView.BookId
    {
        get => (bookComboBox.SelectedItem as LookupItem)?.Id;
        set
        {
            _isUpdatingBook = true;

            bookComboBox.SelectedItem = bookComboBox.Items
                .OfType<LookupItem>()
                .FirstOrDefault(item => item.Id == value);

            _isUpdatingBook = false;
        }
    }

    int IOrderEditorView.Quantity
    {
        get => (int)quantityUpDown.Value;
        set => quantityUpDown.Value = Math.Clamp(value, quantityUpDown.Minimum, quantityUpDown.Maximum);
    }

    decimal IOrderEditorView.UnitPrice
    {
        get => unitPriceUpDown.Value;
        set => unitPriceUpDown.Value = Math.Clamp(value, unitPriceUpDown.Minimum, unitPriceUpDown.Maximum);
    }

    public Guid? SelectedItemBookId =>
        itemsGrid.SelectedRows.Count > 0 &&
        itemsGrid.SelectedRows[0].DataBoundItem is OrderItemRow item
            ? item.BookId
            : null;

    /// <summary>Окно, поверх которого открывается диалог; задаёт фабрика представлений.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IWin32Window? ModalOwner { get; set; }

    public void ShowModal()
    {
        ShowDialog(ModalOwner);
    }

    public void SetBooks(IReadOnlyList<LookupItem> books)
    {
        bookComboBox.Items.Clear();
        bookComboBox.Items.AddRange([.. books]);
    }

    public void ShowOrder(OrderDetails order, OrderActions actions)
    {
        Text = $"Заказ #{order.Number}";
        titleLabel.Text = Text;
        statusBadge.Badge = order.Status;
        customerValueLabel.Text = order.Customer;
        createdValueLabel.Text = order.CreatedAt.ToString("dd.MM.yyyy HH:mm");
        totalValueLabel.Text = order.TotalPrice.ToString("C2");

        itemsGrid.DataSource = order.Items.ToList();

        _actions = actions;

        addItemCard.Visible = actions.CanEditItems;
        lockedHintLabel.Visible = !actions.CanEditItems;

        UpdateButtons();
    }

    public void SetBusy(bool isBusy)
    {
        _isBusy = isBusy;

        UseWaitCursor = isBusy;
        UpdateButtons();
    }

    public void ShowError(string? message)
    {
        errorBanner.ShowMessage(message);
    }

    public void ShowFieldError(string field, string message)
    {
        ShowError(message);

        Control? control = field switch
        {
            nameof(IOrderEditorView.BookId) => bookComboBox,
            nameof(IOrderEditorView.Quantity) => quantityUpDown,
            nameof(IOrderEditorView.UnitPrice) => unitPriceUpDown,
            _ => null
        };

        control?.Focus();
    }

    public bool Confirm(string title, string message, string confirmText)
    {
        return MessageDialog.Confirm(this, title, message, confirmText);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Started?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Пока запрос не завершён, окно не закрываем: иначе не увидим результат действия.
        if (_isBusy)
            e.Cancel = true;

        base.OnFormClosing(e);
    }

    private void UpdateButtons()
    {
        var isIdle = !_isBusy;

        addItemLayout.Enabled = isIdle && _actions.CanEditItems;
        removeItemButton.Enabled = isIdle && _actions.CanEditItems && SelectedItemBookId is not null;
        confirmButton.Enabled = isIdle && _actions.CanConfirm;
        completeButton.Enabled = isIdle && _actions.CanComplete;
        cancelOrderButton.Enabled = isIdle && _actions.CanCancel;
        closeButton.Enabled = isIdle;
    }
}
