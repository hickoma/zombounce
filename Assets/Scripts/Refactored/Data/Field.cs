using LeopotamGroup.Pooling;

namespace Data
{
    public class Field
    {
        public Field(int prefabId, bool isOnScene = false)
        {
            PrefabId = prefabId;
            IsOnScene = isOnScene;
        }
        public int PrefabId;
        public bool IsOnScene;
        public IPoolObject PoolObject;
    }
}