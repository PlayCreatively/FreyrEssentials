using System.Collections;
using UnityEngine;

namespace FreyrEssentials.Unity
{
    public static class Functions
	{
		public static void IgnoreCollision(this MonoBehaviour mono, Collider2D col1, Collider2D col2)
		=> mono.StartCoroutine(IgnoreCollisionRoutine(col1, col2));
		static IEnumerator IgnoreCollisionRoutine(Collider2D col1, Collider2D col2)
		{
			Physics2D.IgnoreCollision(col1, col2, true);
			while (Physics2D.Distance(col1, col2).isOverlapped) yield return new WaitForFixedUpdate();
			Physics2D.IgnoreCollision(col1, col2, false);
		}


		public static void SquashNStretch(this Transform trans, float value)
		{
            (Sign x, Sign y) = (trans.localScale.x, trans.localScale.y);
			float magnitude = (Abs)trans.localScale.x + (Abs)trans.localScale.y;
			var scale = trans.localScale;
			scale.y = magnitude * .5f * (1f - value);
			scale.x = magnitude - scale.y;
			scale.x *= x;
			scale.y *= y;
			trans.localScale = scale;
		}
	}
}