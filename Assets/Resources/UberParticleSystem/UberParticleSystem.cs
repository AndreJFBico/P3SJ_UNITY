using UnityEngine;
using System.Collections;
using UberIncludes;
using System.Threading;

public class UberParticleSystem : MonoBehaviour 
{
    //QUANTITY & SIZE
    public int _Max_Particles = 100;
    public int _Emission = 10;
    public float _Time_Between_Emissions = 1;
    public float _Size = 1;
    public float _MaxVariableSize = 2;
    public float _Size_Growth_Per_Frame = 0.1f;
    public bool _Rotate_Random = false;
    public float _Rotation_Update_Speed = 2f;
    public float _MaxTheta = 2* Mathf.PI;
    public float _MaxPhi = Mathf.PI;

    //SHAPES
    public GameObject uberParticle;
    public SHAPE _Shape = SHAPE.QUAD;
    public EMITION_SHAPE _Emit_Shape = EMITION_SHAPE.LINEAR_SINGLE_POINT;

    //LIFETIME
    public float _LifeTime = 4, _LifeTimePerFrame = 0.2f;

    //VELOCITY & ACELERATION
    public Vector3 _Velocity = new Vector3(0f, 0f, 0f);
    public Vector3 _Aceleration = new Vector3(0f, 3f, 0f);

    //PARTICLE ARRAY HOLDER
    UberParticle[] _particles;

    //PRESET 3D SHAPES FOR PARTICLES 
    GameObject _uberParticleQuadPrefab;

	void Start () 
    {
        _uberParticleQuadPrefab = uberParticle;//Resources.Load("UberParticleSystem/UberParticleQuadPrefab") as GameObject;
        initParticles();
        StartCoroutine("emitParticles", 1);
        StartCoroutine("handleRotation", 1);
	}

    //Instantiates and prepares all particles at system origin.
    void initParticles()
    {
        _particles = new UberParticle[_Max_Particles];
        for (int i = 0; i < _particles.Length; i++ )
        {
            //Emition shape check.
            Vector3 instantiatePosition;
            switch (_Emit_Shape)
            {
                case EMITION_SHAPE.LINEAR_SINGLE_POINT:
                    instantiatePosition = transform.position;
                    break;
                case EMITION_SHAPE.LINEAR_CIRCULAR_AREA:
                    instantiatePosition = UberFunctions.genCircularPos(transform.position);
                    break;
                case EMITION_SHAPE.FIREWORKS_C_RANDOM:
                    instantiatePosition = transform.position;
                    break;
                default:
                    instantiatePosition = transform.position;
                    break;
            }

            //Particle Instantiation.
            GameObject instantiated = Instantiate(
                _uberParticleQuadPrefab, 
                instantiatePosition, 
                Quaternion.identity) as GameObject;

            instantiated.transform.parent = transform;
            instantiated.SetActive(false);
            UberParticle uberP = new UberParticle();

            //Particle initiation.
            uberP.initUberParticle(
                _Shape, UberFunctions.randomSize(_Size, _MaxVariableSize), 
                _Velocity, _Aceleration, 
                _LifeTime, _LifeTimePerFrame,
                _Size_Growth_Per_Frame, _Rotation_Update_Speed, 
                instantiated, _Rotate_Random,
                _MaxTheta, _MaxPhi);

            _particles[i] = uberP;
        }
    }

    //Coroutine that emits particles on a defined time schedule.
    IEnumerator emitParticles()
    { 
        while(true)
        {
            int toEmit = _Emission;
            foreach (UberParticle uberP in _particles)
            {
                if (!uberP.enabled())
                {
                    uberP.activate();
                    toEmit--;
                    if (toEmit == 0)
                        break;
                }
            }
            yield return new WaitForSeconds(_Time_Between_Emissions);
        }
    }

    //Coroutine that handles particle rotation.
    IEnumerator handleRotation()
    {
        while (true)
        {
            foreach (UberParticle uberP in _particles)
            {
                if (uberP.enabled())
                {
                    uberP.handleRotation();
                }
            }
            yield return new WaitForSeconds(_Rotation_Update_Speed);
        }
    }

    void updateParticles()
    {
        foreach (UberParticle uberP in _particles)
        {
            if (uberP.enabled())
            {
                uberP.Update();
                uberP.checkMovement(_Emit_Shape);
                uberP.checkLifeTime();
            }
        }
    }

    /*void OnDrawGizmos()
    {
         Gizmos.DrawIcon(transform.position, "Assets/Resources/Gizmos/UberGizmo", true);
    }*/

	// Update is called once per frame
    void LateUpdate() 
    {
        updateParticles();
	}
}
