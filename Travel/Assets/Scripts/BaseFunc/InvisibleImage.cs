using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Lucky
{
    /// <summary>
    /// 与新建一个Image然后alpha置为0相比，没有任何渲染压力
    /// </summary>
    [AddComponentMenu("UI/InvisibleImage")]
    public class InvisibleImage : MaskableGraphic
    {
        public override void SetMaterialDirty()
        {
            //base.SetMaterialDirty();
        }

        public override void SetVerticesDirty()
        {
            //base.SetVerticesDirty();
        }

        protected override void OnFillVBO(System.Collections.Generic.List<UIVertex> vbo)
        {
            //base.OnFillVBO(vbo);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
        protected override void UpdateGeometry()
        {

        }
        public override void Rebuild(CanvasUpdate update)
        {
            return;
        }

    }
}