using UnityEngine;

namespace Helper
{
    public static class Extensions
    {
        public static void SetCamera(this Camera cam)
        {
            var baseScale = 1080f / 1920f;
            var currentScale = Screen.width / (Screen.height * 1f);
            if (currentScale <= 2)
                cam.orthographicSize *= baseScale / currentScale;
        }
    }
}
