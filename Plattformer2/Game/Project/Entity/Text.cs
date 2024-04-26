using UI;

namespace Engine
{
    public class TextBox : GameEntity
    {
        //Add all diffrent components
        public TextBox()
        {
            name = "TextBox";

            UIText uIText = new()
            {
                Layer = 10,
                text = "Press space to start",
                pos = new(10, 10)
            };
        }
        public TextBox(UIText text) : this()
        {
            AddComponent<UIText>(text);
        }
    }
}