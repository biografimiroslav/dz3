namespace dz;

public abstract class Dialog
{
    public abstract IButton CreateButton();

    public void Render()
    {
        Console.WriteLine("---------------------------------------------");
        Console.WriteLine("Відображення діалогового вікна...");
        
        IButton okButton = CreateButton();
        okButton.OnClick();
        okButton.Render();

        Console.WriteLine("---------------------------------------------\n");
    }
}
