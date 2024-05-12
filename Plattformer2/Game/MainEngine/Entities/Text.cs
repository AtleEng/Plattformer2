using UI;

namespace Engine
{
    public class TextBox : GameEntity
    {
        public TextBox(UIText text)
        {
            name = "TextBox";
            AddComponent<UIText>(text);
        }
    }
}