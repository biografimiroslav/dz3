namespace dz;

public class HtmlButton : IButton
{
    public void Render()
    {
        Console.WriteLine("<button class=\"btn-primary\" type=\"submit\">OK</button>");
    }

    public void OnClick()
    {
        Console.WriteLine("Подія: onClick() у браузері. Форму надіслано.");
    }
}
