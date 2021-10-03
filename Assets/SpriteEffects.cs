using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace fx
{
    public class SpriteEffects : MonoBehaviour
    {
        public void Fadeout(SpriteRenderer sp, float t = 1f)
        {
          /*  while (sp.color.a != 0)
            {
                delay<SpriteRenderer>(t, fade, sp);
            }*/
            for(int i = 0; i < 255; i+=0)
            {
                StartCoroutine(fadeDelay(t, sp));
            }
        }

        IEnumerator fadeDelay(float t, SpriteRenderer s)
        {
            yield return new WaitForSeconds(t);
            Color c = s.color;
            c.a--;
            s.color = c;
        }




        /*
        int fade(SpriteRenderer s)
        {
            Color c = s.color;
            c.a--;
            s.color = c;
            return 0;
        }

        IEnumerator delay<T>(float t, Func<T, int> f, T value)
        {
            yield return new WaitForSeconds(t);
            f(value);
        }*/
    }
}
