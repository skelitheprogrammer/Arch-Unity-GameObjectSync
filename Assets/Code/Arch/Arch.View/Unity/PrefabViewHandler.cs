using UnityEngine;

namespace Code.Arch.Arch.View
{
    public class PrefabViewHandler : IViewHandler<GameObject>
    {
        public void SetActiveState(in GameObject view, bool state) => view.SetActive(state);

        public void Destroy(GameObject view) => Object.Destroy(view);

        public GameObject Create(GameObject resource) => Object.Instantiate(resource);
    }
}