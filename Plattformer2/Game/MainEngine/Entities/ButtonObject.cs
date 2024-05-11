using UI;

namespace Engine
{
    public class ButtonObject : GameEntity
    {
        //Add all diffrent components
        public ButtonObject(Action onKlick)
        {
            name = "Button";
            UIImage uIImage= new UIImage();
            AddComponent<UIImage>(uIImage);

            Button button= new Button(onKlick, uIImage);
            AddComponent<Button>(button);
        }
    }
}