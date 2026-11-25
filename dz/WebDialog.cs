namespace dz;

public class WebDialog : Dialog
{
    public override IButton CreateButton()
    {
        return new HtmlButton();
    }
}
