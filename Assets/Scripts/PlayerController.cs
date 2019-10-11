using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2;
    public float runSpeed = 4;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //calcula un vector dos cuando presionamos una tecla de movimeinto en horizontal y vertical
        Vector2 inputDir = input.normalized;    //calcula la hipotenusa del vector dando como resultado la direccion del movimiento, bastante util para calcular un movimiento en diagonal

        if(inputDir != Vector2.zero)    //evita que el personaje vuelva a la posicion inicial
            {
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y)* Mathf.Rad2Deg; //calcula el angulo al cual va a rotar el personaje
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);  //transforma y suaviza la rotacion del personaje a la direccion que le estamos ordenando
            }

        bool walking = Input.GetKey(KeyCode.LeftShift); //si se presiona shift izquierdo, el personaje caminara
        float targetSpeed = ((walking)? walkSpeed : runSpeed) * inputDir.magnitude; //cambia la velocidad del personaje y si no se esta presionado alguna tecla, la velocidad se vuelve cero
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothTime, speedSmoothTime);

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);    //desplazamiento del personaje

        float animatorSpeedPercent = ((walking)?.5f:1f * inputDir.magnitude);
        animator.SetFloat("Speed", animatorSpeedPercent);      
    }
}
