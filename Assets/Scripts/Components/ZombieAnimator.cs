using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class ZombieAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator m_AnimatorController = null;

		private Coroutine m_AnimateCoroutine = null;

    	void OnEnable ()
        {
			if (m_AnimateCoroutine != null)
			{
				StopCoroutine (m_AnimateCoroutine);
			}

			m_AnimateCoroutine = StartCoroutine(Animate());
    	}
    	
        IEnumerator Animate()
        {
            while (true)
            {
				float waitTime = 0;
                int animationNumber = Random.Range(1, 4);
                // wait and start animation
                yield return new WaitForSeconds(waitTime);
                m_AnimatorController.SetBool(string.Format("shake_{0}", animationNumber), true);
                yield return new WaitForSeconds(1.5f);
                m_AnimatorController.SetBool(string.Format("shake_{0}", animationNumber), false);
            }
        }
    }
}
