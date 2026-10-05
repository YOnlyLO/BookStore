namespace BookStore.GUI.ViewModels;

/// <summary>Метка-«пилюля»: например, статус заказа. В таблице рисуется цветом <see cref="Tone"/>.</summary>
public sealed record Badge(
    string Text,
    Tone Tone)
{
    public override string ToString() => Text;
}
