using UnityEngine;
using System.Collections;
using UberIncludes;

public class UberParticle
{
    //Actual Particle.
    GameObject _uberParticle;

    //Size of the particle and growth per frame.
    float _size, _growthPerFrame;
    
    //Shape of the particle-USELESS.
    SHAPE _shape;

    //Values that are possible to change.
    Vector3 _velocity, _aceleration;
    float _lifetime, _lifetimeDecreasePerFrame;

    //Inicial values.
    Vector3 _startPosition, _iniVelocity;
    float _iniLifeTime;
    float _startTime;
    float _rotationSpeed;

    //Rotate variables.
    Quaternion rotateTo;
    bool _RandomRotate, _rotating;

    //Control variables.
    float _theta, _phi, _MaxTheta, _MaxPhi;


    GameObject Camera;

    public void initUberParticle(SHAPE shape, float size, 
        Vector3 velocity, Vector3 aceleration, 
        float lifetime, float lifetimeDecreasePerFrame, float growthPerFrame, float rotationSpeed,
        GameObject uberParticle,
        bool randomRotation, float theta, float phi)
    {
        _rotationSpeed = rotationSpeed;
        _RandomRotate = randomRotation;
        _growthPerFrame = growthPerFrame;
        _uberParticle = uberParticle;
        _uberParticle.transform.localScale = new Vector3(size, size, size);

        _startPosition = uberParticle.transform.position;
        _size = size;
        _shape = shape;
        _velocity = velocity;
        _iniVelocity = _velocity;
        _aceleration = aceleration;
        _lifetime = lifetime;
        _iniLifeTime = lifetime;
        _lifetimeDecreasePerFrame = lifetimeDecreasePerFrame;
        _startTime = Time.realtimeSinceStartup;
        _theta = Random.Range(0f, theta);
        _phi = Random.Range(0f, phi);
        _rotating = false;

        Camera = GameObject.FindWithTag("MainCamera");
    }

    public void checkMovement(EMITION_SHAPE shape)
    {
        switch (shape)
        {
            case EMITION_SHAPE.LINEAR_SINGLE_POINT:
                _velocity = UberFunctions.eulerVelocity(_velocity, _aceleration);
                break;
            case EMITION_SHAPE.LINEAR_CIRCULAR_AREA:
                _velocity = UberFunctions.eulerVelocity(_velocity, _aceleration);
                break;
            case EMITION_SHAPE.FIREWORKS_C_RANDOM:
                _velocity = UberFunctions.circularVelocity(Random.Range(0.2f, 1f), Random.Range(0f, 2 * Mathf.PI), Random.Range(0f, Mathf.PI));
                break;
            case EMITION_SHAPE.FIREWORKS_C:
                _velocity = UberFunctions.circularVelocity(Random.Range(0.2f, 1f), _theta, _phi);
                break;
            default:

                break;
        }
        _uberParticle.transform.position = _uberParticle.transform.position + _velocity * Time.deltaTime;
    }

    public void Update()
    {
        //Actualise fading
        checkFading();
        //check if time has reached the end
        checkRegularLifeTime();

        //Grow particles acording to metrics
        checkParticleGrowth();
    }

    //Checks to see if all particles face towards camera position or rotate randomly.
    public void handleRotation()
    { 
        if (!_RandomRotate)
        {
            if(Camera!= null)
            {
                var n = Camera.transform.position - _uberParticle.transform.position;
                _uberParticle.transform.rotation = Quaternion.LookRotation(n);
            }

        }
        else if (!_rotating)
        {
            rotateTo = Random.rotation;
            _rotating = true;
        }
        else
        {
            _uberParticle.transform.rotation = Quaternion.Slerp(_uberParticle.transform.rotation, rotateTo, Time.deltaTime);
            if(_uberParticle.transform.rotation == rotateTo)
            {
                _rotating = false;
            }
        }
    }

    void checkParticleGrowth()
    {
        _uberParticle.transform.localScale += new Vector3(_growthPerFrame, _growthPerFrame, _growthPerFrame);
    }

    void checkFading()
    {
        Color color = _uberParticle.renderer.material.GetColor("_TintColor");
        color.a = (_lifetime * 1f) / _iniLifeTime;
        _uberParticle.renderer.material.SetColor("_TintColor", color);
    }

    void reset()
    {
        _uberParticle.transform.localScale = new Vector3(_size, _size, _size);
        _lifetime = _iniLifeTime;
        _velocity = _iniVelocity;
        _uberParticle.transform.position = _startPosition;
        _uberParticle.gameObject.SetActive(false);
    }

    void checkRegularLifeTime()
    {
        if(Time.realtimeSinceStartup - _startTime > _iniLifeTime)
        {
            reset();
        }
        else
        {
            _lifetime -= Time.deltaTime;
        }
    }

    public bool enabled()
    {
        return _uberParticle.gameObject.activeSelf;
    }

    public void activate()
    {
        _uberParticle.SetActive(true);
        _startTime = Time.realtimeSinceStartup;
    }

    public void checkLifeTime()
    {
        _lifetime-= _lifetimeDecreasePerFrame;
        if(_lifetime < 0)
        {
            reset();
        }
    }
}

