namespace BookStore.GUI.ViewModels;

/// <summary>Элемент выпадающего списка: идентификатор записи и её отображаемое имя.</summary>
public sealed record LookupItem(
    Guid Id,
    string Text)
{
    public override string ToString() => Text;
}
