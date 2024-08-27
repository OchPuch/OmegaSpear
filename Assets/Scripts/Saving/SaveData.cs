using System;


namespace Saving
{
    [Serializable]
    public struct SaveData
    {
        public string sceneName;
        public CharacterSaveData characterSaveData;
        public float seconds;
        
        [Serializable]
        public struct CharacterSaveData
        {
            public float positionX;
            public float positionY;
        }
    }
}