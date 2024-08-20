using UnityEngine;

namespace EnvironmentObjects
{
    public interface ICrushable
    {
        public void Crush(Vector2 crushDirection);
    }
}