using UnityEngine;

namespace FreyrEssentials.Unity
{
    static class Functions
	{
		public static void IgnoreCollision(this UnityEngine.monobeha Collider2D col1, Collider2D col2)
		=> StaticMono.Get.StartCoroutine(IgnoreCollisionRoutine(col1, col2));
		public static IEnumerator IgnoreCollisionRoutine(Collider2D col1, Collider2D col2)
		{
			Physics2D.IgnoreCollision(col1, col2, true);
			while (Physics2D.Distance(col1, col2).isOverlapped) yield return new WaitForFixedUpdate();
			Physics2D.IgnoreCollision(col1, col2, false);
		}
	}
}