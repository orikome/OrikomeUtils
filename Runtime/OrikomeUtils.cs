using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OrikomeUtils
{
    public static class TransitionUtils
    {
        /// <summary>
        /// Fades the color alpha values of each TextMeshProUGUI and Image component found in the children of the transform.
        /// </summary>
        /// <param name="trans">The Transform containing the components to fade.</param>
        /// <param name="transitionTime">The duration of the fade transition in seconds.</param>
        /// <param name="startValue">The starting alpha value (0-1).</param>
        /// <param name="endValue">The ending alpha value (0-1).</param>
        /// <param name="callback">Optional callback action to invoke when transition completes.</param>
        /// <param name="curve">Optional animation curve to apply to the transition. Uses linear interpolation if null.</param>
        /// <returns>An IEnumerator for use with StartCoroutine.</returns>
        public static IEnumerator FadeTransition(
            Transform trans,
            float transitionTime,
            float startValue,
            float endValue,
            Action callback = null,
            AnimationCurve curve = null
        )
        {
            float elapsed = 0;
            Image[] images = trans.GetComponentsInChildren<Image>();
            TextMeshProUGUI[] texts = trans.GetComponentsInChildren<TextMeshProUGUI>();
            while (elapsed < transitionTime)
            {
                elapsed += Time.unscaledDeltaTime;
                float normalizedTime = elapsed / transitionTime;
                float t = curve != null ? curve.Evaluate(normalizedTime) : normalizedTime;
                float change = Mathf.Lerp(startValue, endValue, t);

                foreach (Image img in images)
                {
                    Color tmpColor = img.color;
                    tmpColor.a = change;
                    img.color = tmpColor;
                }
                foreach (TextMeshProUGUI text in texts)
                {
                    Color tmpColor = text.color;
                    tmpColor.a = change;
                    text.color = tmpColor;
                }
                yield return null;
            }

            callback?.Invoke();
        }

        /// <summary>
        /// Scales the transform over a certain period of time.
        /// </summary>
        /// <param name="trans">The Transform to scale.</param>
        /// <param name="transitionTime">The duration of the scaling transition in seconds.</param>
        /// <param name="startSize">The starting scale vector.</param>
        /// <param name="endSize">The target scale vector.</param>
        /// <param name="callback">Optional callback action to invoke when transition completes.</param>
        /// <param name="curve">Optional animation curve to apply to the transition. Uses linear interpolation if null.</param>
        /// <returns>An IEnumerator for use with StartCoroutine.</returns>
        public static IEnumerator ZoomTransition(
            Transform trans,
            float transitionTime,
            Vector3 startSize,
            Vector3 endSize,
            Action callback = null,
            AnimationCurve curve = null
        )
        {
            float elapsed = 0;
            while (elapsed < transitionTime)
            {
                elapsed += Time.unscaledDeltaTime;
                float normalizedTime = elapsed / transitionTime;
                float t = curve != null ? curve.Evaluate(normalizedTime) : normalizedTime;
                trans.localScale = Vector3.Lerp(startSize, endSize, t);
                yield return null;
            }

            trans.transform.localScale = endSize;
            callback?.Invoke();
        }

        /// <summary>
        /// Slides a Transform in a specified direction over time.
        /// </summary>
        /// <param name="objectToSlide">The Transform to slide.</param>
        /// <param name="direction">The direction and distance vector to slide.</param>
        /// <param name="duration">The duration of the slide transition in seconds.</param>
        /// <param name="callback">Optional callback action to invoke when transition completes.</param>
        /// <param name="curve">Optional animation curve to apply to the transition. Uses linear interpolation if null.</param>
        /// <returns>An IEnumerator for use with StartCoroutine.</returns>
        public static IEnumerator SlideTransition(
            Transform objectToSlide,
            Vector3 direction,
            float duration,
            Action callback = null,
            AnimationCurve curve = null
        )
        {
            Vector3 startPosition = objectToSlide.position;
            Vector3 endPosition = startPosition + direction;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
                float t = curve != null ? curve.Evaluate(normalizedTime) : normalizedTime;
                objectToSlide.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            objectToSlide.position = endPosition;
            callback?.Invoke();
        }

        #region LightTransition

        /// <summary>
        /// Transitions a light's color to a target color over time.
        /// </summary>
        /// <param name="light">The Light component to modify.</param>
        /// <param name="targetColor">The target Color to transition to.</param>
        /// <param name="transitionDuration">The duration of the transition in seconds.</param>
        /// <param name="curve">Optional animation curve to apply to the transition. Uses linear interpolation if null.</param>
        /// <returns>An IEnumerator for use with StartCoroutine.</returns>
        public static IEnumerator LightTransition(
            Light light,
            Color targetColor,
            float transitionDuration,
            AnimationCurve curve = null
        )
        {
            Color startColor = light.color;
            float elapsedTime = 0.0f;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / transitionDuration);
                float t = curve != null ? curve.Evaluate(normalizedTime) : normalizedTime;
                light.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }
        }

        /// <summary>
        /// Transitions a light's color to a target color specified by a hex string over time.
        /// </summary>
        /// <param name="light">The Light component to modify.</param>
        /// <param name="hexColor">The target color as a hex string (e.g., "#FF0000" for red).</param>
        /// <param name="transitionDuration">The duration of the transition in seconds.</param>
        /// <param name="curve">Optional animation curve to apply to the transition. Uses linear interpolation if null.</param>
        /// <returns>An IEnumerator for use with StartCoroutine.</returns>
        public static IEnumerator LightTransition(
            Light light,
            string hexColor,
            float transitionDuration,
            AnimationCurve curve = null
        )
        {
            if (ColorUtility.TryParseHtmlString(hexColor, out Color targetColor))
            {
                Color startColor = light.color;
                float elapsedTime = 0.0f;

                while (elapsedTime < transitionDuration)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    float normalizedTime = Mathf.Clamp01(elapsedTime / transitionDuration);
                    float t = curve != null ? curve.Evaluate(normalizedTime) : normalizedTime;
                    light.color = Color.Lerp(startColor, targetColor, t);
                    yield return null;
                }
            }
            else
            {
                Debug.LogError("Invalid hex color string");
                yield break;
            }
        }
        #endregion
    }

    public static class LayerMaskUtils
    {
        /// <summary>
        /// Creates a LayerMask from one or more layer names.
        /// </summary>
        /// <param name="layerNames">Array of layer names to include in the mask.</param>
        /// <returns>A LayerMask containing the specified layers.</returns>
        public static LayerMask CreateMask(params string[] layerNames)
        {
            // Selectively ignore objects when casting a ray.
            LayerMask layerMask = 0;

            foreach (string layerName in layerNames)
            {
                // Find the index of the layer that matches the string.
                int layerIndex = LayerMask.NameToLayer(layerName);
                if (layerIndex == -1)
                {
                    Debug.LogError("Layer not found: " + layerName);
                    continue;
                }

                // Use a bitwise OR operation to add the layer to the LayerMask.
                layerMask |= (1 << layerIndex);
            }

            return layerMask;
        }

        /// <summary>
        /// Checks if a layer is included in a LayerMask.
        /// </summary>
        /// <param name="layer">The layer index to check.</param>
        /// <param name="mask">The LayerMask to check against.</param>
        /// <returns>True if the layer is in the mask; otherwise, false.</returns>
        public static bool IsLayerInMask(int layer, LayerMask mask)
        {
            return (mask.value & (1 << layer)) != 0;
        }
    }

    public static class GeneralUtils
    {
        /// <summary>
        /// Calculates the squared distance between two Vector3 points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The squared distance between the two points.</returns>
        public static float GetDistanceSquared(Vector3 point1, Vector3 point2)
        {
            // Using the instance method sqrMagnitude is slightly faster
            // than the static Vector3.SqrMagnitude method due to reduced overhead.
            return (point1 - point2).sqrMagnitude;
        }

        /// <summary>
        /// Checks if two Transforms are within a specified range of each other.
        /// </summary>
        /// <param name="transform1">The first Transform.</param>
        /// <param name="transform2">The second Transform.</param>
        /// <param name="range">The distance range to compare.</param>
        /// <returns>True if the Transforms are within the range; otherwise, false.</returns>
        public static bool IsInRange(Transform transform1, Transform transform2, float range)
        {
            // Calculate the squared range for performance reasons.
            float rangeSquared = range * range;

            // Determine if the distance between the two Transforms is less than the squared range.
            return (transform1.position - transform2.position).sqrMagnitude < rangeSquared;
        }

        /// <summary>
        /// Generates a random position within a circle around a center point, with a specified radius and optional Y-axis height.
        /// </summary>
        /// <param name="center">Center point of the circle.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="yHeight">Y-axis height of the resulting position. Default is 0.</param>
        /// <returns>A Vector3 representing the random position.</returns>
        public static Vector3 GetRandomPositionInCircle(
            Vector3 center,
            float radius,
            float yHeight = 0
        )
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 position = new Vector3(randomCircle.x, yHeight, randomCircle.y) + center;

            return position;
        }

        /// <summary>
        /// Returns a position that is offset from the given transform's position.
        /// </summary>
        /// <param name="transform">The reference transform.</param>
        /// <param name="xOffset">The offset on the X axis. Default is 0.</param>
        /// <param name="yOffset">The offset on the Y axis. Default is 0.</param>
        /// <param name="zOffset">The offset on the Z axis. Default is 0.</param>
        /// <returns>A Vector3 representing the offset position.</returns>
        public static Vector3 GetPositionWithOffset(
            Transform transform,
            float xOffset = 0,
            float yOffset = 0,
            float zOffset = 0
        )
        {
            return transform.position + new Vector3(xOffset, yOffset, zOffset);
        }

        /// <summary>
        /// Generates a random point outside the camera's frustum, with an optional offset.
        /// </summary>
        /// <param name="targetPos">The transform to use as a reference point.</param>
        /// <param name="cam">The camera whose frustum to avoid.</param>
        /// <param name="offsetDistance">Optional additional distance from the frustum edge. Default is 0.</param>
        /// <returns>A Vector3 representing a random position outside the camera's view. Returns Vector3.zero if no suitable position is found after 10 attempts.</returns>
        public static Vector3 GetRandomPositionOutsideFrustum(
            Transform targetPos,
            Camera cam,
            float offsetDistance = 0f
        )
        {
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

            for (int i = 0; i < 10; i++)
            {
                float radius = 40f;
                Vector3 candidatePos = GetRandomPositionInCircle(targetPos.position, radius);

                if (
                    !GeometryUtility.TestPlanesAABB(
                        frustumPlanes,
                        new Bounds(candidatePos, Vector3.one)
                    )
                )
                {
                    return candidatePos;
                }
            }

            return Vector3.zero;
        }
    }
}
