namespace dz;

public class WindowsButton : IButton
{
    public void Render()
    {
        Console.WriteLine("[Windows Style Button] <OK>");
    }

    public void OnClick()
    {
        Console.WriteLine("Подія: Натиснуто кнопку Windows. Вікно закрито.");
    }
}
