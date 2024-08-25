using UnityEngine;

namespace EnvironmentObjects.Crushable
{
    public interface ICrushable
    {
        public void Crush(Vector2 crushDirection);
    }
}