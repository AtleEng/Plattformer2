using System.Numerics;
using Raylib_cs;
using UI;

namespace Engine
{
    public class ButtonObject : GameEntity
    {
        public UIImage uIImage;
        public UIText uIText;
        public Button button;
        //Add all diffrent components
        public ButtonObject(Action onKlick)
        {
            name = "Button";
            uIImage = new UIImage();
            AddComponent<UIImage>(uIImage);

            uIText = new UIText
            {
                Layer = 11,
                text = "Button",
                color = Color.Black
            };
            EntityManager.SpawnEntity(new TextBox(uIText), Vector2.Zero, Vector2.One, transform);

            button = new Button(onKlick, uIImage);
            AddComponent<Button>(button);
        }
    }
}