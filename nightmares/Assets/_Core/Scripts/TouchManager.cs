using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour{
    public float speed = 1f;
    public float sinValue = 10f;

    private Touch anyTouch;
    private bool isTouch = true;

    public Vector3 movement;
    public Vector3 lookAtPos;

    private float moveX, moveY;
    private Vector2 fingerDownPos, fingerPos, movePos;
    private Rect touchZone;
    void Start(){
        touchZone = GameObject.Find("TouchPad").GetComponent<RectTransform>().rect;
    }

    void ResetJoystick(){
        movement = Vector3.zero;
        fingerPos = Vector2.zero;
        fingerDownPos = Vector2.zero;
        movePos = Vector2.zero;
        isTouch = true;
        moveX=0;
        moveY=0;
    }

    // Update is called once per frame
    void Update(){
        if(Input.touchCount>0){
            anyTouch = Input.GetTouch((Input.touchCount -1));
            if(touchZone.Contains(anyTouch.position)){//패널 터치 체크
                if(isTouch){
                    isTouch = false;
                    fingerDownPos = anyTouch.position;//최초 터치 발생 좌표 체크
                }
                //터치 이동점 체크
                fingerPos = anyTouch.position;
                moveX = fingerPos.x - fingerDownPos.x;
                moveY = fingerPos.y - fingerDownPos.y;
                //예민한 터치를 방지하기 위해서 적은 수치는 초기화시킨다
                if(sinValue > Mathf.Abs(moveX)) {moveX =0;}
                if(sinValue > Mathf.Abs(moveY)) {moveY =0;}
                //터치가 일어나는 수치 최댓값 고정
                movePos.Set((Mathf.Clamp(moveX, -1, 1)), Mathf.Clamp(moveY, -1, 1));
                //캐릭터 이동
                movement.Set(movePos.x, 0, movePos.y);
                movement *= speed * Time.deltaTime;

                //캐릭터 이동시 이동되는 방향을 바라보도록 설정
                lookAtPos.Set(transform.position.x + moveX, 0, transform.position.z + moveY);
            }
        }else{ResetJoystick();}
    }
}
