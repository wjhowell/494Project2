// using UnityEngine;
// using System.Collections;

// public class Flash : MonoBehaviour {

//   public bool selectable = false;
//   public float duration = 1f;
//   public float alpha = 0f;

// 	public void lerpAlpha(){
//     float lerp = Mathf.PingPong(Time.time, duration) / duration;
//     alpha = Mathf.Lerp(0f, 1f, lerp);
//     Color color = GetComponent<Renderer>().material.color;
//     color.a = alpha;
//     GetComponent<Renderer>().material.color = color;
//   }
	
// 	// Update is called once per frame
// 	void Update(){
//     if(selectable){
//       lerpAlpha();
//     }
//     else{
//       Color color = GetComponent<Renderer>().material.color;
//       color.a = 0f;
//       GetComponent<Renderer>().material.color = color;
//     }
// 	}
// }