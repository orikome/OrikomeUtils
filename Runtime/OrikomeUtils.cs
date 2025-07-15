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
        public static IEnumerator FadeTransition(
            Transform trans,
            float transitionTime,
            float startValue,
            float endValue,
            Action callback = null
        )
        {
            float elapsed = 0;
            Image[] images = trans.GetComponentsInChildren<Image>();
            TextMeshProUGUI[] texts = trans.GetComponentsInChildren<TextMeshProUGUI>();
            while (elapsed < transitionTime)
            {
                elapsed += Time.unscaledDeltaTime;
                float change = Mathf.Lerp(startValue, endValue, elapsed / transitionTime);
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
        public static IEnumerator ZoomTransition(
            Transform trans,
            float transitionTime,
            Vector3 startSize,
            Vector3 endSize,
            Action callback = null
        )
        {
            float elapsed = 0;
            while (elapsed < transitionTime)
            {
                elapsed += Time.unscaledDeltaTime;
                float change = Mathf.Lerp(startSize.x, endSize.x, elapsed / transitionTime);
                trans.localScale = Vector3.one * change;
                yield return null;
            }

            trans.transform.localScale = endSize;
            callback?.Invoke();
        }

        public static IEnumerator SlideTransition(
            Transform objectToSlide,
            Vector3 direction,
            float duration,
            Action callback = null
        )
        {
            Vector3 startPosition = objectToSlide.position;
            Vector3 endPosition = startPosition + direction;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                objectToSlide.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            objectToSlide.position = endPosition;
            callback?.Invoke();
        }

        #region LightTransition
        public static IEnumerator LightTransition(
            Light light,
            Color targetColor,
            float transitionDuration
        )
        {
            Color startColor = light.color;
            float elapsedTime = 0.0f;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / transitionDuration;
                light.color = Color.Lerp(startColor, targetColor, t);
                yield return null;
            }
        }

        public static IEnumerator LightTransition(
            Light light,
            string hexColor,
            float transitionDuration
        )
        {
            if (ColorUtility.TryParseHtmlString(hexColor, out Color targetColor))
            {
                Color startColor = light.color;
                float elapsedTime = 0.0f;

                while (elapsedTime < transitionDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / transitionDuration;
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
        /// <param name="yHeight">Optional Y-axis height. Default is 0.</param>
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
